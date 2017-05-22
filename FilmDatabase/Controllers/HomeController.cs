using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FilmDatabase.Models;
using System.IO;
using System.Drawing;
using System.Net;
using Microsoft.AspNet.Identity;
using System.Text;
using FilmDatabase.Filters;
namespace FilmDatabase.Controllers
{
    [Culture]
    public class HomeController : Controller
    {

        IFilmRepository repo;
        IIdentityRepository repos;
        public HomeController()
        {
            repo = new FilmRepository();
            repos = new IdentityRepository();
            
        }
        public HomeController(IFilmRepository context)
        {
            repo = context;
        }
            public HomeController(IFilmRepository context, IIdentityRepository rep)
        {
            repo = context;
            repos = rep;
        }
        public ViewResult Index(string search=null,bool rated=false,string category=null)
        {

            List<Film> films;
            if(category!=null)
            {
                films = repo.Films.Where(a => a.Categories.Any(b => b.Name == category)).ToList();
               
            }
            else
            {
                films = repo.Films.ToList();
            }
            if(rated)
            {
                
                films = repo.Include().Where(a=>a.Marks.Count!=0).OrderByDescending(a => a.Marks.Average(b => b.MarkValue)).ToList();
                
            }
            if(search!=null)
            {
                search = search.ToLower();
                films = repo.Films.Where(a => a.Name.ToLower().Contains(search)).ToList();
            }
            ViewBag.Count = films.Count;
            return View(films);
        }

        public ActionResult Filter(string[] selectedCategories)
        {
            List<Film> films;
            if (selectedCategories != null)
            {
               films = repo.Films.Where(film => selectedCategories.All(cat => film.Categories.Any(fcat => fcat.Name == cat))).ToList();
            }
            else
            {
                films = repo.Films.ToList();
            }
            ViewBag.Count = films.Count;
            return View(films);
        }
        [HttpGet]
        public ActionResult CreateComment()
        {
            
            return View();
        }

       

        [HttpPost]
        public ActionResult CreateComment(Comment comment)
        {
            int filmId = Convert.ToInt32(TempData["FilmId"]);
            if (ModelState.IsValid)
            {
                comment.Date = DateTime.Now;
                comment.UserName = User.Identity.Name;
              

                Film film = repo.Films.Find(m => m.Id == filmId);
                comment.Film = film;

                repo.AddComment(comment);
                repo.SaveChanges();
            }

                return RedirectToAction("Details", new { id = filmId });
            

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film product = repo.Include().Find(m => m.Id == id);
            
            if (product == null)
            {
                return HttpNotFound();
            }
           
            //ApplicationDbContext db = new ApplicationDbContext();
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = repos.Users.FirstOrDefault(x => x.Id == currentUserId);
         //   ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            var list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var aList = list.Select((x, i) => new { Value = x, Data = x }).ToList();
            ViewBag.List = new SelectList(aList, "Value", "Data");
            if (currentUser != null)
            {
                ViewBag.Blocked = currentUser.Blocked;
            }
           
            return View(product);
        }

        public ActionResult PutMark(Mark mark)
        {
            int filmId = Convert.ToInt32(TempData["FilmId"]);
            if (filmId != null)
            {
                if (ModelState.IsValid)
                {

                    mark.UserId = User.Identity.GetUserId();
                    if (repo.Marks.Any(a => a.FilmId == filmId && a.UserId == User.Identity.GetUserId()))
                    {
                        Mark m = repo.Marks.Find(a => a.FilmId == filmId && a.UserId == User.Identity.GetUserId());
                        m.MarkValue = mark.MarkValue;
                        repo.SaveChanges();
                    }
                    else
                    {

                        Film film = repo.Films.Find(m => m.Id == filmId);
                        mark.Film = film;

                        repo.AddMark(mark);
                        repo.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Details", new { id = filmId });
           
        }
        

    }
}