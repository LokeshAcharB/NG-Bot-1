using System.Collections.Generic;
using System;

namespace DataAccess
{
    public class Team
    {
        public Team()
        {
            this.Project = new HashSet<Project>();
        }
        public Guid TeamID { get; set; }
        public string TeamName { get; set; }
        public byte TeamStatus { get; set; } 
        public ICollection<Project> Project { get; set; }
    }
}