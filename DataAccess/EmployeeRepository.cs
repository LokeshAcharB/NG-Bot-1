using NLog;
using System;
using System.Linq;

namespace DataAccess
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly Logger logger;
        private Employee employee = null;
        private readonly EmployeeDbEntities EmployeeDbcontext;
        public EmployeeRepository()
        {
            logger = LogManager.GetCurrentClassLogger();
            EmployeeDbcontext = new EmployeeDbEntities();
        }
        public bool AddEmployee(Employee employee)
        {
            try
            {
                EmployeeDbcontext.Employees.Add(employee);
                EmployeeDbcontext.SaveChanges();
                return true;
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                logger.Error($"Something bad happened in employee site {ex}!");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }
        }
        public Employee GetEmployeeProfile(string PUID)
        {
            try
            {
                employee = (from emp in EmployeeDbcontext.Employees.Include("CFGApplications")
                            where emp.PUID == PUID
                            select emp).AsEnumerable().FirstOrDefault();
                return employee;
            }
            catch (ArgumentNullException ex)
            {
                logger.Error($"Something bad happened in employee site {ex}!");
                return employee;
            }
        }
        public bool ModifyEmployeeDetails(string OldPUID, Employee NewEmployee)
        {
            try
            {
                employee = (from emp in EmployeeDbcontext.Employees
                            where emp.PUID == OldPUID
                            select emp).AsEnumerable().FirstOrDefault();
                employee.FullName = NewEmployee.FullName;
                employee.PUID = NewEmployee.PUID;
                employee.EmailID = "";
                EmployeeDbcontext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error($"Something bad happened in employee site {ex}!");
                return false;
            }
        }
        public bool DeleteEmployee(string PUID)
        {
            try
            {
                employee = (from emp in EmployeeDbcontext.Employees
                                where emp.PUID == PUID
                                select emp).AsEnumerable().FirstOrDefault();
                EmployeeDbcontext.Employees.Remove(employee);
                EmployeeDbcontext.SaveChanges();
                return true;
            }
            catch (ArgumentNullException ex)
            {
                logger.Error($"Something bad happened in employee site {ex}!");
                return false;
            }
        }
    }
}
