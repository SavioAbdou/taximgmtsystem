using Microsoft.AspNet.Identity.EntityFramework;
using TaxiManagementSystem.BLL.Interfaces;
using TaxiManagementSystem.DAL.Interfaces;

namespace TaxiManagementSystem.BLL.Services
{
    public class RoleManagerService : IRoleManagerService
    {
        private readonly IRoleManager _roleManager;

        public RoleManagerService(IRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public void CreateRole(string roleName)
        {
            _roleManager.CreateRole(roleName);
        }

        public IdentityRole FindRoleByName(string roleName)
        {
            return _roleManager.GetRoleByName(roleName);
        }
    }
}
