using System.ComponentModel;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarkovTestApp.Models;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;
        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }
        [HttpGet]
        public string GetTable(Consts.ContentTypes fileType)
        {
            Type t;
            switch (fileType)
            {
                case Consts.ContentTypes.Department: return _tableService.ExportTable<Department>(); break;
                case Consts.ContentTypes.Employee: return _tableService.ExportTable<Employee>(); break;
                default: return _tableService.ExportTable<JobTitle>(); break;
            }
        }
    }
}
