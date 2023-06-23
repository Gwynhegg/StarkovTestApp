namespace StarkovTestApp.Models.Extensions
{
    public class DepartmentExtension : Department
    {
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

        public DepartmentExtension? ParentDepartment { get; set; }
        public List<DepartmentExtension?> ChildrenDepartment { get; set; } = new();

        public List<Employee> Employees { get; set; } = new();
        public int Level { get; set; } = 0;
    }
}
