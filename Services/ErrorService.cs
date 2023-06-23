using System.Text;
using StarkovTestApp.DbLayer;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Services
{
    public class ErrorService : IErrorService
    {
        private readonly DbLayerContext _dbLayerContext;

        public ErrorService(DbLayerContext dbLayerContext)
        {
            _dbLayerContext = dbLayerContext;
        }
        public string GetErrors()
        {
            var result = new StringBuilder();

            var invalidDepartments = _dbLayerContext.Departments
                .Where(dep => dep.NotValid)
                .ToList();

            invalidDepartments.ForEach(dep => result.AppendLine($"У департамента {dep.Name} в качестве менеджера задан несуществующий человек"));

            var invalidPersons = _dbLayerContext.Employees
                .Where(emp => emp.NotValid);

            var personsWithoutDepartment = invalidPersons
                .Where(emp => emp.DepartmentID == 0)
                .ToList();

            personsWithoutDepartment.ForEach(emp => result.AppendLine($"Для работника {emp.Fullname} не задан департамент"));

            var personsWithoutJobTitle = invalidPersons
                .Where(emp => emp.JobTitle == 0)
                .ToList();

            personsWithoutJobTitle.ForEach(emp => result.AppendLine($"У пользователя {emp.Fullname} задана несуществующая профессия"));

            return result.ToString();
        }
    }
}
