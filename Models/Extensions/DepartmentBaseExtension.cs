namespace StarkovTestApp.Models.Extensions
{
    public class DepartmentExtension : Department
    {
        public Department ParentDepartment { get; set; }
        public List<Department> ChildrenDepartment { get; set; }
    }
}
