using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using FilmDatabase.Models;
namespace FilmDatabase.Models
{
    public class IdentityDbInit:DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            
            var role1 = new IdentityRole { Name = "admin" };
            var role2 = new IdentityRole { Name = "user" };

           
            roleManager.Create(role1);
            roleManager.Create(role2);

            
            var admin = new ApplicationUser { UserName = "somemail" };
            string password = "qwerty123";
            var result = userManager.Create(admin, password);

           
            if (result.Succeeded)
            {
                
                userManager.AddToRole(admin.Id, role1.Name);
             
            }

            base.Seed(context);
        }
    }
}