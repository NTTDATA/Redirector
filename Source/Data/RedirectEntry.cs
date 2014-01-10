using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NTTData.Redirector.Data
{
    public class RedirectEntry
    {
        [Key]
        public Guid RedirectEntryId { get; set; }
        public string Site { get; set; }
        public string OldPath { get; set; }
        public Guid ItemID { get; set; }
        public string QueryString { get; set; }
    }
}
