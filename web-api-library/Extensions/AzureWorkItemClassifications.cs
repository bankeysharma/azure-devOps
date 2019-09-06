using System;
using System.Collections.Generic;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi;

using web_api_library.Elements;
using web_api_library.Extensions;

namespace web_api_library.Extensions
{
    /// <summary>
    /// 
    /// Samples showing how to work with work item tracking areas and iterations.
    /// 
    /// See https://www.visualstudio.com/docs/integrate/api/wit/classification-nodes for more details.
    /// 
    /// </summary>
    [AzureClassificationNodeAttribute(WitConstants.WorkItemTrackingWebConstants.RestAreaName, WitConstants.WorkItemTrackingRestResources.ClassificationNodes)]
    public static class AzureWorkItemClassifications
    {
        public static WorkItemClassificationNode ListAreas(this AzureContext azureContext)
        {
            int depth = 2;

            return azureContext.GetHttpClientWorkItem().GetClassificationNodeAsync(
                azureContext.GetDefaultProject().Name,
                TreeStructureGroup.Areas,
                null,
                depth).Result;
        }

        public static WorkItemClassificationNode ListIterations(this AzureContext azureContext)
        {
            int depth = 4;

            WorkItemClassificationNode rootIterationNode = azureContext.GetHttpClientWorkItem().GetClassificationNodeAsync(
                azureContext.GetDefaultProject().Name,
                TreeStructureGroup.Iterations,
                null,
                depth).Result;

            ShowNodeTree(rootIterationNode);

            return rootIterationNode;
        }

        public static WorkItemClassificationNode GetArea(this AzureContext azureContext)
        {
            Guid projectId;
            string areaPath;

            // Get values from previous sample method that created a sample iteration
            //azureContext.TryGetValue<Guid>("$newAreaProjectId", out projectId);
            projectId = azureContext.GetDefaultProject().Id;
            azureContext.TryGetValue<string>("$newAreaName", out areaPath);

            return azureContext.GetHttpClientWorkItem().GetClassificationNodeAsync(
                projectId.ToString(),
                TreeStructureGroup.Areas,
                areaPath,
                5).Result;

        }

        public static WorkItemClassificationNode GetIteration(this AzureContext azureContext)
        {
            Guid projectId;
            string iterationPath;

            // Get values from previous sample method that created a sample iteration
            //azureContext.TryGetValue<Guid>("$newIterationProjectId", out projectId);
            projectId = azureContext.GetDefaultProject().Id;
            azureContext.TryGetValue<string>("$newIterationName", out iterationPath);
            
            return azureContext.GetHttpClientWorkItem().GetClassificationNodeAsync(
                projectId.ToString(),
                TreeStructureGroup.Iterations,
                iterationPath,
                4).Result;
        }

        private static void ShowNodeTree(WorkItemClassificationNode node, string path = "")
        {
            path = path + "/" + node.Name;
            Console.WriteLine(path);

            if (node.Children != null)
            {
                foreach (var child in node.Children)
                {
                    ShowNodeTree(child, path);
                }
            }
        }

        [AzureClassificationNodeMethod]
        public static WorkItemClassificationNode GetFullTree(this AzureContext azureContext, string project, TreeStructureGroup type)
        {
            return azureContext.GetHttpClientWorkItem().GetClassificationNodeAsync(project, type, null, 1000).Result;

        }
    }
}
