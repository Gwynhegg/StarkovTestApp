using StarkovTestApp.DbLayer;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Services
{
    public class LinkService : ILinkService
    {
        private readonly DbLayerContext _dbLayerContext;

        public LinkService(DbLayerContext dbLayerContext)
        {
            _dbLayerContext = dbLayerContext;
        }
        public void UpdateManagerLink()
        {
            var departmentList = _dbLayerContext.Departments.ToList();

            foreach (var department in departmentList)
                department.ManagerID =
                    _dbLayerContext.Employees
                        .Where(pers => pers.Fullname.Equals(department.ManagerName))?
                        .Select(pers => pers.ID)
                        .SingleOrDefault() ?? 0;

            _dbLayerContext.UpdateRange(departmentList);
            _dbLayerContext.SaveChanges();
        }

        public void UpdateDepartmentLink()
        {
            var employeeList = _dbLayerContext.Employees.ToList();

            foreach (var employee in employeeList)
                employee.DepartmentID = _dbLayerContext.Departments
                    .Where(dep => dep.Name.Equals(employee.DepartmentName))?
                    .Select(dep => dep.ID)
                    .SingleOrDefault() ?? 0;

            _dbLayerContext.UpdateRange(employeeList);
            _dbLayerContext.SaveChanges();
        }

        public void UpdateJobTitleLink()
        {
            var employeeList = _dbLayerContext.Employees.ToList();

            foreach (var employee in employeeList)
                employee.JobTitle = _dbLayerContext.JobTitles
                    .Where(jt => jt.Name.Equals(employee.JobDescription))?
                    .Select(jt => jt.ID)
                    .SingleOrDefault() ?? 0;

            _dbLayerContext.UpdateRange(employeeList);
            _dbLayerContext.SaveChanges();
        }

        public void UpdateInnerDepartmentLink()
        {
            var departmentList = _dbLayerContext.Departments.ToList();

            foreach (var department in departmentList)
                department.ParentID = departmentList
                    .Where(dep => dep.Name.Equals(department.ParentName))?
                    .Select(dep => dep.ID)
                    .SingleOrDefault() ?? 0;

            _dbLayerContext.UpdateRange(departmentList);
            _dbLayerContext.SaveChanges();
        }
    }
}
