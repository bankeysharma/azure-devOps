using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using web_api_library.Elements;

namespace web_api_library
{
    public class AzureHttpLogger : DelegatingHandler
    {
        public static readonly string PropertyOutputFilePath = "$outputFilePath";   // value is a string indicating the folder to output files to
        public static readonly string PropertySuppressOutput = "$suppressOutput";   // value is a boolan indicating whether to suppress output
                                                                                    //public static readonly string PropertyOutputToConsole = "$outputToConsole"; // value is a boolan indicating whether to output JSON to the console
        public static readonly string PropertyOperationName = "$operationName";   // value is a string indicating the logical name of the operation. If output is enabled, this value is used to produce the output file name.

        private JsonSerializerSettings serializerSettings;

        private static HashSet<string> s_excludedHeaders = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "x-VSS-PerfData",
            "x-TFS-Session",
            "x-VSS-E2EID",
            "x-VSS-Agent",
            "authorization",
            "x-TFS-ProcessId",
            "x-VSS-UserData",
            "activityId",
            "p3P",
            "x-Powered-By",
            "cookie",
            "x-TFS-FedAuthRedirect",
            "strict-Transport-Security",
            "x-FRAME-OPTIONS",
            "x-Content-Type-Options",
            "x-AspNet-Version",
            "server",
            "pragma",
            "vary",
            "x-MSEdge-Ref",
            "cache-Control",
            "date",
            "user-Agent",
            "accept-Language"
        };

        private static HashSet<string> s_combinableHeaders = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase)
        {
            "user-Agent"
        };

        public AzureHttpLogger()
        {
            serializerSettings = new JsonSerializerSettings();

            serializerSettings.Formatting = Formatting.Indented;
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }

        private static Dictionary<string, string> ProcessHeaders(HttpHeaders headers)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            foreach (var h in headers.Where(kvp => { return !s_excludedHeaders.Contains(kvp.Key); }))
            {
                if (h.Value.Count() == 1)
                {
                    ret[h.Key] = h.Value.First();
                }
                else
                {
                    if (s_combinableHeaders.Contains(h.Key))
                    {
                        ret[h.Key] = String.Join(" ", h.Value);
                    }
                    else
                    {
                        ret[h.Key] = h.Value.ToString();
                    }
                }
            }

            return ret;
        }

        private static bool ResponseHasContent(HttpResponseMessage response)
        {
            if (response != null &&
                response.StatusCode != HttpStatusCode.NoContent &&
                response.Content != null &&
                response.Content.Headers != null &&
                (!response.Content.Headers.ContentLength.HasValue ||
                 (response.Content.Headers.ContentLength.HasValue && response.Content.Headers.ContentLength != 0)))
            {
                return true;
            }

            return false;
        }

        private static bool IsJsonResponse(HttpResponseMessage response)
        {
            if (ResponseHasContent(response)
                && response.Content.Headers != null && response.Content.Headers.ContentType != null
                && !String.IsNullOrEmpty(response.Content.Headers.ContentType.MediaType))
            {
                return (0 == String.Compare("application/json", response.Content.Headers.ContentType.MediaType, StringComparison.OrdinalIgnoreCase));
            }

            return false;
        }

        public static void SetSuppressOutput(AzureContext context, bool suppress)
        {
            context.SetValue<bool>(PropertySuppressOutput, suppress);
        }

        public static void SetOperationName(AzureContext context, string name)
        {
            context.SetValue<string>(PropertyOperationName, name);
        }

        public static void ResetOperationName(AzureContext context)
        {
            context.RemoveValue(PropertyOperationName);
        }
    }

    public class ClientSampleHttpLoggerOutputSuppression : IDisposable
    {
        private bool OriginalSuppressValue;
        private AzureContext azureContext;

        public ClientSampleHttpLoggerOutputSuppression(AzureContext context)
        {
            azureContext = context;
            if (!context.TryGetValue<bool>(AzureHttpLogger.PropertySuppressOutput, out OriginalSuppressValue))
            {
                OriginalSuppressValue = false;
            }
            AzureHttpLogger.SetSuppressOutput(context, true);
        }

        public void Dispose()
        {
            AzureHttpLogger.SetSuppressOutput(azureContext, OriginalSuppressValue);
        }
    }

    [DataContract]
    class ApiRequestResponseMetdata : AzureMethodInfo
    {
        [DataMember(Name = "x-ms-vss-request-method")]
        public String HttpMethod;

        [DataMember(Name = "x-ms-vss-request-url")]
        public String RequestUrl;

        [DataMember]
        public Dictionary<string, object> Parameters;

        [DataMember]
        public Dictionary<string, ApiResponseMetadata> Responses;

        [DataMember(Name = "x-ms-vss-generated")]
        public bool Generated;

        [DataMember(Name = "x-ms-vss-generated-date")]
        public DateTime GeneratedDate;

        [DataMember(Name = "x-ms-vss-format")]
        public int Format { get { return 1; } }
    }

    [DataContract]
    class ApiResponseMetadata
    {
        [DataMember]
        public Dictionary<string, string> Headers;

        [DataMember(EmitDefaultValue = false)]
        public Object Body;
    }
}
