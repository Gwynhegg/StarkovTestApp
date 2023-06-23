using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ErrorsController : ControllerBase
    {
        private readonly IErrorService _errorService;

        public ErrorsController(IErrorService errorService)
        {
            _errorService = errorService;
        }

        [HttpGet]
        public string GetErrors()
        {
            return _errorService.GetErrors();
        }
    }
}
