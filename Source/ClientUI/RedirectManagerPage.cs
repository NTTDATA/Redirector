using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Data.Items;
using ComponentArt.Web.UI;

using Sitecore.Web.UI.WebControls.Ribbons;
using System.Web.UI.HtmlControls;
using Sitecore.Web.UI.XamlSharp.Ajax;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.Sheer;
using Sitecore;
using Sitecore.Web.UI.WebControls;
using Sitecore.Web.UI.Grids;
using Sitecore.Security.Accounts;
using Sitecore.Extensions;
using Sitecore.Text;
using Sitecore.Security;
using Sitecore.Resources;
using System.Web;
using System.Web.UI.WebControls;
using NTTData.Redirector.Data;
using NTTData.Redirector.Managers;
using Sitecore.Data;
using Sitecore.Configuration;

namespace NTTData.Redirector.ClientUI
{
    public class RedirectManagerPage : System.Web.UI.Page, IHasCommandContext
    {

        // Fields
        protected GridServerTemplate CommentTemplate;
        protected GridServerTemplate FullNameTemplate;
        protected Sitecore.Web.UI.HtmlControls.Border GridContainer;
        protected ClientTemplate LoadingFeedbackTemplate;
        protected ClientTemplate LocalNameTemplate;
        protected Ribbon Ribbon;
        protected ClientTemplate SliderTemplate;
        protected HtmlForm RedirectManagerForm;
        protected Grid Redirects;

        protected Database db = Sitecore.Context.ContentDatabase;

        // Methods
        private void Current_OnExecute(object sender, AjaxCommandEventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");
            if (args.Name == "redirects:redirectdeleted")
            {
                SheerResponse.Eval("refresh()");
            }
            else if (args.Name == "redirects:refresh")
            {
                SheerResponse.Eval("refresh()");
            }
            
        }

        protected override void OnInit(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnInit(e);
            Sitecore.Context.State.DataBind = false;
            this.Redirects.ItemDataBound += new Grid.ItemDataBoundEventHandler(this.Redirects_ItemDataBound);
            this.Redirects.ItemContentCreated += new Grid.ItemContentCreatedEventHandler(this.RedirectsItemContentCreated);
            Client.AjaxScriptManager.OnExecute += new AjaxScriptManager.ExecuteDelegate(this.Current_OnExecute);
        }

        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);
            //Assert.CanRunApplication("Security/User Manager");
            IEnumerable<RedirectGridEntry> redirects = RedirectManager.GetRedirects().ToList().Select(re => new RedirectGridEntry(re)).Where(re => !String.IsNullOrEmpty(re.Site));
            ComponentArtGridHandler<RedirectGridEntry>.Manage(this.Redirects, new GridSource<RedirectGridEntry>(redirects), this.RebindRequired);
            this.Redirects.LocalizeGrid();
        }

        CommandContext IHasCommandContext.GetCommandContext()
        {
            CommandContext context = new CommandContext();
            Item itemNotNull = Client.GetItemNotNull("/sitecore/content/Applications/Redirector/Redirect Manager/Ribbon", Client.CoreDatabase);
            context.RibbonSourceUri = itemNotNull.Uri;
            string selectedValue = GridUtil.GetSelectedValue("Redirects");

            context.Parameters["entryids"] = selectedValue;
            return context;
        }

        private void Redirects_ItemDataBound(object sender, GridItemDataBoundEventArgs e)
        {
            RedirectGridEntry redirectEntry = e.DataItem as RedirectGridEntry;
            Assert.IsNotNull(redirectEntry, "redirect");
        }

        private void RedirectsItemContentCreated(object sender, GridItemContentCreatedEventArgs e)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(e, "e");
        }

        // Properties
        private bool RebindRequired
        {
            get
            {
                return ((!this.Page.IsPostBack && (base.Request.QueryString["Cart_Redirects_Callback"] != "yes")) || (this.Page.Request.Params["requireRebind"] == "true"));
            }
        }


        


        public class RedirectGridEntry
        {
            public string EntryId { get; set; }
            public string ItemIcon { get; set; }
            public string Site { get; set; }
            public string OldPath { get; set; }
            public string NewPath { get; set; }
            public string QueryString { get; set; }
            public string Name
            {
                get
                {
                    return EntryId.ToString();
                }
            }
            public RedirectGridEntry() { }

            public RedirectGridEntry(RedirectEntry entry)
            {
                Database db = Sitecore.Context.ContentDatabase;

                Item item = db.GetItem(new ID(entry.ItemID));

                if (item != null)
                {
                    this.EntryId = entry.RedirectEntryId.ToString();
                    this.ItemIcon = Themes.MapTheme(item.Appearance.Icon);
                    this.Site = entry.Site;
                    this.OldPath = entry.OldPath;
                    this.NewPath = item.Paths.ContentPath;
                    this.QueryString = entry.QueryString;
                }
            }

        }


    }
}
