using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace FilmDatabase.Models
{
    public class IdentityRepository:IIdentityRepository
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        public ApplicationUser FindById(string id)
        {
            return context.Users.Single(i => i.Id == id);
        }

        public List<ApplicationUser> Users
        {
            get { return context.Users.ToList(); }
        }

        
        public void SaveChanges()
        {
            context.SaveChanges();
        }


        public List<ApplicationUser> GetUsersInRole(string role)
        {
            var role1 = context.Roles.SingleOrDefault(m => m.Name == "user");
            var usersInRole = context.Users.Where(m => m.Roles.Any(r => r.RoleId == role1.Id)).ToList();
            return usersInRole;
        }


        public List<Microsoft.AspNet.Identity.EntityFramework.IdentityRole> Roles
        {
            get
            {
                return context.Roles.ToList();
               
            }
            set
            {
              
            }
        }


        public Microsoft.AspNet.Identity.EntityFramework.IdentityRole FindRole(string role)
        {
            return context.Roles.Where(a => a.Name == role).Single();
        }
    }
}