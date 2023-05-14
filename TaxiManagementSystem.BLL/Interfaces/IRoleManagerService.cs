using Microsoft.AspNet.Identity.EntityFramework;

namespace TaxiManagementSystem.BLL.Interfaces
{
    public interface IRoleManagerService
    {
        void CreateRole(string roleName);
        IdentityRole FindRoleByName(string roleName);
    }
}
