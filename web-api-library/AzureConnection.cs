using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace web_api_library
{
    public class AzureConnection : VssConnection
    {
        public AzureConnection(Uri baseUrl, VssCredentials credentials) : base(baseUrl, credentials)
        {
        }

        public AzureConnection(Uri baseUrl, VssCredentials credentials, VssHttpRequestSettings settings) : base(baseUrl, credentials, settings)
        {
        }

        public AzureConnection(Uri baseUrl, VssHttpMessageHandler innerHandler, IEnumerable<DelegatingHandler> delegatingHandlers) : base(baseUrl, innerHandler, delegatingHandlers)
        {
        }
    }
}