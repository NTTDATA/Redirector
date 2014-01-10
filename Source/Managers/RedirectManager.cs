using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTTData.Redirector.Providers;
using Sitecore.Data;
using NTTData.Redirector.Data;
using Sitecore.Data.Items;
using Sitecore.Configuration;

namespace NTTData.Redirector.Managers
{
    public static class RedirectManager
    {

        private static readonly ProviderHelper<RedirectProvider, RedirectProviderCollection> _providerHelper;




        public static RedirectProvider Provider { get { return _providerHelper.Provider; } }

        static RedirectManager()
        {
            _providerHelper = new ProviderHelper<RedirectProvider, RedirectProviderCollection>("redirectManager");

        }



        public static void CreateRedirect(string site, string oldPath, ID itemId)
        {
            Provider.CreateRedirect(site, oldPath, itemId);
        }

        public static void CreateRedirect(string site, string oldPath, ID itemId, string queryString)
        {
            Provider.CreateRedirect(site, oldPath, itemId, queryString);
        }

        public static void UpdateRedirect(RedirectEntry entry)
        {
            Provider.UpdateRedirect(entry);
        }

        public static RedirectEntry GetRedirect(string site, string oldPath)
        {
            return Provider.GetRedirect(site, oldPath);
        }

        public static RedirectEntry GetRedirect(Guid redirectEntryId)
        {
            return Provider.GetRedirect(redirectEntryId);
        }

        public static IEnumerable<RedirectEntry> GetRedirects(ID itemId)
        {
            return Provider.GetRedirects(itemId);
        }

        public static IQueryable<RedirectEntry> GetRedirects(string site)
        {
            return Provider.GetRedirects(site);
        }

        public static IQueryable<RedirectEntry> GetRedirects()
        {
            return Provider.GetRedirects();
        }

        public static void DeleteRedirects(ID itemId)
        {
            Provider.DeleteRedirects(itemId);
        }

        public static void DeleteRedirect(Guid redirectEntryId)
        {
            Provider.DeleteRedirect(redirectEntryId);
        }

        public static void DeleteRedirect(string site, string oldPath)
        {
            Provider.DeleteRedirect(site, oldPath);
        }

        public static void DeleteSiteRedirects(string site)
        {
            Provider.DeleteSiteRedirects(site);
        }

        public static string ResolveSiteName(Item item)
        {
            return Provider.ResolveSiteName(item);
        }

        public static NotFoundRule ResolveRedirectRule(string siteName, string path)
        {
            return Provider.ResolveRedirectRule(siteName, path);
        }
    }
}
