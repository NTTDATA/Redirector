using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Diagnostics;
using Sitecore;
using System.Text.RegularExpressions;
using System.Web;
using NTTData.Redirector.Data;
using NTTData.Redirector.Managers;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.Links;
using Sitecore.Text;
using NTTData.Redirector.Utils;
using Sitecore.Reflection;
using Sitecore.Web;

namespace NTTData.Redirector.Pipelines
{
    public class RedirectResolver : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            string url = args.Context.Request.Url.PathAndQuery;

            // if ItemResolver failed to find an Item
            if (((Context.Item == null) && string.IsNullOrEmpty(Context.Page.FilePath)) || url.Contains("404;"))
            {
                // extract 404 url
                if (url.Contains("404;"))
                    url = Regex.Replace(HttpUtility.UrlDecode(url), ".*404;(.*)", "$1");

                // find an Item Redirect
                RedirectEntry redirect = RedirectManager.GetRedirect(Sitecore.Context.GetSiteName(), url);
                if (redirect != null)
                {
                    UrlString newUrl = null;
                    Item item = Context.Database.GetItem(new ID(redirect.ItemID));
                    if (item != null)
                    {
                        UrlOptions options = LinkManager.GetDefaultUrlOptions();
                        newUrl = new UrlString(LinkManager.GetItemUrl(item, options));
                    }

                    if (newUrl != null && !string.IsNullOrEmpty(redirect.QueryString))
                    {
                        var qsParameters = StringUtil.ParseNameValueCollection(redirect.QueryString, '&', '=');
                        foreach (string key in qsParameters.AllKeys)
                        {
                            newUrl[key] = qsParameters[key];
                        }
                    }


                    // avoid looping
                    if (newUrl != null && string.Compare(url, newUrl.ToString(), true) != 0)
                    {
                        args.AbortPipeline();
                        RedirectUtils.Do301Redirect(args.Context.Response, newUrl.ToString());
                    }
                }
                // no Item redirect found, try a NotFoundRule
                else if (url != null)
                {
                    NotFoundRule rule = RedirectManager.ResolveRedirectRule(Sitecore.Context.GetSiteName(), url);
                    if (rule != null)
                        ExecuteRule(url, rule, args);
                }
            }
        }

        public void ExecuteRule(string url, NotFoundRule rule, HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(url, "url");
            Assert.ArgumentNotNull(rule, "rule");
            Assert.ArgumentNotNull(args, "args");

            // execute a sub pipeline
            if (!string.IsNullOrEmpty(rule.Pattern) && !string.IsNullOrEmpty(rule.Method))
            {
                object command = ReflectionUtil.CreateObject(rule.Type, new object[] { });
                if (command != null)
                {
                    try
                    {
                        ReflectionUtil.CallMethod(command, rule.Method, new object[] { args });
                    }
                    catch (System.Threading.ThreadAbortException) 
                    {
                        //don't log. this is expected.
                    }
                    catch (Exception ex)
                    {
                        Log.Error("NotFoundRule Error", ex, this);
                    }
                }
            }
            // or redirect to new url or regex
            else if (rule.NewUrl.Length > 0)
            {
                Regex reg = new Regex(rule.Pattern, RegexOptions.IgnoreCase);
                string newUrl = reg.Replace(url, rule.NewUrl);
                switch (rule.StatusCode)
                {
                    case "301":
                        RedirectUtils.Do301Redirect(args.Context.Response, newUrl);
                        break;
                    case "302":
                        RedirectUtils.Do302Redirect(args.Context.Response, newUrl);
                        break;
                    case "404":
                        RedirectUtils.Do404Redirect(args.Context.Response, newUrl);
                        break;
                    default:
                        WebUtil.Redirect(newUrl, false);
                        break;
                }
                args.Context.Response.End();
            }
        }
    }
}
