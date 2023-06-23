using System.Runtime.InteropServices.ComTypes;
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
        public async Task<string> UploadFile(Consts.ContentTypes fileType, string filePath)
        {
            var result = String.Empty;

            switch (fileType)
            {
                case Consts.ContentTypes.Employee: result = _uploadService.UploadEmployees(filePath); break;
                case Consts.ContentTypes.Department: result = _uploadService.UploadDepartments(filePath); break;
                case Consts.ContentTypes.JobTitle: result = _uploadService.UploadJobTitles(filePath); break;
            }
            
            return await Task.FromResult(result);
        }
    }
}
