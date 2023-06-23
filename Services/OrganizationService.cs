using StarkovTestApp.DbLayer;
using StarkovTestApp.Models;
using StarkovTestApp.Models.Extensions;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly DbLayerContext _dbLayerContext;

        public OrganizationService(DbLayerContext dbLayerContext)
        {
            _dbLayerContext = dbLayerContext;
        }

        public Enterprise GetEnterprise()
        {
            var enterprise = new Enterprise();

            var rootDepartments = _dbLayerContext.Departments
                .Where(x => x.ParentID == 0 && !x.NotValid)
                .Select(x => new DepartmentExtension(x))
                .ToList();

            enterprise.Departments.AddRange(rootDepartments);
            
            foreach (var department in enterprise.Departments)
                SetChildNodes(department);

            return enterprise;
        }

        public Enterprise GetEnterprise(int ID)
        {
            var enterprise = new Enterprise();

            var employee = _dbLayerContext.Employees.SingleOrDefault(x => x.ID == ID);

            if (employee == null) return enterprise;

            var department = _dbLayerContext.Departments
                .Where(x => x.ID == employee.DepartmentID && !x.NotValid)
                .Select(x => new DepartmentExtension(x))
                .SingleOrDefault();

            if (department == null) return enterprise;

            department.Employees.Add(employee);
            var topDepartment = GetParentNodes(department);

            enterprise.Departments.Add(topDepartment);

            return enterprise;
        }

        private DepartmentExtension? GetParentNodes(DepartmentExtension? department)
        {
            if (department.ParentID == 0)
                return department;

            department.ParentDepartment = _dbLayerContext.Departments
                .Where(x => x.ID == department.ParentID && !x.NotValid)?
                .Select(x => new DepartmentExtension(x))
                .SingleOrDefault();

            if (department.ParentDepartment == null) return department;

            department.ParentDepartment.ChildrenDepartment.Add(department);
            return GetParentNodes(department.ParentDepartment);
        }
        private void SetChildNodes(DepartmentExtension? department, int currentLevel = 0)
        {
            department.Level = currentLevel;

            SetEmployees(department);

            var childDepartments = _dbLayerContext.Departments
                .Where(x => x.ParentID == department.ID && !x.NotValid)
                .Select(x => new DepartmentExtension(x))
                .ToList();

            department.ChildrenDepartment.AddRange(childDepartments);

            foreach (var dep in department.ChildrenDepartment)
            {
                dep.ParentDepartment = department;
                SetChildNodes(dep, currentLevel + 1);
            }
        }

        private void SetEmployees(DepartmentExtension? department)
        {
            var departmentEmployees = _dbLayerContext.Employees
                .Where(x => x.DepartmentID == department.ID && !x.NotValid)
                .ToList();

            department.Employees.AddRange(departmentEmployees);
        }
    }
}
