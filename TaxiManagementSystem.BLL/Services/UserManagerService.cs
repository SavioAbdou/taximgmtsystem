using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TaxiManagementSystem.BLL.Interfaces;
using TaxiManagementSystem.DAL.Interfaces;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.BLL.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly IUserManager _userManager;

        public UserManagerService(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public string RegisterUser(string userName, string email, Guid userRole, out string userId)
        {
            return _userManager.RegisterUser(userName, email, userRole, out userId);
        }

        public string RegisterDriver(string userName, string email, Guid userRole, out string userId)
        {
            return _userManager.RegisterDriver(userName, email, userRole, out userId);
        }

        public string RegisterAdmin(string userName, string email, Guid userRole, out string userId)
        {
            return _userManager.RegisterAdmin(userName, email, userRole, out userId);
        }

        public ApplicationBaseIdentity FindByUsername(string username)
        {
            return _userManager.FindByUsername(username);
        }

        public ApplicationBaseIdentity FindById(string id)
        {
            return _userManager.FindById(id);
        }

        public async Task<ApplicationBaseIdentity> FindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public Task<ApplicationBaseIdentity> FindAsync(string username, string password)
        {
            return _userManager.FindAsync(username, password);
        }

        public IEnumerable<ApplicationDriver> FindAllDrivers()
        {
            return _userManager.GetAllDrivers();
        }

        public IEnumerable<ApplicationUser> FindAllUsers()
        {
            return _userManager.GetAllUsers();
        }

        public IEnumerable<ApplicationAdmin> FindAllAdmins()
        {
            return _userManager.GetAllAdmins();
        }

        public async Task<IdentityResult> CreateAsync(ApplicationBaseIdentity user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<ClaimsIdentity> CreateIdentityAsync(ApplicationBaseIdentity user, string authenticationType)
        {
            return await _userManager.CreateIdentityAsync(user, authenticationType);
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(userId, currentPassword, newPassword);
        }

        public async Task<IdentityResult> AddPasswordAsync(string userId, string password)
        {
            return await _userManager.AddPasswordAsync(userId, password);
        }

        public async Task<string> GetPhoneNumberAsync(string userId)
        {
            return await _userManager.GetPhoneNumberAsync(userId);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(string userId)
        {
            return await _userManager.GetLoginsAsync(userId);
        }

        public async Task<string> GetEmailAsync(string userId)
        {
            return await _userManager.GetEmailAsync(userId);
        }

        public async Task<IdentityResult> AssignToRole(string userId, string role)
        {
            return await _userManager.AssignToRole(userId, role);
        }

        public void SetBalance(string userId, decimal balance)
        {
            _userManager.SetBalance(userId, balance);
        }
    }
}
