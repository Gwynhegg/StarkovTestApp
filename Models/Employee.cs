namespace StarkovTestApp.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string Fullname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public int JobTitle { get; set; }
        public string JobDescription { get; set; }
    }
}
