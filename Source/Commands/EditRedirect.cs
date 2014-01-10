using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.XamlSharp.Continuations;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.WebControls;
using Sitecore.Web.UI.Sheer;
using Sitecore;
using Sitecore.Text;
using System.Collections.Specialized;
using Sitecore.Data;
using System.Web;
using NTTData.Redirector.Data;
using NTTData.Redirector.Managers;
using Sitecore.Data.Items;

namespace NTTData.Redirector.Commands
{
    public class EditRedirect : Command, ISupportsContinuation
    {

        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");

            ClientPipelineArgs args = new ClientPipelineArgs();

            ListString list = new ListString(context.Parameters["entryids"]);

            bool isNew = context.Parameters["new"] == "1";

            if (list.Items.Any() && !isNew)
            {
                Guid entryId = MainUtil.GetGuid(list.Items.FirstOrDefault(), Guid.Empty);
                if (entryId != Guid.Empty)
                {
                    RedirectEntry entry = RedirectManager.GetRedirect(entryId);
                    if (entry != null)
                    {
                        args.Parameters["entryid"] = entryId.ToString();
                        args.Parameters["oldpath"] = entry.OldPath;
                        args.Parameters["itemid"] = entry.ItemID.ToString();
                        args.Parameters["querystring"] = entry.QueryString;
                        args.Parameters["site"] = entry.Site;
                    }
                }
            }


            ContinuationManager current = ContinuationManager.Current;
            if (current != null)
            {
                current.Start(this, "Run", args);
            }
            else
            {
                Context.ClientPage.Start(this, "Run", args);
            }

        }

        protected void Run(ClientPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            if (args.IsPostBack)
            {
                if (args.HasResult && args.Result != "undefined")
                {

                    UrlString results = new UrlString(args.Result);
                    Database db = Sitecore.Context.ContentDatabase;

                    Guid entryId = MainUtil.GetGuid(args.Parameters["entryid"], Guid.Empty);
                    Item item = db.GetItem(HttpUtility.UrlDecode(results["itemid"]));
                    string oldPath = HttpUtility.UrlDecode(results["oldpath"]);
                    string querystring = HttpUtility.UrlDecode(results["querystring"]);
                    string site = HttpUtility.UrlDecode(results["site"]);

                    if (item != null)
                    {
                        string siteName = site;// RedirectManager.ResolveSiteName(item);

                        if (entryId != Guid.Empty)
                        {
                            RedirectEntry entry = RedirectManager.GetRedirect(entryId);
                            if (entry != null)
                            {
                                entry.ItemID = item.ID.ToGuid();
                                entry.OldPath = oldPath;
                                entry.QueryString = querystring;
                                entry.Site = siteName;

                                RedirectManager.UpdateRedirect(entry);
                            }
                        }
                        else
                        {
                            RedirectManager.CreateRedirect(siteName, oldPath, item.ID, querystring);
                        }


                    }

                    Client.AjaxScriptManager.Dispatch("redirects:refresh");
                }
            }
            else
            {
                UrlString url = new UrlString(UIUtil.GetUri("control:EditRedirectForm"));

                if (!string.IsNullOrEmpty(args.Parameters["itemid"]))
                    url["itemid"] = args.Parameters["itemid"];
                if (!string.IsNullOrEmpty(args.Parameters["oldpath"]))
                    url["oldpath"] = args.Parameters["oldpath"];
                if (!string.IsNullOrEmpty(args.Parameters["querystring"]))
                    url["querystring"] = args.Parameters["querystring"];
                if (!string.IsNullOrEmpty(args.Parameters["site"]))
                    url["site"] = args.Parameters["site"];


                SheerResponse.ShowModalDialog(url.ToString(), true);
                args.WaitForPostBack();
            }
        }
    }
}
