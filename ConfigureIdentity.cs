using Microsoft.AspNetCore.Identity;
using timely_backend.Models;
using timely_backend.Models.Enum;

namespace timely_backend;

public static class ConfigureIdentity {
    public static async Task ConfigureIdentityAsync(this WebApplication app) {
        using var serviceScope = app.Services.CreateScope();
        var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
        var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();
        var config = app.Configuration.GetSection("DefaultUsersConfig");

        // Try to create Administrator Role
        var adminRole = await roleManager.FindByNameAsync(ApplicationRoleNames.Administrator);
        if (adminRole == null) {
            var roleResult = await roleManager.CreateAsync(new Role {
                Name = ApplicationRoleNames.Administrator,
                Type = RoleType.Administrator
            });
            if (!roleResult.Succeeded) {
                throw new InvalidOperationException($"Unable to create {ApplicationRoleNames.Administrator} role.");
            }

            adminRole = await roleManager.FindByNameAsync(ApplicationRoleNames.Administrator);
        }

        // Try to create Composer Role
        var composerRole = await roleManager.FindByNameAsync(ApplicationRoleNames.Composer);
        if (composerRole == null) {
            var roleResult = await roleManager.CreateAsync(new Role {
                Name = ApplicationRoleNames.Composer,
                Type = RoleType.Composer
            });
            if (!roleResult.Succeeded) {
                throw new InvalidOperationException($"Unable to create {ApplicationRoleNames.Composer} role.");
            }
        }
        
        // Try to create Teacher Role
        var teacherRole = await roleManager.FindByNameAsync(ApplicationRoleNames.Teacher);
        if (teacherRole == null) {
            var roleResult = await roleManager.CreateAsync(new Role {
                Name = ApplicationRoleNames.Teacher,
                Type = RoleType.Teacher
            });
            if (!roleResult.Succeeded) {
                throw new InvalidOperationException($"Unable to create {ApplicationRoleNames.Teacher} role.");
            }
        }

        // Try to create Student Role
        var studentRole = await roleManager.FindByNameAsync(ApplicationRoleNames.Student);
        if (studentRole == null) {
            var roleResult = await roleManager.CreateAsync(new Role {
                Name = ApplicationRoleNames.Student,
                Type = RoleType.Student
            });
            if (!roleResult.Succeeded) {
                throw new InvalidOperationException($"Unable to create {ApplicationRoleNames.Student} role.");
            }
        }

        // Try to create Administrator user
        var adminUser = await userManager.FindByEmailAsync(config["AdminEmail"]);
        if (adminUser == null) {
            var userResult = await userManager.CreateAsync(new User {
                FullName = config["AdminFullName"],
                UserName = config["AdminUserName"],
                Email = config["AdminEmail"],
            }, config["AdminPassword"]);
            if (!userResult.Succeeded) {
                throw new InvalidOperationException($"Unable to create {config["AdminUserName"]} user");
            }
            
            adminUser = await userManager.FindByNameAsync(config["AdminUserName"]);
        }

        if (!await userManager.IsInRoleAsync(adminUser, adminRole.Name)) {
            await userManager.AddToRoleAsync(adminUser, adminRole.Name);
        }
    }
}