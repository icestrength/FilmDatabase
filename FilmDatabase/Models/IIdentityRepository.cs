using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
namespace FilmDatabase.Models
{
    public interface IIdentityRepository
    {
        List<ApplicationUser> Users { get; }
        ApplicationUser FindById(string id);
        List<ApplicationUser> GetUsersInRole(string role);
        List<IdentityRole> Roles { get; set; }
        IdentityRole FindRole(string role);
        void SaveChanges();
    }
}
