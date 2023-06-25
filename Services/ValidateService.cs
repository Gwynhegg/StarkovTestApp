using StarkovTestApp.Models;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Services
{
    /// <summary>
    /// Сервис для проверки корректности заполнения параметров сущностей
    /// </summary>
    public class ValidateService : IValidateService
    {
        /// <summary>
        /// Проверка полей сущности Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Проверка полей сущности Department
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        public bool IsValid(Department department)
        {
            if (!department.Phone.Equals(String.Empty) &&
                !department.Name.Equals(String.Empty))
                return true;

            return false;
        }

        /// <summary>
        /// Проверка полей сущности JobTitle
        /// </summary>
        /// <param name="jobTitle"></param>
        /// <returns></returns>
        public bool IsValid(JobTitle jobTitle)
        {
            if (!jobTitle.Name.Equals(String.Empty))
                return true;

            return false;
        }
    }
}
