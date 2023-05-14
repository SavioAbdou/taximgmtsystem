using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TaxiManagementSystem.DAL.Interfaces;

namespace TaxiManagementSystem.DAL.Repository
{
    public class RoleManager : IRoleManager
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManager()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        }

        public void CreateRole(string roleName)
        {
            _roleManager.Create(new IdentityRole(roleName));
        }

        public IdentityRole GetRoleByName(string roleName)
        {
            return _roleManager.FindByName(roleName);
        }
    }
}
