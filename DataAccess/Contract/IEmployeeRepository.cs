namespace DataAccess
{
    public interface IEmployeeRepository
    {
        bool AddEmployee(Employee employee);
        Employee GetEmployeeProfile(string PUID);
        bool ModifyEmployeeDetails(string OldPUID, Employee NewEmployee);
        bool DeleteEmployee(string PUID);
    }
}
