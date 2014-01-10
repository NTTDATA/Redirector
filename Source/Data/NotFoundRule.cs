using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTTData.Redirector.Data
{
    public class NotFoundRule
    {
        public string Pattern { get; set; }
        public string NewUrl { get; set; }
        public string Type { get; set; }
        public string Method { get; set; }
        public string StatusCode { get; set; }
        public string Site { get; set; }

        public NotFoundRule(string site, string pattern, string newUrl, string type, string method, string statusCode)
        {
            Site = site;
            Pattern = pattern;
            NewUrl = newUrl;
            Type = type;
            Method = method;
            StatusCode = statusCode;
        }
    }
}
