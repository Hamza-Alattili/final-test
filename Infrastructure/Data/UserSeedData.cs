using Domain.Const;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UserSeedData
    {
        public static async Task InitializeAsync(ProjectDbContext context)
        {
            if (!context.Roles.Any())
            {
                context.Roles.AddRange
                    (
                    new Role { Name = ProjectConst.ADMIN_ROLE, Code = RoleEnum.Admin },
                    new Role { Name = ProjectConst.DOCTOR_ROLE, Code = RoleEnum.Doctor },
                    new Role { Name = ProjectConst.PATIENT_ROLE, Code = RoleEnum.Patient }
                    );
                await context.SaveChangesAsync();
            }

            if (!context.Users.Any())
            {
                var passwordHasher = new PasswordHasher<User>();
                var adminRoleId = context.Roles.First(a => a.Code == RoleEnum.Admin).Id;

                var admin = new User
                {

                    Name = "System Admin",
                    Email = "Hamza@pro.com",
                    PhoneNumber = "0781505664",
                    RoleId = adminRoleId
                };
                admin.Password = passwordHasher.HashPassword(admin, "Admin@123");

                context.Users.Add(admin);
                await context.SaveChangesAsync();
            }
        }
    }
}
