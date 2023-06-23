using StarkovTestApp.Models;

namespace StarkovTestApp.Services.Interfaces
{
    public interface IOrganizationService
    {
        Enterprise GetEnterprise();
        Enterprise GetEnterprise(int ID);
    }
}
