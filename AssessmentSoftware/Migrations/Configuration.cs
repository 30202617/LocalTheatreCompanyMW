namespace AssessmentSoftware.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AssessmentSoftware.Models.ApplicationDbContext>
    {
        /// <summary>
        /// Method that sets up initial roles
        /// </summary>
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(AssessmentSoftware.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            

            var user = new ApplicationUser();
            user.UserName = "testuser@localtheatrecompany.com";
            user.Email = "testuser@localtheatrecompany.com";

            string userPWD = "A@Z200700";

            UserManager.Create(user, userPWD);


            if (!roleManager.RoleExists("Admin"))
            { 
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
                 

            var adminuser = new ApplicationUser();
            adminuser.UserName = "admin@localtheatrecompany.com";
            adminuser.Email = "admin@localtheatrecompany.com";
            userPWD = "A@Z200711";

            var chkUser = UserManager.Create(adminuser, userPWD);

               
            if (chkUser.Succeeded)
            {
                var result1 = UserManager.AddToRole(adminuser.Id, "Admin");
            }

 
            if (!roleManager.RoleExists("Restricted"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Restricted";
                roleManager.Create(role);
            }
        }
    }
}
