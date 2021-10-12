using Foromanager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Foromanager.Authorization;
// dotnet aspnet-codegenerator razorpage -m Contact -dc ApplicationDbContext -udl -outDir Pages\Contacts --referenceScriptLibraries

namespace Foromanager.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@forum.com");
                await EnsureRole(serviceProvider, adminID, Constants.ForumAdministratorsRole);

                // allowed user can create and edit contacts that they create
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@forum.com");
                await EnsureRole(serviceProvider, managerID, Constants.ForumManagersRole);

                SeedDB(context, adminID);
            }
        }        
        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if(user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }
            
            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
        public static void SeedDB(ApplicationDbContext context, string adminID)
        {
            if (context.Foro.Any())
            {
                return;   // DB has been seeded
            }
            var foros = new Foro[]
            {
                new Foro{Nombre="GithNik",Descripcion="Foro Principal",Categoria="Principal",Fecha=DateTime.Parse("2020-09-01"),Status=ForumStatus.Approved,OwnerID=adminID},
                new Foro{Nombre="Genshin",Descripcion="Foro de Genshin",Categoria="Genshin",Fecha=DateTime.Parse("2020-09-02")},
                new Foro{Nombre="Minecraft",Descripcion="Foro de Minecraft",Categoria="Principal",Fecha=DateTime.Parse("2020-09-03")},
                new Foro{Nombre="ElRichMC",Descripcion="Foro del rich",Categoria="Principal",Fecha=DateTime.Parse("2020-09-04")}
            };
            context.Foro.AddRange(foros);
            context.SaveChanges();

            var publicaciones = new Publicacion[]
            {
                new Publicacion{ForoId=1,Titulo="Que bueno el GithNik",Descripcion="Me gusta mucho ;D"}
            };
            context.Publicacion.AddRange(publicaciones);
            context.SaveChanges();
        }

    }
}