using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using web_api_library;
using web_api_library.Constants;
using web_api_library.Elements;
using web_api_library.Extensions;

namespace web_api_library_test
{
    [TestClass]
    public class AzureContextTest
    {
        //* Append your account name
        internal const string azureProjectUrl = "https://dev.azure.com/<tfs-account>";
        internal const string projectName = "<project-name>";

        AzureContext azureContext = null;

        public AzureContextTest()
        {
            azureContext = new AzureContext(new Uri(azureProjectUrl), projectName);
        }

        ~AzureContextTest()
        {
            azureContext.Dispose();
        }

        [TestMethod]
        public void TestAzureContextFormation()
        {
            using (AzureContext context = new AzureContext(new Uri(azureProjectUrl), projectName))
            {
                Assert.IsNotNull(context);
            }
        }
        [TestMethod]
        public void TestCreateBug()
        {
            string constantValue = string.Format("{0} at {1} UTC", DateTime.UtcNow.ToLongDateString(), DateTime.UtcNow.ToLongTimeString());

            //This is to list fields and their values for bug creation
            IEnumerable<JsonPatchItem> bugItemValues = FieldValueMap.CreateInstance(Maps.TFSEnumToJsonMap)
                                                        .Append(FieldName.Title, string.Format("Test Bug Created via API on '{0}'", constantValue))
                                                        .Append(FieldName.AssignedTo, "Bankey Sharma")
                                                        .Append(FieldName.AreaPath, "area-path-value")
                                                        .Append(FieldName.IterationPath, "iteration-value")
                                                        .Append(FieldName.Tags, "available-tags")
                                                        .Append(FieldName.ReproSteps, string.Format("Sample repro steps.\r\n{0}", constantValue))
                                                        .Append(FieldName.Priority, "1")
                                                        .Append(FieldName.Severity, "2 - High")
                                                        .Build();

            azureContext.CreateBug(bugItemValues);
        }

        [TestMethod]
        public void TestCreateIssue()
        {

            string constantValue = string.Format("{0} at {1} UTC", DateTime.UtcNow.ToLongDateString(), DateTime.UtcNow.ToLongTimeString());

            //This is to list fields and their values for bug creation
            IEnumerable<JsonPatchItem> bugItemValues = FieldValueMap.CreateInstance(Maps.TFSEnumToJsonMap)
                                                        .Append(FieldName.Title, string.Format("Test Issue Created via API on '{0}'", constantValue))
                                                        .Append(FieldName.AssignedTo, "Bankey Sharma")
                                                        .Append(FieldName.AreaPath, "area-path-value")
                                                        .Append(FieldName.IterationPath, "iteration-value")
                                                        .Append(FieldName.Tags, "available-tags")
                                                        .Append(FieldName.Description, string.Format("Sample description steps.\r\n{0}", constantValue))
                                                        .Append(FieldName.Priority, "1")
                                                        .Append(FieldName.Severity, "2 - High")
                                                        .Build();

            azureContext.CreateIssue(bugItemValues);
        }

        [TestMethod]
        public void TestFindProject()
        {
            Guid expectedProjectId = new Guid("abcdeff61-d357-42c8-a339-8b80824pqrst");

            azureContext.FindProject(projectName);
            TeamProjectReference defaultProject = azureContext.GetDefaultProject();

            Assert.IsNotNull(defaultProject);
            Assert.IsTrue(expectedProjectId.Equals(defaultProject.Id));
        }

        [TestMethod]
        public void TestDefaultProject()
        {
            Guid expectedProjectId = new Guid("abcdeff61-d357-42c8-a339-8b80824pqrst");

            Assert.IsTrue(expectedProjectId.Equals(azureContext.GetDefaultProject().Id));
        }


        [TestMethod]
        public void TestGetTeams()
        {
            IEnumerable<WebApiTeam> teams;

            teams = azureContext.GetTeams();

            Assert.IsNotNull(teams);
            Assert.IsTrue(teams.Count() > 0);
            Assert.IsNotNull(teams.FirstOrDefault(item => item.Name.CompareTo("team-1") == 0));
            Assert.IsNotNull(teams.FirstOrDefault(item => item.Name.CompareTo("team-2") == 0));
            Assert.IsNotNull(teams.FirstOrDefault(item => item.Name.CompareTo("team-3") == 0));

        }

        [TestMethod]
        public void TestGetAllTeamMembers()
        {
            IEnumerable<TeamMember> teamMembers;

            teamMembers = azureContext.GetAllTeamMembers();

            Assert.IsNotNull(teamMembers);
            Assert.IsTrue(teamMembers.Count() > 0);
            Assert.IsNotNull(teamMembers.FirstOrDefault(item => item.Identity.DisplayName.CompareTo("Bankey Sharma") == 0));
            
            //* Should not be duplicate records
            Assert.IsTrue(teamMembers.GroupBy(item => item.Identity.Id).Where(item => item.Count() > 1).Count() == 0);

        }

        [TestMethod]
        public void TestGetAssignToUsers()
        {
            string[] teamMembers = azureContext.GetAssignToUsers();

            Assert.IsNotNull(teamMembers);
            Assert.IsTrue(teamMembers.Count() > 0);
            Assert.IsNotNull(teamMembers.FirstOrDefault(item => item.CompareTo("Bankey Sharma") == 0));

            //* Should not be duplicate records
            Assert.IsTrue(teamMembers.GroupBy(item => item).Where(item => item.Count() > 1).Count() == 0);

        }
    }
}
