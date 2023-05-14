using Microsoft.AspNet.Identity.EntityFramework;

namespace TaxiManagementSystem.DAL.Interfaces
{
    public interface IRoleManager
    {
        void CreateRole(string roleName);
        IdentityRole GetRoleByName(string roleName);
    }
}