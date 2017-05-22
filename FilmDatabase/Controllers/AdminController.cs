using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FilmDatabase.Models;
using System.IO;
using System.Drawing;
using System.Net;
using System.Text;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using FilmDatabase.Filters;
namespace FilmDatabase.Controllers
{
    [Culture]
    public class AdminController : Controller
    {
        IFilmRepository repo;
        IIdentityRepository repos;
        public AdminController()
        {
            repo = new FilmRepository();
            repos = new IdentityRepository();

        }
        public AdminController(IFilmRepository context, IIdentityRepository repository)
        {
            repo = context;
            repos = repository;
        }


     
        [Authorize(Roles = "admin")]
        public ViewResult ViewUsers()
        {

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
           foreach(var user in repos.Users)
           {
               if(user.Roles.Count==0)
               {
                   userManager.AddToRole(user.Id, "user");
               }
           }
            ViewBag.UserCount = repos.GetUsersInRole("user").Count;
            return View(repos.Users.Where(a=>a.Id!=User.Identity.GetUserId()).ToList());

        }

        [Authorize(Roles = "admin")]
        public ActionResult Ban(string id)
        {

            var user = repos.FindById(id);

            if (user.Blocked)
            {
                user.Blocked = false;
            }
            else
            {
                user.Blocked = true;
            }

            repos.SaveChanges();
            return RedirectToAction("ViewUsers", "Admin");
        }

        [Authorize(Roles = "admin")]
        public ActionResult PromoteToRole(string id, string role)
        {
          //  var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            
            var user = repos.FindById(id);
            var role1 = repos.FindRole(role);
            user.Roles.Clear();
            IdentityUserRole userRole = new IdentityUserRole { RoleId = role1.Id, UserId = user.Id };
            user.Roles.Add(userRole);
            repos.SaveChanges();

           
          
          return RedirectToAction("ViewUsers", "Admin");

        }

    }
}