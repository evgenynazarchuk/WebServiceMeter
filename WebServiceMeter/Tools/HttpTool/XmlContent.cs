using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;

namespace WebServiceMeter.Tools.HttpTool
{
    public class XmlContent : StringContent
    {
        public XmlContent(string content)
            : this(content, Encoding.UTF8) { }

        public XmlContent(string content, Encoding? encoding)
            : base(content, encoding, "application/xml") { }
    }
}
