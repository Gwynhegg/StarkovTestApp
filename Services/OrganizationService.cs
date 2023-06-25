using StarkovTestApp.DbLayer;
using StarkovTestApp.Models;
using StarkovTestApp.Models.Extensions;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Services
{
    /// <summary>
    /// Сервис дял воссоздания структуры организации
    /// </summary>
    public class OrganizationService : IOrganizationService
    {
        private readonly DbLayerContext _dbLayerContext;

        public OrganizationService(DbLayerContext dbLayerContext)
        {
            _dbLayerContext = dbLayerContext;
        }

        /// <summary>
        /// Метод для конструктирования полной структуры
        /// </summary>
        /// <returns></returns>
        public Enterprise GetEnterprise()
        {
            var enterprise = new Enterprise();

            // поулчаем корневые отделы c ParentId = 0 и расширяем их с помощью класса DepartmentExtension
            var rootDepartments = _dbLayerContext.Departments
                .Where(x => x.ParentID == 0 && !x.NotValid)
                .Select(x => new DepartmentExtension(x))
                .ToList();

            enterprise.Departments.AddRange(rootDepartments);
            
            // дял каждого из отделов устанавливаем дочерние
            foreach (var department in enterprise.Departments)
                SetChildNodes(department);

            return enterprise;
        }

        /// <summary>
        /// Методя дял восстановления структуры предприятия, исходя из конкретного работника
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Enterprise GetEnterprise(int ID)
        {
            var enterprise = new Enterprise();

            // получаем работника из таблицы
            var employee = _dbLayerContext.Employees.SingleOrDefault(x => x.ID == ID);

            if (employee == null) throw new Exception("Пользователь с таким ID не найден");

            var department = _dbLayerContext.Departments
                .Where(x => x.ID == employee.DepartmentID && !x.NotValid)
                .Select(x => new DepartmentExtension(x))
                .SingleOrDefault();

            if (department == null) throw new Exception("Для пользователя не найден родительский отдел");

            // добавляем в отдел нашего работника
            department.Employees.Add(employee);
            // рекурсивно получаем родительские узлы
            var topDepartment = GetParentNodes(department);

            enterprise.Departments.Add(topDepartment);

            return enterprise;
        }

        /// <summary>
        /// Метод для получения родительских нодов
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        private DepartmentExtension? GetParentNodes(DepartmentExtension? department)
        {
            // условие выхода из рекурсии - наткнулись на самый верхний отдел
            if (department.ParentID == 0)
                return department;

            // получаем родительский отдел и формируем расширения департамента
            department.ParentDepartment = _dbLayerContext.Departments
                .Where(x => x.ID == department.ParentID && !x.NotValid)?
                .Select(x => new DepartmentExtension(x))
                .SingleOrDefault();

            if (department.ParentDepartment == null) return department;

            // добавляем полученных отдел в родительские
            department.ParentDepartment.ChildrenDepartment.Add(department);
            // вызываем тот же метод для родителского элемента
            return GetParentNodes(department.ParentDepartment);
        }

        /// <summary>
        /// Метод для получения дочерних нодов для отделов
        /// </summary>
        /// <param name="department"></param>
        /// <param name="currentLevel"></param>
        private void SetChildNodes(DepartmentExtension? department, int currentLevel = 0)
        {
            // устанавливаем текущий уровень отдела
            department.Level = currentLevel;

            // устанавливаем сотрудников для отдела
            SetEmployees(department);

            // получаем список дочерних отделов
            var childDepartments = _dbLayerContext.Departments
                .Where(x => x.ParentID == department.ID && !x.NotValid)
                .Select(x => new DepartmentExtension(x))
                .ToList();

            department.ChildrenDepartment.AddRange(childDepartments);

            // для каждого дочернего отдела устанавливаем родительский и рекурсивно вызываем заполнение
            foreach (var dep in department.ChildrenDepartment)
            {
                dep.ParentDepartment = department;
                SetChildNodes(dep, currentLevel + 1);
            }
        }

        /// <summary>
        /// Метод для установки сотрудников в департамент
        /// </summary>
        /// <param name="department"></param>
        private void SetEmployees(DepartmentExtension? department)
        {
            var departmentEmployees = _dbLayerContext.Employees
                .Where(x => x.DepartmentID == department.ID && !x.NotValid)
                .ToList();

            department.Employees.AddRange(departmentEmployees);
        }
    }
}
