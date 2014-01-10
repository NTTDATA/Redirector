using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Diagnostics;
using Sitecore;
using Sitecore.Web;
using NTTData.Redirector.Managers;
using NTTData.Redirector.Data;
using Sitecore.Data;
using Sitecore.Web.UI.Sheer;
using Sitecore.Text;
using System.Web;
using Sitecore.Sites;

namespace NTTData.Redirector.ClientUI
{
    public class EditRedirectorForm : DialogForm
    {
        protected TreePicker ItemPicker { get; set; }
        protected Edit OldUrlEdit { get; set; }
        protected Edit QueryStringEdit { get; set; }
        protected Combobox SiteCombobox { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");

            base.OnLoad(e);

            if (!Context.ClientPage.IsEvent)
            {
                if (MainUtil.IsID(HttpUtility.UrlDecode(WebUtil.GetQueryString("itemid"))))
                    ItemPicker.Value = HttpUtility.UrlDecode(WebUtil.GetQueryString("itemid"));
                OldUrlEdit.Value = HttpUtility.UrlDecode(WebUtil.GetQueryString("oldpath"));
                QueryStringEdit.Value = HttpUtility.UrlDecode(WebUtil.GetQueryString("querystring"));

                string selectedSite = HttpUtility.UrlDecode(WebUtil.GetQueryString("site"));
                if (string.IsNullOrEmpty(selectedSite))
                    selectedSite = "website";

                foreach (var site in SiteManager.GetSites())
                {
                    ListItem li = new ListItem()
                    {
                        Header = site.Name,
                        Value = site.Name
                    };
                    if (site.Name.ToLower() == selectedSite)
                        li.Selected = true;

                    SiteCombobox.Controls.Add(li);
                }


            }
        }

        protected override void OnOK(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            if (string.IsNullOrEmpty(ItemPicker.Value))
            {
                Context.ClientPage.ClientResponse.Alert("Please Select a Target Item");
                return;
            }

            if (string.IsNullOrEmpty(OldUrlEdit.Value))
            {
                Context.ClientPage.ClientResponse.Alert("Please enter an old Url for redirection");
                return;
            }

            UrlString results = new UrlString();
            results["itemid"] = ItemPicker.Value;
            results["oldpath"] = OldUrlEdit.Value;
            results["querystring"] = QueryStringEdit.Value;
            results["site"] = SiteCombobox.Value;
            SheerResponse.SetDialogValue(results.ToString());
            base.OnOK(sender, args);
        }

    }
}
