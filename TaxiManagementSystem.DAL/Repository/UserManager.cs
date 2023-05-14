using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaxiManagementSystem.DAL.Interfaces;
using TaxiManagementSystem.DTO;

namespace TaxiManagementSystem.DAL.Repository
{
    public class UserManager : IUserManager
    {
        private bool _disposed;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationBaseIdentity> _baseUserManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserManager<ApplicationDriver> _driverManager;
        private readonly UserManager<ApplicationAdmin> _adminManager;

        public UserManager()
        {
            _context = new ApplicationDbContext();
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
            _baseUserManager = new UserManager<ApplicationBaseIdentity>(new UserStore<ApplicationBaseIdentity>(_context));
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            _driverManager = new UserManager<ApplicationDriver>(new UserStore<ApplicationDriver>(_context));
            _adminManager = new UserManager<ApplicationAdmin>(new UserStore<ApplicationAdmin>(_context));
            _baseUserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationBaseIdentity>(new DpapiDataProtectionProvider("TaxiService").Create("EmailConfirmation"));
        }

        public IEnumerable<ApplicationBaseIdentity> GetUsers()
        {
            return _baseUserManager.Users.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedDate).AsEnumerable();
        }

        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedDate).AsEnumerable();
        }

        public IEnumerable<ApplicationDriver> GetAllDrivers()
        {
            return _driverManager.Users.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedDate).AsEnumerable();
        }

        public IEnumerable<ApplicationAdmin> GetAllAdmins()
        {
            return _adminManager.Users.Where(x => !x.IsDeleted).OrderByDescending(x => x.CreatedDate).AsEnumerable();
        }

        public ApplicationBaseIdentity FindById(string id)
        {
            return _baseUserManager.FindById(id);
        }

        public ApplicationBaseIdentity FindByEmail(string email)
        {
            return _baseUserManager.FindByEmail(email);
        }

        public ApplicationBaseIdentity FindByUsername(string username)
        {
            return _baseUserManager.FindByName(username);
        }

        public async Task<ApplicationBaseIdentity> FindAsync(string username, string password)
        {
            return await _baseUserManager.FindAsync(username, password);
        }

        public bool ConfirmUserByEmail(string email, string token)
        {
            return _baseUserManager.ConfirmEmail(_baseUserManager.FindByEmail(email).Id, token).Succeeded;
        }

        public string RegisterUser(string userName, string email, Guid userRole, out string userId)
        {
            userId = "";
            string token = "";
            ApplicationUser user = new ApplicationUser
            {
                UserName = userName,
                Email = email
            };
            IdentityResult chkUser = _baseUserManager.Create(user);
            if (chkUser.Succeeded)
            {
                userId = _baseUserManager.FindByEmail(email).Id;
                token = _baseUserManager.GenerateEmailConfirmationToken(user.Id);
                _baseUserManager.AddToRole(userId, _roleManager.FindById(userRole.ToString()).Name);
            }
            return token;
        }

        public string RegisterDriver(string userName, string email, Guid userRole, out string userId)
        {
            userId = "";
            string token = "";
            ApplicationDriver user = new ApplicationDriver
            {
                UserName = userName,
                Email = email
            };
            IdentityResult chkUser = _baseUserManager.Create(user);
            if (chkUser.Succeeded)
            {
                userId = _baseUserManager.FindByEmail(email).Id;
                token = _baseUserManager.GenerateEmailConfirmationToken(user.Id);
                _baseUserManager.AddToRole(userId, _roleManager.FindById(userRole.ToString()).Name);
            }
            return token;
        }

        public string RegisterAdmin(string userName, string email, Guid userRole, out string userId)
        {
            userId = "";
            string token = "";
            ApplicationAdmin user = new ApplicationAdmin
            {
                UserName = userName,
                Email = email
            };
            IdentityResult chkUser = _baseUserManager.Create(user);
            if (chkUser.Succeeded)
            {
                userId = _baseUserManager.FindByEmail(email).Id;
                token = _baseUserManager.GenerateEmailConfirmationToken(user.Id);
                _baseUserManager.AddToRole(userId, _roleManager.FindById(userRole.ToString()).Name);
            }
            return token;
        }

        public bool DeleteUserReg(Guid id)
        {
            var user = _baseUserManager.FindById(id.ToString());
            user.IsDeleted = true;
            return _baseUserManager.Update(user).Succeeded;
        }

        public Dictionary<string, string> GetRoles()
        {
            return _roleManager.Roles.ToDictionary(x => x.Id, x => x.Name);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationBaseIdentity user, string password)
        {
            return await _baseUserManager.CreateAsync(user, password);
        }

        public async Task<ClaimsIdentity> CreateIdentityAsync(ApplicationBaseIdentity user, string authenticationType)
        {
            return await _baseUserManager.CreateIdentityAsync(user, authenticationType);
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            return await _baseUserManager.ChangePasswordAsync(userId, currentPassword, newPassword);
        }

        public async Task<IdentityResult> AddPasswordAsync(string userId, string password)
        {
            return await _baseUserManager.AddPasswordAsync(userId, password);
        }

        public async Task<string> GetPhoneNumberAsync(string userId)
        {
            return await _baseUserManager.GetPhoneNumberAsync(userId);
        }

        public async Task<ApplicationBaseIdentity> FindByIdAsync(string id)
        {
            return await _baseUserManager.FindByIdAsync(id);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(string userId)
        {
            return await _baseUserManager.GetLoginsAsync(userId);
        }

        public async Task<string> GetEmailAsync(string userId)
        {
            return await _baseUserManager.GetEmailAsync(userId);
        }

        public async Task<IdentityResult> AssignToRole(string userId, string role)
        {
            return await _baseUserManager.AddToRoleAsync(userId, role);
        }

        public void SetBalance(string userId, decimal balance)
        {
            var user = _baseUserManager.FindById(userId);
            if (user == null) return;
            user.Balance = balance;
            _baseUserManager.Update(user);
        }

        //public void Dispose()
        //{
        //    Dispose(true);
        //    GC.SuppressFinalize(this);
        //}

        //private void Dispose(bool disposing)
        //{
        //    if (!_disposed && disposing)
        //    {
        //        _context?.Dispose();
        //        _userManager?.Dispose();
        //        _baseUserManager?.Dispose();
        //        _adminManager?.Dispose();
        //        _driverManager?.Dispose();
        //        _roleManager?.Dispose();
        //    }
        //    _disposed = true;
        //}
    }
}