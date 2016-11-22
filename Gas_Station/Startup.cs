using Gas_Station.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Gas_Station.Startup))]
namespace Gas_Station
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //Create Admin Role
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            //Create DefaultUser Role
            if (!roleManager.RoleExists("DefaultUser"))
            {
                var role = new IdentityRole();
                role.Name = "DefaultUser";
                roleManager.Create(role);
            }

            //////Create an Admin
            //var user = new ApplicationUser();
            //user.UserName = "xkourtz@hotmail.com";
            //user.Email = "xkourtz@hotmail.com";
            //string Password = "xk@123456";
            //var adminUser1 = UserManager.Create(user, Password);
            //if (adminUser1.Succeeded)
            //{
            //    var assignRole = UserManager.AddToRole(user.Id, "Admin");
            //}

        }

    }
}
