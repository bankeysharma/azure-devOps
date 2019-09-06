using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_api_library.Constants
{
    public static class Maps
    {
        public static readonly Dictionary<FieldName, string> TFSEnumToJsonMap = new Dictionary<FieldName, string>() {
            [FieldName.Title] = "/fields/System.Title",
            [FieldName.AreaPath] = "/fields/System.AreaPath",
            [FieldName.AssignedTo] = "/fields/System.AssignedTo",
            [FieldName.IterationPath] = "/fields/System.IterationPath",
            [FieldName.Description] = "/fields/System.Description",
            [FieldName.Tags] = "/fields/System.Tags",
            [FieldName.Priority] = "/fields/Microsoft.VSTS.Common.Priority",
            [FieldName.Severity] = "/fields/Microsoft.VSTS.Common.Severity",
            [FieldName.ReproSteps] = "/fields/Microsoft.VSTS.TCM.ReproSteps"
        };
    }
}
