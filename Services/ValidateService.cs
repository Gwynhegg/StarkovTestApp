using StarkovTestApp.Models;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Services
{
    public class ValidateService : IValidateService
    {
        public bool IsValid(Employee employee)
        {
            if (!employee.Fullname.Equals(String.Empty) &&
                !employee.Login.Equals(String.Empty) &&
                !employee.Password.Equals(String.Empty) &&
                !employee.DepartmentName.Equals(String.Empty) &&
                !employee.JobDescription.Equals(String.Empty))
                return true;

            return false;
        }

        public bool IsValid(Department department)
        {
            if (!department.Phone.Equals(String.Empty) &&
                !department.Name.Equals(String.Empty))
                return true;

            return false;
        }

        public bool IsValid(JobTitle jobTitle)
        {
            if (!jobTitle.Name.Equals(String.Empty))
                return true;

            return false;
        }
    }
}
