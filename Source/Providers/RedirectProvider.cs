using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data;
using NTTData.Redirector.Data;
using Sitecore.Data.Items;
using Sitecore.Sites;
using Sitecore.Links;
using System.Reflection;
using Sitecore.Web;
using System.Xml;
using System.Configuration.Provider;
using Sitecore.Xml;
using Sitecore.Diagnostics;
using System.Text.RegularExpressions;
using Sitecore;
using Sitecore.Text;

namespace NTTData.Redirector.Providers
{
    public class RedirectProvider : ProviderBase
    {

        public static List<NotFoundRule> _rules = null;
        public static readonly object _ruleLock = new object();
        public static List<NotFoundRule> Rules
        {
            get
            {
                return _rules;
            }
        }

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            XmlNode processorNode = Sitecore.Configuration.Factory.GetConfigNode("redirectManager");
            if (processorNode != null)
            {
                _rules = new List<NotFoundRule>();
                foreach (XmlNode node in XmlUtil.GetChildElements("rule", processorNode))
                {
                    _rules.Add(new NotFoundRule(
                        XmlUtil.GetAttribute("site", node, ""),
                        XmlUtil.GetAttribute("pattern", node, ""),
                        XmlUtil.GetAttribute("newUrl", node, ""),
                        XmlUtil.GetAttribute("type", node, ""),
                        XmlUtil.GetAttribute("method", node, ""),
                        XmlUtil.GetAttribute("statusCode", node, "")
                        ));
                }
            }


            base.Initialize(name, config);
        }

        public virtual void CreateRedirect(string site, string oldPath, ID itemId)
        {
            CreateRedirect(site, oldPath, itemId, string.Empty);
        }

        public virtual void CreateRedirect(string site, string oldPath, ID itemId, string queryString)
        {
            DeleteRedirect(site, oldPath);

            using (RedirectorDbContext db = new RedirectorDbContext())
            {
                var redirect = new RedirectEntry()
                {
                    RedirectEntryId = Guid.NewGuid(),
                    Site = site,
                    OldPath = oldPath,
                    ItemID = itemId.ToGuid(),
                    QueryString = queryString
                };

                db.RedirectEntries.Add(redirect);

                db.SaveChanges();
            }
        }

        public virtual void UpdateRedirect(RedirectEntry entry)
        {
            using (RedirectorDbContext db = new RedirectorDbContext())
            {
                db.RedirectEntries.Attach(entry);
                db.Entry<RedirectEntry>(entry).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public virtual RedirectEntry GetRedirect(string site, string oldPath)
        {
            return GetRedirects(site).SingleOrDefault(r => r.OldPath.Equals(oldPath, StringComparison.CurrentCultureIgnoreCase));
        }

        public virtual RedirectEntry GetRedirect(Guid redirectEntryId)
        {
            using (RedirectorDbContext db = new RedirectorDbContext())
            {
                return db.RedirectEntries.Find(redirectEntryId);
            }
        }

        public virtual IEnumerable<RedirectEntry> GetRedirects(ID itemId)
        {
            using (RedirectorDbContext db = new RedirectorDbContext())
            {
                return db.RedirectEntries.Where(r => r.ItemID == itemId.ToGuid()).OrderBy(r => r.ItemID);
            }
        }

        public virtual IQueryable<RedirectEntry> GetRedirects(string site)
        {
            RedirectorDbContext db = new RedirectorDbContext();
            return db.RedirectEntries.Where(r => r.Site.Equals(site, StringComparison.CurrentCultureIgnoreCase)).OrderBy(r => r.ItemID);
        }

        public virtual IQueryable<RedirectEntry> GetRedirects()
        {
            RedirectorDbContext db = new RedirectorDbContext();
            return db.RedirectEntries.OrderBy(r => r.ItemID);
        }

        public virtual void DeleteRedirects(ID itemId)
        {
            using (RedirectorDbContext db = new RedirectorDbContext())
            {
                var redirects = GetRedirects(itemId);

                foreach (var redirect in redirects)
                {
                    db.RedirectEntries.Remove(redirect);
                }
                db.SaveChanges();
            }
        }

        public virtual void DeleteRedirect(Guid redirectEntryId)
        {
            using (RedirectorDbContext db = new RedirectorDbContext())
            {
                var redirect = GetRedirect(redirectEntryId);
                if (redirect != null)
                {
                    db.RedirectEntries.Attach(redirect);
                    db.RedirectEntries.Remove(redirect);
                    db.SaveChanges();
                }

            }
        }

        public virtual void DeleteRedirect(string site, string oldPath)
        {
            using (RedirectorDbContext db = new RedirectorDbContext())
            {
                var redirect = GetRedirect(site, oldPath);
                if (redirect != null)
                {
                    db.RedirectEntries.Attach(redirect);
                    db.RedirectEntries.Remove(redirect);
                    db.SaveChanges();
                }
            }
        }

        public virtual void DeleteSiteRedirects(string site)
        {
            using (RedirectorDbContext db = new RedirectorDbContext())
            {
                var redirects = GetRedirects(site);

                foreach (var redirect in redirects)
                {
                    db.RedirectEntries.Remove(redirect);
                }
                db.SaveChanges();
            }
        }


        public virtual string ResolveSiteName(Item item)
        {
            var sites = SiteContextFactory.Sites.Where(
                s => !string.IsNullOrEmpty(s.RootPath) && s.VirtualFolder=="/" && s.PhysicalFolder=="/" ).Select(s => new { Path = GetPath(s), Name = s.Name }).OrderByDescending(s => s.Path);

            string itemPath = item.Paths.FullPath.ToLower();

            foreach (var site in sites)
            {
                if (itemPath.StartsWith(site.Path.ToLower()))
                    return site.Name;
            }

            //if (site != null)
            //    return site.Name;

            return "website";
        }


        private string GetPath(SiteInfo site)
        {
            string root = site.RootPath;
            string startItem = site.StartItem;
             

            if (startItem.StartsWith("/sitecore"))
            {
                return startItem;
            }
            else if (!string.IsNullOrEmpty(startItem))
            {
                return root + StringUtil.EnsurePrefix('/', startItem);
            }
            else
            {
                return root;
            }


        }


        public virtual NotFoundRule ResolveRedirectRule(string siteName, string path)
        {
            Assert.ArgumentNotNull(path, "url");
            if (path.Length > 0)
            {
                var rule = Rules.Where(r => r.Site == string.Empty || new ListString(r.Site).Contains(siteName)).FirstOrDefault(r => new Regex(r.Pattern, RegexOptions.IgnoreCase).IsMatch(path));
                if (rule != null)
                    return rule;

            }
            return null;
        }
    }
}
