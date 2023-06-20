using StarkovTestApp.Models;

namespace StarkovTestApp.Services.Interfaces
{
    public interface IValidateService
    {
        bool IsValid(Employee employee);
        bool IsValid(Department department);
        bool IsValid(JobTitle jobTitle);
    }
}
