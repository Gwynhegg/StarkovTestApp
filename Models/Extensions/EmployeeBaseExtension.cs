namespace StarkovTestApp.Models.Extensions
{
    public class EmployeeExtension : Employee
    {
        public Department ParentDepartment { get; set; }
    }
}
