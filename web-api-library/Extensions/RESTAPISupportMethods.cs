using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api_library.Elements;

namespace web_api_library.Extensions
{
    public static class RESTAPISupportMethods
    {

        public static JsonPatchDocument ToJsonPatchDocument(this IEnumerable<JsonPatchItem> valuePair)
        {
            JsonPatchDocument jsonPatchDocument = new JsonPatchDocument();
            jsonPatchDocument.AddRange(valuePair.Select(item => new JsonPatchOperation() {
                Operation = Operation.Add,
                Path = item.Path,
                Value = item.Value
            }));

            return jsonPatchDocument;
        }
    }
}
