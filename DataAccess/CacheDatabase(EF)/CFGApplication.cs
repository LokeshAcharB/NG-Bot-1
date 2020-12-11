using System;
using System.Collections.Generic;

namespace DataAccess
{
    public class CFGApplication
    {
        public CFGApplication()
        {
            this.Employees = new HashSet<Employee>();
        }
        public Guid Id { get; set; }
        public string TeamName { get; set; }
        public string ProjectName { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}