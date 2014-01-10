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
using Sitecore.Globalization;

namespace NTTData.Redirector.Commands
{
    public class DeleteRedirect : Command, ISupportsContinuation
    {

        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");

            ClientPipelineArgs args = new ClientPipelineArgs();

            args.Parameters["entryids"] = context.Parameters["entryids"];

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

            ListString list = new ListString(args.Parameters["entryids"]);

            if (args.IsPostBack)
            {
                if (args.HasResult && args.Result == "yes")
                {
                    if (list.Items.Any())
                    {
                        foreach (string id in list.Items)
                        {
                            Guid entryId = MainUtil.GetGuid(id, Guid.Empty);

                            if (entryId != Guid.Empty)
                            {
                                RedirectManager.DeleteRedirect(entryId);
                            }
                        }

                        Client.AjaxScriptManager.Dispatch("redirects:refresh");
                    }
                }
            }
            else
            {
                if (list.Any())
                {
                    if (list.Count == 1)
                    {
                        SheerResponse.Confirm(Translate.Text("Are you sure you want to delete this redirect?"));
                    }
                    else
                    {
                        SheerResponse.Confirm(Translate.Text("Are you sure you want to delete these {0} redirects?", new object[] { list.Count }));
                    }
                    args.WaitForPostBack();

                }
            }
        }
    }
}
