using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess
{
    public class Employee
    {
        public Employee()
        {
            this.CFGApplications = new HashSet<CFGApplication>();
        }
        public Guid ID { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string PUID { get; set; }
        private string _EmailID;
        [Required]
        public string EmailID
        {
            get => _EmailID;
            set => _EmailID = $"{PUID}@eurofins.com";
        }
        [Required]
        public string JobTitle { get; set; }
        [Required]
        public ICollection<CFGApplication> CFGApplications { get; set; }
    }
}
