using CsvFileReader;
using CsvFileReader.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarkovTestApp.Models;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImportController : ControllerBase
    {
        private readonly IUploadService _uploadService;

        public ImportController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(Consts.ContentTypes fileType, string filePath)
        {
            switch (fileType)
            {
                case Consts.ContentTypes.Employee: _uploadService.UploadEmployees(filePath); break;
                case Consts.ContentTypes.Department: _uploadService.UploadDepartments(filePath); break;
                case Consts.ContentTypes.JobTitle: _uploadService.UploadJobTitles(filePath); break;
            }
            
            return await Task.FromResult(Ok());
        }
    }
}
