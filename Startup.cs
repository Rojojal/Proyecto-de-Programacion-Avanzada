using Libreria.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Libreria.Startup))]
namespace Libreria
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            InitializeRolesAndAdminUser();
        }

        private void InitializeRolesAndAdminUser()
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                if (!roleManager.RoleExists("Admin"))
                {
                    var role = new IdentityRole("Admin");
                    roleManager.Create(role);
                }

                if (!roleManager.RoleExists("User"))
                {
                    var role = new IdentityRole("User");
                    roleManager.Create(role);
                }

                if (userManager.FindByName("admin@domain.com") == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@domain.com"
                    };

                    var adminPassword = "Admin123!";
                    var userCreationResult = userManager.Create(adminUser, adminPassword);

                    if (userCreationResult.Succeeded)
                    {
                        userManager.AddToRole(adminUser.Id, "Admin");
                    }   
                }
            }

        }
    }
}

