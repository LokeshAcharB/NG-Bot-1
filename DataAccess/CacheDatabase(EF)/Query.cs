using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess
{
    public class Query
    {
        public Query()
        {
            this.Resolution = new HashSet<Resolution>();
        }
        [Key]
        [Column(Order = 0)]
        public Guid QueryID { get; set; }
        public string QueryName { get; set; }
        public byte QueryStatus { get; set; }
        [Column(Order = 1)]
        public Guid ProjectID { get; set; }
        public virtual Project Project { get; set; }
        public ICollection<Resolution> Resolution { get; set; }
    }
}