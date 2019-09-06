using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_api_library.Elements
{
    /// <summary>
    /// Base class that all client samples extend from.
    /// </summary>
    [InheritedExport]
    public class AzureClassificationNodeBase
    {
        public AzureContext Context { get; set; }

        public AzureClassificationNodeBase(AzureContext context)
        {
            Context = context;
        }
    }
}
