using Common;
using Entities.Data;
using Entities.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace FinalProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            try
            {
                var scope = host.Services.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<ShopDbContext>();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                context.Database.EnsureCreated();

                var adminRole = new IdentityRole(RoleConstant.ROLE_ADMIN);
                var customerRole = new IdentityRole(RoleConstant.ROLE_CUSTOMER);
                var shipperRole = new IdentityRole(RoleConstant.ROLE_SHIPPER);
                var warehouseManagerRole = new IdentityRole(RoleConstant.ROLE_WAREHOUSE_MANAGER);

                if (!context.Roles.Any())
                {
                    roleManager.CreateAsync(adminRole).GetAwaiter().GetResult();
                    roleManager.CreateAsync(customerRole).GetAwaiter().GetResult();
                    roleManager.CreateAsync(shipperRole).GetAwaiter().GetResult();
                    roleManager.CreateAsync(warehouseManagerRole).GetAwaiter().GetResult();
                }

                if (!context.Users.Any(u => u.UserName == AdminAccountDefaultConstant.USERNAME))
                {
                    var adminUser = new User
                    {
                        UserName = AdminAccountDefaultConstant.USERNAME,
                        Email = AdminAccountDefaultConstant.EMAIL,
                        FullName = AdminAccountDefaultConstant.FULLNAME,
                        DateOfBirth = AdminAccountDefaultConstant.DATEOFBIRTH,
                        CreateDate = DateTime.Now,
                        ModifyDate = DateTime.Now,
                        IsDeleted = false,
                        EmailConfirmed = true,
                        Gender = AdminAccountDefaultConstant.GENDER,
                        WardCode = AdminAccountDefaultConstant.WARD_CODE,
                        StreetName = AdminAccountDefaultConstant.STREET_NAME,
                        Image = AdminAccountDefaultConstant.IMAGE
                    };

                    var result = userManager.
                        CreateAsync(adminUser, AdminAccountDefaultConstant.PASSWORD).GetAwaiter().GetResult();

                    userManager.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
