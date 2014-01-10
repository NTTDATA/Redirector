using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Sitecore.Web;

namespace NTTData.Redirector.Utils
{
    public static class RedirectUtils
    {
        public static void Do301Redirect(HttpResponse response, string targetURL)
        {
            response.StatusCode = 0x12d;
            response.StatusDescription = "301 Moved Permanently";
            response.RedirectLocation = targetURL;
            response.End();
        }

        public static void Do302Redirect(HttpResponse response, string targetURL)
        {
            response.StatusCode = 0x12e;
            response.StatusDescription = "302 Moved Permanently";
            response.RedirectLocation = targetURL;
            response.End();
        }

        public static void Do404Redirect(HttpResponse response, string targetURL)
        {
            WebUtil.Redirect(targetURL, false);
        }
    }
}
