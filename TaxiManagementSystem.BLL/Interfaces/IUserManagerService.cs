using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.BLL.Interfaces
{
    public interface IUserManagerService
    {
        string RegisterUser(string userName, string email, Guid userRole, out string userId);
        string RegisterDriver(string userName, string email, Guid userRole, out string userId);
        string RegisterAdmin(string userName, string email, Guid userRole, out string userId);
        ApplicationBaseIdentity FindByUsername(string username);
        ApplicationBaseIdentity FindById(string id);
        Task<ApplicationBaseIdentity> FindByIdAsync(string id);
        Task<ApplicationBaseIdentity> FindAsync(string username, string password);
        IEnumerable<ApplicationDriver> FindAllDrivers();
        IEnumerable<ApplicationUser> FindAllUsers();
        IEnumerable<ApplicationAdmin> FindAllAdmins();
        Task<IdentityResult> CreateAsync(ApplicationBaseIdentity user, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(ApplicationBaseIdentity user, string authenticationType);
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<IdentityResult> AddPasswordAsync(string userId, string password);
        Task<string> GetPhoneNumberAsync(string userId);
        Task<IList<UserLoginInfo>> GetLoginsAsync(string userId);
        Task<string> GetEmailAsync(string userId);
        Task<IdentityResult> AssignToRole(string userId, string role);
        void SetBalance(string userId, decimal balance);
    }
}