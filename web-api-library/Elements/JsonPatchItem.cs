using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_api_library.Elements
{
    public struct JsonPatchItem
    {
        public string Path { get; set; }
        public string Value { get; set; }
    }
}
