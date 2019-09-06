using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
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
    public static class AzureContextWorkItems
    {

        /// WorkItem Type should be configuration as user may have derived workitems
        /// 

        /// <summary>
        /// Create a bug using the .NET client library
        /// </summary>
        /// <returns>Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem</returns>    
        public static WorkItem CreateBug(this AzureContext azureContext, IEnumerable<JsonPatchItem> itemValues)
        {
            return azureContext.GetHttpClientWorkItem().CreateWorkItemAsync(itemValues.ToJsonPatchDocument(), azureContext.GetDefaultProject().Name, "Bug").Result;
        }

        public static WorkItem CreateIssue(this AzureContext azureContext, IEnumerable<JsonPatchItem> itemValues)
        {
            return azureContext.GetHttpClientWorkItem().CreateWorkItemAsync(itemValues.ToJsonPatchDocument(), azureContext.GetDefaultProject().Name, "Issue").Result;
        }
    }
}
