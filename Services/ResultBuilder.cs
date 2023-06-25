using System.Text;
using System.Text.RegularExpressions;
using StarkovTestApp.Models;
using StarkovTestApp.Models.Extensions;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Services
{
    /// <summary>
    /// Сервис дял построения структуры предприятия
    /// </summary>
    public class ResultBuilder : IResultBuilder
    {
        /// <summary>
        /// Метод для создания результата в соответствии со структурой класса Enterprise
        /// </summary>
        /// <param name="enterprise"></param>
        /// <returns></returns>
        public string CreateResult(Enterprise enterprise)
        {
            StringBuilder result = new StringBuilder();

            // на данный момент в Enterprise отображены корневые отделы. Дочерние отделы находятся ниже,
            // в расширении DepartmentExtension в поле ChildrenDepartment
            foreach (var department in enterprise.Departments)
                result.AppendLine(GetDepartmentDescription(department));
            
            return Regex.Replace(result.ToString(), @"(\r\n)+", "\r\n");
        }

        /// <summary>
        /// Метод формирования описания департамента
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        private string GetDepartmentDescription(DepartmentExtension? department)
        {
            var newBuilder = new StringBuilder();

            // формируем описание отдела, используя нужное количество отступов и поля сущности
            newBuilder.AppendLine($"{new string('=', department.Level + 1)} " +
                                  $"{department.Name} (ID = {department.ID})");

            // аналогично формируем список сотрудников отдела
            foreach (var employee in department.Employees)
            {
                var managerIndicator = department.ManagerID == employee.ID ? "*" : "-";
                newBuilder.AppendLine($"{new string(' ', department.Level + 1)}" +
                                      $" {managerIndicator} {employee.Fullname} (ID = {employee.ID}) " +
                                      $"{employee.JobDescription} (JobID = {employee.JobTitle})");
            }

            // для каждого дочернего отдела рекурсивно вызываем этот же метод
            foreach (var childDep in department.ChildrenDepartment)
            {
                var childDepDescription = GetDepartmentDescription(childDep);
                newBuilder.AppendLine(childDepDescription);
            }

            return newBuilder.ToString();
        }
    }
}
