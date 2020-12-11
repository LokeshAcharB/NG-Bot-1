using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess
{
    public class Project
    {
        public Project()
        {
            this.Query = new HashSet<Query>();
        }
        [Column(Order = 0)]
        public Guid ProjectID { get; set; }
        public string ProjectName { get; set; }
        public byte ProjectStatus { get; set; }
        [Column(Order = 1)]
        public Guid TeamID { get; set; }
        public virtual Team Team { get; set; }
        public ICollection<Query> Query { get; set; }
    }
}