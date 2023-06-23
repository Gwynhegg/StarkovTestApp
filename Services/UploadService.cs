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
        public UploadService(DbLayerContext dbLayerContext, IValidateService validateService, ILinkService linkService)
        {
            _dbLayerContext = dbLayerContext;
            _validateService = validateService;
            _linkService = linkService;
        }

        public string UploadEmployees(string file)
        {
            Logger.Instance.Clear();

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
                    employee.NotValid = false;

                    if (!_validateService.IsValid(employee))
                    {
                        Logger.Instance.Log($"Пользователь {employee.Fullname} не будет добавлен");
                        continue;
                    }

                    employees.Add(fullname, employee);
                }
            }

            _dbLayerContext.UpdateRange(employees.Values);
            _dbLayerContext.SaveChanges();

            Logger.Instance.Log($"Успешно добавлено/изменено {employees.Values.Count} пользователей");

            _linkService.UpdateEmployeesTable();

            return Logger.Instance.GetLog();
        }

        public string UploadDepartments(string file)
        {
            Logger.Instance.Clear();

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
                    department.Phone = StandartizePhoneNumber(rowArray[3]) ?? String.Empty;

                    department.NotValid = department.Phone == String.Empty;

                    if (!_validateService.IsValid(department))
                    {
                        Logger.Instance.Log($"Департамент {department.Name} не был добавлен");
                        continue;
                    }

                    departments.Add((name, parentName), department);
                }
            }

            _dbLayerContext.Departments.UpdateRange(departments.Values);
            _dbLayerContext.SaveChanges();

            Logger.Instance.Log($"Успешно добавлено/изменено {departments.Values.Count} отделов");

            _linkService.UpdateDepartmentTable();

            return Logger.Instance.GetLog();
        }

        public string UploadJobTitles(string file)
        {
            Logger.Instance.Clear();

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
                        Logger.Instance.Log($"Должность {jobTitle.Name} не была добавлена");
                        continue;
                    }

                    jobTitles.Add(jobTitle.Name, jobTitle);
                }
            }

            _dbLayerContext.JobTitles.UpdateRange(jobTitles.Values);
            _dbLayerContext.SaveChanges();

            Logger.Instance.Log($"Добавлено {jobTitles.Values.Count} должностей");

            _linkService.UpdateJobTitlesTable();

            return Logger.Instance.GetLog();
        }

        private string StandartizeString(string str)
        {
             return Regex.Replace(new CultureInfo("ru-RU").TextInfo.ToTitleCase(str.Trim()), @"\s+", " ");
        }

        private string StandartizePhoneNumber(string str)
        {
            str = Regex.Replace(str, @"[^\d]", "");
            str = Regex.Replace(str, "^8", "7");

            if (str.Length > 11) return null;

            str = Regex.Replace(str, @"(\d{1})(\d{3})(\d{3})(\d{2})(\d{2})", "+$1 ($2) $3-$4-$5");
            return str;
        }
    }
}
