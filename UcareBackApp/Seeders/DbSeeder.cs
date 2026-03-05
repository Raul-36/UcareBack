using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UcareBackApp.Identity.Entities;

namespace UcareBackApp.Seeders
{
    public class DbSeeder
    {
         public static async Task SeedAdminAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<UcareUser>>();
        var roleManager = services.GetRequiredService<RoleManager<UcareRole>>();

        string adminEmail = "admin@admin.com";
        string adminPassword = "Admin123!";
        if (!await roleManager.RoleExistsAsync("admin"))
        {
            await roleManager.CreateAsync(new UcareRole{ Name = "admin" });
        }

        // проверяем есть ли уже админ
        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new UcareUser
            {
                UserName = adminEmail,
                Email = adminEmail
            };

            await userManager.CreateAsync(admin, adminPassword);
            await userManager.AddToRoleAsync(admin, "admin");
        }
    }
}}