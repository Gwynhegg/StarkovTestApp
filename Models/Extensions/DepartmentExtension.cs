namespace StarkovTestApp.Models.Extensions
{
    /// <summary>
    /// Класс-расширение для класса Department
    /// </summary>
    public class DepartmentExtension : Department
    {
        /// <summary>
        /// Копирующий конструктор
        /// </summary>
        /// <param name="dep"></param>
        public DepartmentExtension(Department dep)
        {
            this.Name = dep.Name;
            this.ManagerName = dep.ManagerName;
            this.ParentName = dep.ParentName;
            this.ID = dep.ID;
            this.ManagerID = dep.ManagerID;
            this.ParentID = dep.ParentID;
            this.Phone = dep.Phone;
            this.NotValid = dep.NotValid;
        }

        public DepartmentExtension(){}

        /// <summary>
        /// Родительский отдел
        /// </summary>
        public DepartmentExtension? ParentDepartment { get; set; }

        /// <summary>
        /// Список дочерних отделов
        /// </summary>
        public List<DepartmentExtension?> ChildrenDepartment { get; set; } = new();

        /// <summary>
        /// Список сотрудников отдела
        /// </summary>
        public List<Employee> Employees { get; set; } = new();

        /// <summary>
        /// Уровень отдела
        /// </summary>
        public int Level { get; set; } = 0;
    }
}
