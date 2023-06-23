using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarkovTestApp.Models;
using StarkovTestApp.Services.Interfaces;

namespace StarkovTestApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly IResultBuilder _resultBuilder;
        public OrganizationController(IOrganizationService organizationService, IResultBuilder resultBuilder)
        {
            _organizationService = organizationService;
            _resultBuilder = resultBuilder;
        }

        [HttpGet]
        public Task<string> GetOrganization()
        {
            var enterprise = _organizationService.GetEnterprise();
            return Task.FromResult(_resultBuilder.CreateResult(enterprise));
        }

        [HttpPost]
        public Task<string> GetOrganization(int employeeID)
        {
            var enterprise = _organizationService.GetEnterprise(employeeID);
            return Task.FromResult(_resultBuilder.CreateResult(enterprise));
        }
    }
}
