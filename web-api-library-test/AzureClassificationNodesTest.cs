using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api_library;
using web_api_library.Extensions;

namespace web_api_library_test
{

    [TestClass]
    public class AzureClassificationNodesTest
    {
        private const string azureProjectUrl = "https://dev.azure.com/<tfs-account>";
        private AzureContext _context;
        internal const string projectName = "project-name";

        private AzureContext GetContext()
        {
            return _context;
        }

        private void SetContext(AzureContext value)
        {
            _context = value;
        }

        public AzureClassificationNodesTest()
        {
            SetContext(new AzureContext(new Uri(azureProjectUrl), projectName));
        }

        [TestMethod]
        public void TestGetAreaListWith2LevelDepth()
        {
            WorkItemClassificationNode areaNode = this.GetContext().ListAreas();

            Assert.IsNotNull(areaNode);
            Assert.IsNotNull(areaNode.Children);
            Assert.AreEqual(2, areaNode.Children.Count());
            Assert.AreEqual("area-1", areaNode.Children.FirstOrDefault().Name);
        }
    }
}
