using System.Text;
using StarkovTestApp.DbLayer;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Services
{
    /// <summary>
    /// Сервис для вывода ошибок по таблицам
    /// </summary>
    public class ErrorService : IErrorService
    {
        private readonly DbLayerContext _dbLayerContext;

        public ErrorService(DbLayerContext dbLayerContext)
        {
            _dbLayerContext = dbLayerContext;
        }

        /// получаем список ошибок из таблиц
        public string GetErrors()
        {
            var result = new StringBuilder();

            // получаем список отделов, у которых проставлено поле NotValid (проставляет ХП)
            var invalidDepartments = _dbLayerContext.Departments
                .Where(dep => dep.NotValid)
                .ToList();

            // выводим ошибки с отсутствующим менеджером
            invalidDepartments.ForEach(dep => result.AppendLine($"У департамента {dep.Name} в качестве менеджера задан несуществующий человек"));

            // получаем список работников, у которых проставлено поле NotValid
            var invalidPersons = _dbLayerContext.Employees
                .Where(emp => emp.NotValid);

            // выбираем тех работников, у которых не проставлен отдел
            var personsWithoutDepartment = invalidPersons
                .Where(emp => emp.DepartmentID == 0)
                .ToList();

            // выводим ошибки по отсутствующим отделам
            personsWithoutDepartment.ForEach(emp => result.AppendLine($"Для работника {emp.Fullname} не задан департамент"));

            // выбираем работников, у которых не проставлена должность
            var personsWithoutJobTitle = invalidPersons
                .Where(emp => emp.JobTitle == 0)
                .ToList();

            // выводим ошибки по отсутствующим должностям
            personsWithoutJobTitle.ForEach(emp => result.AppendLine($"У пользователя {emp.Fullname} задана несуществующая профессия"));

            return result.ToString();
        }
    }
}
