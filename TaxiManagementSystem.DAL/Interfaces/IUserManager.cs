using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.DAL.Interfaces
{
    public interface IUserManager //: IDisposable
    {
        IEnumerable<ApplicationBaseIdentity> GetUsers();
        IEnumerable<ApplicationUser> GetAllUsers();
        IEnumerable<ApplicationDriver> GetAllDrivers();
        IEnumerable<ApplicationAdmin> GetAllAdmins();
        ApplicationBaseIdentity FindById(string id);
        ApplicationBaseIdentity FindByEmail(string email);
        ApplicationBaseIdentity FindByUsername(string username);
        Task<ApplicationBaseIdentity> FindAsync(string username, string password);
        bool ConfirmUserByEmail(string email, string token);
        string RegisterUser(string userName, string email, Guid userRole, out string userId);
        string RegisterDriver(string userName, string email, Guid userRole, out string userId);
        string RegisterAdmin(string userName, string email, Guid userRole, out string userId);
        bool DeleteUserReg(Guid id);
        Dictionary<string, string> GetRoles();
        Task<IdentityResult> CreateAsync(ApplicationBaseIdentity user, string password);
        Task<ClaimsIdentity> CreateIdentityAsync(ApplicationBaseIdentity user, string authenticationType);
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<IdentityResult> AddPasswordAsync(string userId, string password);
        Task<string> GetPhoneNumberAsync(string userId);
        Task<ApplicationBaseIdentity> FindByIdAsync(string id);
        Task<IList<UserLoginInfo>> GetLoginsAsync(string userId);
        Task<string> GetEmailAsync(string userId);
        Task<IdentityResult> AssignToRole(string userId, string role);
        void SetBalance(string userId, decimal balance);
    }
}