using System;
using DataAccess;
using CommonLibrary;

namespace BusinessLogic
{
    public class EmployeeValidator : IEmployeeValidator
    {
        private readonly IEmployeeRepository _EmployeeRepository;
        public EmployeeValidator(IEmployeeRepository EmployeeRepository)
        {
            _EmployeeRepository = EmployeeRepository;
        }
        public bool ValidateToUpload(object obj)
        {
            var NewEmployee = (Employee)obj;
            if (NewEmployee != null)
            {
                return !string.IsNullOrEmpty(NewEmployee.FullName) && NewEmployee.PUID.IsValidPUID() && !string.IsNullOrEmpty(NewEmployee.JobTitle) ? 
                        _EmployeeRepository.AddEmployee(NewEmployee) : false;
            }
            return false;
        }
        public object ValidateToRead(string PUID)
        {
            return PUID.IsValidPUID() ? _EmployeeRepository.GetEmployeeProfile(PUID) : null ;
        }
        public bool ValidateToModify(string OldPUID, object NewEmploye)
        {
            var NewEmployee = (Employee)NewEmploye;
            bool Status = (OldPUID.IsValidPUID() && NewEmployee.PUID.IsValidPUID() &&
                           !string.IsNullOrEmpty(NewEmployee.FullName))?
                          _EmployeeRepository.ModifyEmployeeDetails(OldPUID, NewEmployee): false;
            return Status;
        }
        public bool ValidateToDelete(string PUID)
        {
            bool Status = PUID.IsValidPUID() ? _EmployeeRepository.DeleteEmployee(PUID) : false;
            return Status;
        }
    }
}
