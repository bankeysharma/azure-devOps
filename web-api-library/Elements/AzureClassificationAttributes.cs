using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace web_api_library.Elements
{

    /// <summary>
    /// Interface representing a client sample method. Provides a way to discover client samples for a particular area, resource, or operation.
    /// </summary>
    public interface IAzureMethodInfo
    {
        string Area { get; }

        string Resource { get; }

        string Operation { get; }
    }


    [DataContract]
    public class AzureMethodInfo : IAzureMethodInfo
    {
        [DataMember(EmitDefaultValue = false, Name = "x-ms-vss-area")]
        public string Area { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "x-ms-vss-resource")]
        public string Resource { get; set; }

        [DataMember(EmitDefaultValue = false, Name = "x-ms-vss-operation")]
        public string Operation { get; set; }
    }

    /// <summary>
    /// Attribute applied to all client samples. Optionally indicates the API "area" and/or "resource" the sample is associatd with.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AzureClassificationNodeAttribute : ExportAttribute
    {
        public string Area { get; private set; }

        public string Resource { get; private set; }

        public AzureClassificationNodeAttribute(String area = null, String resource = null) : base(typeof(AzureClassificationNodeBase))
        {
            this.Area = area;
            this.Resource = resource;
        }
    }

    /// <summary>
    /// Attribute applied to methods within a client sample. Allow overriding the area or resource of the containing client sample.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AzureClassificationNodeMethodAttribute : Attribute, IAzureMethodInfo
    {
        public string Area { get; internal set; }

        public string Resource { get; internal set; }

        public string Operation { get; internal set; }

        public AzureClassificationNodeMethodAttribute(String area = null, String resource = null, String operation = null)
        {
            this.Area = area;
            this.Resource = resource;
            this.Operation = operation;
        }
    }
}
