using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace WebServiceMeter.Tools.HttpTool
{
    public class JsonContent : StringContent
    {
        public JsonContent(string content)
            : this(content, Encoding.UTF8) { }

        public JsonContent(string content, Encoding? encoding)
            : base(content, encoding, "application/json") { }
    }
}
