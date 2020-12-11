using System.Data.Entity;
namespace DataAccess
{
    public class EmployeeDbEntities : DbContext
    {
        public EmployeeDbEntities() : base("name=EmployeeDbEntities")
        {
            Database.SetInitializer<EmployeeDbEntities>(new CreateDatabaseIfNotExists<EmployeeDbEntities>());
            Database.SetInitializer<EmployeeDbEntities>(new DropCreateDatabaseIfModelChanges<EmployeeDbEntities>());
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<CFGApplication> CFGApplications { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Query> Queries { get; set; }
        public DbSet<Resolution> Resolutions { get; set; }

    }
}