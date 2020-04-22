using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MyShop.Helpers;
using MyShop.Models;

namespace MyShop.Data
{
    public class DBInitializer : IDBInitializer
    {
        private readonly ShopDataContext _dataContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DBInitializer(ShopDataContext dataContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dataContext = dataContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async void Initialize()
        {
            if(_dataContext.Database.GetPendingMigrations().Count() > 0)
            {
                _dataContext.Database.Migrate();
            }

            if (_dataContext.Roles.Any(r => r.Name == Roles.Admin))
                return;

            _roleManager.CreateAsync(new IdentityRole(Roles.Admin)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "Admin",
                Email = "Admin@gmail.com",
                EmailConfirmed = true,
            },"Admin@123").GetAwaiter().GetResult();

          
            await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync("Admin"),Roles.Admin);

        }
    }
}
