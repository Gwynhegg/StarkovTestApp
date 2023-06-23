using System.Text;
using System.Text.RegularExpressions;
using StarkovTestApp.Models;
using StarkovTestApp.Models.Extensions;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Services
{
    public class ResultBuilder : IResultBuilder
    {
        public string CreateResult(Enterprise enterprise)
        {
            StringBuilder result = new StringBuilder();

            foreach (var department in enterprise.Departments)
                result.AppendLine(GetDepartmentDescription(department));
            
            return Regex.Replace(result.ToString(), @"(\r\n)+", "\r\n");
        }

        private string GetDepartmentDescription(DepartmentExtension? department)
        {
            var newBuilder = new StringBuilder();

            newBuilder.AppendLine($"{new string('=', department.Level + 1)} " +
                                  $"{department.Name} (ID = {department.ID})");

            foreach (var employee in department.Employees)
            {
                var managerIndicator = department.ManagerID == employee.ID ? "*" : "-";
                newBuilder.AppendLine($"{new string(' ', department.Level + 1)}" +
                                      $" {managerIndicator} {employee.Fullname} (ID = {employee.ID}) " +
                                      $"{employee.JobDescription} (JobID = {employee.JobTitle})");
            }

            foreach (var childDep in department.ChildrenDepartment)
            {
                var childDepDescription = GetDepartmentDescription(childDep);
                newBuilder.AppendLine(childDepDescription);
            }

            return newBuilder.ToString();
        }
    }
}
