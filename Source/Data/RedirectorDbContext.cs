using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace NTTData.Redirector.Data
{
    public class RedirectorDbContext : DbContext
    {
        public RedirectorDbContext() : base("redirector") { }


        public DbSet<RedirectEntry> RedirectEntries { get; set; }
    }
}
