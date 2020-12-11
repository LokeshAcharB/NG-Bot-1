using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess
{
    public class Resolution
    {
        [Column(Order = 0)]
        public Guid ID { get; set; }
        public string Solution { get; set; }
        public byte ResolutionStatus { get; set; }
        [Column(Order = 1)]
        public Guid QueryID { get; set; }
        public virtual Query Query { get; set; }
    }
}