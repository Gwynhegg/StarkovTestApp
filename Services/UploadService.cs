using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using CsvFileReader;
using CsvFileReader.Models;
using StarkovTestApp.DbLayer;
using StarkovTestApp.Models;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Services
{
    public class UploadService : IUploadService
    {
        private readonly DbLayerContext _dbLayerContext;
        private readonly IValidateService _validateService;
        private readonly ILinkService _linkService;
        private List<String> _messageCollector;
        public UploadService(DbLayerContext dbLayerContext, IValidateService validateService, ILinkService linkService)
        {
            _dbLayerContext = dbLayerContext;
            _validateService = validateService;
            _linkService = linkService;
            _messageCollector = new List<String>();
        }

        public void UploadEmployees(string file)
        {
            var employees = new Dictionary<string, Employee>();
            var csvReader = new CsvReader(file, new CsvFileParser('\t'), new DataTypeService());

            for (int i = 1; i < csvReader.DataRows.Count; i++)
            {
                var rowArray = csvReader.DataRows[i].Split('\t');

                var fullname = StandartizeString(rowArray[1]);
                if (!employees.ContainsKey(fullname))
                {

                    var employee = _dbLayerContext.Employees
                        .SingleOrDefault(pers => pers.Fullname.Equals(fullname)) ?? new Employee();

                    employee.DepartmentName = StandartizeString(rowArray[0]);
                    employee.Fullname = fullname;
                    employee.Login = rowArray[2];
                    employee.Password = rowArray[3];
                    employee.JobDescription = StandartizeString(rowArray[4]);

                    if (!_validateService.IsValid(employee))
                    {
                        _messageCollector.Add($"Пользователь {employee.Fullname} не будет добавлен");
                        continue;
                    }

                    employee.DepartmentID = _dbLayerContext.Departments
                        .Where(dep => dep.Name.Equals(employee.DepartmentName))?
                        .Select(dep => dep.ID)
                        .SingleOrDefault() ?? 0;

                    employee.JobTitle = _dbLayerContext.JobTitles
                        .Where(jt => jt.Name.Equals(employee.JobDescription))?
                        .Select(jt => jt.ID)
                        .SingleOrDefault() ?? 0;

                    employees.Add(fullname, employee);
                }
            }

            _dbLayerContext.UpdateRange(employees.Values);
            _dbLayerContext.SaveChanges();

            _messageCollector.Add($"Успешно добавлено/изменено {employees.Values.Count} пользователей");

            _linkService.UpdateManagerLink();
            _linkService.UpdateDepartmentLink();
            _linkService.UpdateJobTitleLink();
        }

        public void UploadDepartments(string file)
        {
            var departments = new Dictionary<(string, string), Department>();
            var csvReader = new CsvReader(file, new CsvFileParser('\t'), new DataTypeService());

            for (int i = 1; i < csvReader.DataRows.Count; i++)
            {
                var rowArray = csvReader.DataRows[i].Split('\t');

                var name = StandartizeString(rowArray[0]);
                var parentName = StandartizeString(rowArray[1]);

                if (!departments.ContainsKey((name, parentName)))
                {
                    var department = _dbLayerContext.Departments
                        .SingleOrDefault(dp => dp.Name.Equals(name)
                                               && dp.ParentName.Equals(parentName)) ?? new Department();

                    department.Name = name;
                    department.ParentName = parentName;
                    department.ManagerName = StandartizeString(rowArray[2]);
                    department.Phone = StandartizeString(rowArray[3]);

                    if (!_validateService.IsValid(department))
                    {
                        _messageCollector.Add($"Департамент {department.Name} не был добавлен");
                        continue;
                    }

                    department.ManagerID = _dbLayerContext.Employees
                        .Where(pers => pers.Fullname.Equals(department.ManagerName))?
                        .Select(pers => pers.ID)
                        .SingleOrDefault() ?? 0;

                    departments.Add((name, parentName), department);
                }
            }

            _dbLayerContext.Departments.UpdateRange(departments.Values);
            _dbLayerContext.SaveChanges();

            _linkService.UpdateDepartmentLink();
            _linkService.UpdateManagerLink();
            _linkService.UpdateInnerDepartmentLink();
        }

        public void UploadJobTitles(string file)
        {
            var jobTitles = new Dictionary<string, JobTitle>();
            var csvReader = new CsvReader(file, new CsvFileParser('\t'), new DataTypeService());

            for (int i = 1; i < csvReader.DataRows.Count; i++)
            {
                var rowArray = csvReader.DataRows[i].Split('\t');

                var name = StandartizeString(rowArray[0]);

                if (!jobTitles.ContainsKey(name))
                {
                    if (_dbLayerContext.JobTitles.Any(jt => jt.Name.Equals(name)))
                        continue;

                    var jobTitle = new JobTitle()
                    {
                        Name = name
                    };

                    if (!_validateService.IsValid(jobTitle))
                    {
                        _messageCollector.Add($"Должность {jobTitle.Name} не была добавлена");
                        continue;
                    }

                    jobTitles.Add(jobTitle.Name, jobTitle);
                }
            }

            _dbLayerContext.JobTitles.UpdateRange(jobTitles.Values);
            _dbLayerContext.SaveChanges();

            _linkService.UpdateJobTitleLink();
        }

        private string StandartizeString(string str)
        {
             return Regex.Replace(new CultureInfo("ru-RU").TextInfo.ToTitleCase(str.Trim()), @"\s+", " ");
        }
    }
}
