using StarkovTestApp.Models.Extensions;

namespace StarkovTestApp.Models
{
    /// <summary>
    /// Модель предприятия, содержащая список дочерних предприятий
    /// </summary>
    public class Enterprise
    {
        public List<DepartmentExtension?> Departments = new();
    }
}
