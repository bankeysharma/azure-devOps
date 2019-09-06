using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_api_library.Extensions
{
    /// <summary>
    /// Common methods used across multiple areas to provide common functions like 
    /// getting a sample project to run samples against.
    /// 
    /// Note: area or resource specific helpers should go into an area-specific helper class or into the client sample class itself.
    ///
    /// </summary>
    public static class AzureContexTeamProject
    {
        public static TeamProjectReference FindProject(this AzureContext context, string projectName) {
            TeamProjectReference project;

            using (new ClientSampleHttpLoggerOutputSuppression(context)) {
                project = context.GetHttpClientProject().GetProjects(null).Result.FirstOrDefault(item => item.Name.CompareTo(projectName) == 0);
            }

            return project;
        }

        public static IEnumerable<WebApiTeam> GetTeams(this AzureContext context) {
            IEnumerable<WebApiTeam> teams;

            using (new ClientSampleHttpLoggerOutputSuppression(context)) {
                teams = context.GetHttpClientTeam().GetTeamsAsync(projectId: context.GetDefaultProject().Id.ToString()).Result;
            }

            return teams;
        }

        public static IEnumerable<TeamMember> GetAllTeamMembers(this AzureContext context)
        {
            List<TeamMember> allMembers = new List<TeamMember>();

            TeamHttpClient teamClient = context.GetHttpClientTeam();
            string defaultProjectId = context.GetDefaultProject().Id.ToString();
            
            foreach (WebApiTeam team in context.GetTeams()) {
                allMembers.AddRange(teamClient.GetTeamMembersWithExtendedPropertiesAsync(defaultProjectId, team.Id.ToString()).Result);
            }

            return allMembers.Where(item => !item.Identity.Inactive).GroupBy(item => item.Identity.Id).Select(groupedItem => groupedItem.First());
        }
    }
}