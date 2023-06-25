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
    /// <summary>
    /// Сервис для загрузки данных о таблицах из файла
    /// </summary>
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

        /// <summary>
        /// Метод для загрузки данных о работниках
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string UploadEmployees(string file)
        {
            Logger.Instance.Clear();

            var employees = new Dictionary<string, Employee>();
            var csvReader = new CsvReader(file, new CsvFileParser('\t'), new DataTypeService());

            for (int i = 1; i < csvReader.DataRows.Count; i++)
            {
                //считываем данные в новую сущность
                var rowArray = csvReader.DataRows[i].Split('\t');

                var fullname = StandartizeString(rowArray[1]);

                //для избежания повторов было принято решение использовать Dictionary для хранения сущностей
                if (!employees.ContainsKey(fullname))
                {

                    // пытаемся найти работника среди существующих. Это нужно для обновления параметров работника
                    var employee = _dbLayerContext.Employees
                        .SingleOrDefault(pers => pers.Fullname.Equals(fullname)) ?? new Employee();

                    employee.DepartmentName = StandartizeString(rowArray[0]);
                    employee.Fullname = fullname;
                    employee.Login = rowArray[2];
                    employee.Password = rowArray[3];
                    employee.JobDescription = StandartizeString(rowArray[4]);
                    employee.NotValid = false;

                    // проверяем корректность заполнения данных
                    if (!_validateService.IsValid(employee))
                    {
                        Logger.Instance.Log($"Пользователь {employee.Fullname} не будет добавлен");
                        continue;
                    }

                    employees.Add(fullname, employee);
                }
            }

            // апдейтим таблицы. Если такой записи нет в таблице - она добавится, если есть - обновятся поля на "свежие"
            _dbLayerContext.UpdateRange(employees.Values);
            _dbLayerContext.SaveChanges();

            Logger.Instance.Log($"Успешно добавлено/изменено {employees.Values.Count} пользователей");

            // вызываем метод для проставления связей и зависимостей между таблицами
            _linkService.UpdateTables(Consts.ContentTypes.Employee);

            return Logger.Instance.GetLog();
        }

        public string UploadDepartments(string file)
        {
            Logger.Instance.Clear();

            var departments = new Dictionary<(string, string), Department>();
            var csvReader = new CsvReader(file, new CsvFileParser('\t'), new DataTypeService());

            //считываем данные в новую сущность
            for (int i = 1; i < csvReader.DataRows.Count; i++)
            {
                var rowArray = csvReader.DataRows[i].Split('\t');

                var name = StandartizeString(rowArray[0]);
                var parentName = StandartizeString(rowArray[1]);

                //для избежания повторов было принято решение использовать Dictionary для хранения сущностей
                if (!departments.ContainsKey((name, parentName)))
                {
                    // пытаемся найти отдел среди существующих. Это нужно для обновления параметров отдела
                    var department = _dbLayerContext.Departments
                        .SingleOrDefault(dp => dp.Name.Equals(name)
                                               && dp.ParentName.Equals(parentName)) ?? new Department();

                    department.Name = name;
                    department.ParentName = parentName;
                    department.ManagerName = StandartizeString(rowArray[2]);
                    department.Phone = StandartizePhoneNumber(rowArray[3]) ?? String.Empty;

                    department.NotValid = department.Phone == String.Empty;

                    // проверяем корректность заполнения данных
                    if (!_validateService.IsValid(department))
                    {
                        Logger.Instance.Log($"Департамент {department.Name} не был добавлен");
                        continue;
                    }

                    departments.Add((name, parentName), department);
                }
            }

            // апдейтим таблицы. Если такой записи нет в таблице - она добавится, если есть - обновятся поля на "свежие"
            _dbLayerContext.Departments.UpdateRange(departments.Values);
            _dbLayerContext.SaveChanges();

            Logger.Instance.Log($"Успешно добавлено/изменено {departments.Values.Count} отделов");
            
            // вызываем метод для проставления связей и зависимостей между таблицами
            _linkService.UpdateTables(Consts.ContentTypes.Department);

            return Logger.Instance.GetLog();
        }

        public string UploadJobTitles(string file)
        {
            Logger.Instance.Clear();

            var jobTitles = new Dictionary<string, JobTitle>();
            var csvReader = new CsvReader(file, new CsvFileParser('\t'), new DataTypeService());

            //считываем данные в новую сущность
            for (int i = 1; i < csvReader.DataRows.Count; i++)
            {
                var rowArray = csvReader.DataRows[i].Split('\t');

                var name = StandartizeString(rowArray[0]);

                //для избежания повторов было принято решение использовать Dictionary для хранения сущностей
                if (!jobTitles.ContainsKey(name))
                {
                    // если в таблице уже есть информация о данной профессии - пропускаем
                    if (_dbLayerContext.JobTitles.Any(jt => jt.Name.Equals(name)))
                        continue;

                    var jobTitle = new JobTitle()
                    {
                        Name = name
                    };

                    // проверяем корректность заполнения данных
                    if (!_validateService.IsValid(jobTitle))
                    {
                        Logger.Instance.Log($"Должность {jobTitle.Name} не была добавлена");
                        continue;
                    }

                    jobTitles.Add(jobTitle.Name, jobTitle);
                }
            }

            // апдейтим таблицы. Если такой записи нет в таблице - она добавится, если есть - обновятся поля на "свежие"
            _dbLayerContext.JobTitles.UpdateRange(jobTitles.Values);
            _dbLayerContext.SaveChanges();

            Logger.Instance.Log($"Добавлено {jobTitles.Values.Count} должностей");

            // вызываем метод для проставления связей и зависимостей между таблицами
            _linkService.UpdateTables(Consts.ContentTypes.JobTitle);

            return Logger.Instance.GetLog();
        }

        /// <summary>
        /// Метод для стандартизации входной строки
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string StandartizeString(string str)
        {
            // удаляем первые и последние пробелы, приводим вид к TitleCase? удаляем повторяющиеся пробелы
             return Regex.Replace(new CultureInfo("ru-RU").TextInfo.ToTitleCase(str.Trim()), @"\s+", " ");
        }

        /// <summary>
        /// Метод для стандартизации телефонного номера
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string StandartizePhoneNumber(string str)
        {
            // заменяем все символы - не цифры на пустой символ
            str = Regex.Replace(str, @"[^\d]", "");
            // заменяем первувю 8-ку на 7
            str = Regex.Replace(str, "^8", "7");

            // проверяем длину телефонного номера
            if (str.Length != 11) return null;

            // форматируем строку, разбивая цифры на группы и подставляя дополнительные символы
            str = Regex.Replace(str, @"(\d{1})(\d{3})(\d{3})(\d{2})(\d{2})", "+$1 ($2) $3-$4-$5");
            return str;
        }
    }
}
