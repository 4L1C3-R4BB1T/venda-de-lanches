using Microsoft.AspNetCore.Identity;

namespace VendaLanches.Services;

public class SeedUserRoleInitial : ISeedUserRoleInitial
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SeedUserRoleInitial(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void SeedRoles()
    {
        var roleName = "";

        if (!_roleManager.RoleExistsAsync("Member").Result) 
            roleName = "Member";

        if (!_roleManager.RoleExistsAsync("Admin").Result) 
            roleName = "Admin";

        IdentityRole role = new IdentityRole
        {
            Name = roleName,
            NormalizedName = roleName.ToUpper()
        };
        _ = _roleManager.CreateAsync(role).Result;
    }

    public void SeedUsers()
    {
        var email = "";
        var role = "";

        if (_userManager.FindByEmailAsync("usuario@localhost").Result == null)
        {
            email = "usuario@localhost";
            role = "Member";
        }

        if (_userManager.FindByEmailAsync("admin@localhost").Result == null)
        {
            email = "admin@localhost";
            role = "Admin";
        }

        IdentityUser user = new IdentityUser
        {
            UserName = email,
            Email = email,
            NormalizedUserName = email.ToUpper(),
            NormalizedEmail = email.ToUpper(),
            EmailConfirmed = true,
            LockoutEnabled = false,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        IdentityResult result = _userManager.CreateAsync(user, "Numsey#2022").Result;

        if (result.Succeeded)
            _userManager.AddToRoleAsync(user, role).Wait();
    }
}
