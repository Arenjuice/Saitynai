
using api.Auth.Model;
using Microsoft.AspNetCore.Identity;

namespace api.Auth
{
    public class AuthSeeder
    {
        private readonly UserManager<SystemUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthSeeder(UserManager<SystemUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await AddDefaultRolesAsync();
            await AddAdminUserAsync();
        }

        private async Task AddAdminUserAsync()
        {
            var newAdminUser = new SystemUser()
            {
                UserName = "admin",
                Email = "admin@admin.com"
            };

            var existAdminUser = await _userManager.FindByNameAsync(newAdminUser.UserName);
            if (existAdminUser == null)
            {
                var createAdminUserResult = await _userManager.CreateAsync(newAdminUser, "VerySafePassword1!");
                if (createAdminUserResult.Succeeded)
                    await _userManager.AddToRolesAsync(newAdminUser, SystemRoles.All);
            }
        }

        private async Task AddDefaultRolesAsync()
        {
            foreach (var role in SystemRoles.All)
            {
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (!roleExists)
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
