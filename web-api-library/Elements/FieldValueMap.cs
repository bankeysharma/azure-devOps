using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api_library.Constants;

namespace web_api_library.Elements
{
    public class FieldValueMap
    {
        private static IDictionary<FieldName, string> FieldsJsonMapping { get; set; }
        protected IDictionary<FieldName, string> FieldValues { get; set; }

        private FieldValueMap(IDictionary<FieldName, string> jsonMapping) {
            FieldsJsonMapping = jsonMapping;
            FieldValues = new Dictionary<FieldName, string>();
        }

        public static FieldValueMap CreateInstance(Dictionary<FieldName, string> mapValues) {
            return new FieldValueMap(mapValues);
        }

        public FieldValueMap Append(FieldName field, string value) {
            this.FieldValues.Add(field, value);
            return this;
        }

        public IEnumerable<JsonPatchItem> Build() {
            if (FieldValues == null || FieldValues.Count == 0) return new JsonPatchItem[0];

            return FieldValues.Select(item => new JsonPatchItem() { Path = FieldsJsonMapping[item.Key], Value = item.Value });
        }
    }
}
