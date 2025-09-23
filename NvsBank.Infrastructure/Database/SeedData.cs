using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NvsBank.Domain.Entities;
using NvsBank.Domain.Entities.Enums;
using NvsBank.Domain.Interfaces;

public sealed class SeedData
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SeedData(IEmployeeRepository employeeRepository, IUnitOfWork unitOfWork)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
    }

    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        // cria a role Admin se não existir
        if (!await roleManager.RoleExistsAsync(nameof(UserRole.Admin)))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(nameof(UserRole.Admin)));
        }

        // cria o admin se não existir
        var adminEmail = "admin@system.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var user = new User
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true,
                Role = UserRole.Admin
            };
            
            var result = await userManager.CreateAsync(user, "Admin123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, nameof(UserRole.Admin));
            }
        }
    }
}