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
using FilmDatabase.Filters;
namespace FilmDatabase.Controllers
{
    [Culture]
    public class ModeratorController : Controller
    {
         IFilmRepository repo;
        IIdentityRepository repos;

        public ModeratorController()
        {
            repo = new FilmRepository();
            repos = new IdentityRepository();

        }
        public ModeratorController(IFilmRepository context, IIdentityRepository repository)
        {
            repo = context;
            repos = repository;
        }
        [Authorize(Roles = "moderator")]
        public ViewResult Index()
        {
            ViewBag.Count = repo.Films.Count;
            return View(repo.Films);
        }
        [Authorize(Roles = "moderator")]
        [HttpGet]
        public ActionResult DeleteComment(int id)
        {
            Comment c = repo.Comments.Find(a => a.Id == id);
            if (c == null)
            {
                return HttpNotFound();
            }
            return View(c);
        }
        [Authorize(Roles = "moderator")]
        [HttpPost, ActionName("DeleteComment")]
        public ActionResult DeleteCommentConfirmed(int id)
        {
            Comment c = repo.Comments.Find(a => a.Id == id);


            if (c == null)
            {
                return HttpNotFound();
            }
            int filmId = c.FilmId;
            repo.RemoveComment(id);
            return RedirectToAction("Details", "Home", new { id = filmId });

        }
        [Authorize(Roles = "moderator")]
        public ActionResult Create()
        {

            ViewBag.Categories = repo.Categories.ToList();
            var model = new FilmViewModel();
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "moderator")]
        //  [ValidateAntiForgeryToken]
        public ActionResult Create(FilmViewModel model)
        {
            var imageTypes = new string[]{    
                    "image/gif",
                    "image/jpeg",
                    "image/pjpeg",
                    "image/png"
                };
            //  if (model.Image == null || model.Image.ContentLength == 0)
            //   {
            //      ModelState.AddModelError("ImageUpload", "Додайте зображення");
            //   }
            if (model.Image != null && !imageTypes.Contains(model.Image.ContentType))
            {
                ModelState.AddModelError("Image", "Зображення повинне бути у GIF, JPG або PNG форматі.");
            }
            else if (model.CategoryId == null)
            {
                ModelState.AddModelError("CategoryId", "Додайте жанр");
            }

            if (ModelState.IsValid)
            {
                var film = new Film();
                film.Name = model.Name;
                film.Description = model.Description;
                if (model.CategoryId != null)
                {
                    foreach (var c in model.CategoryId)
                    {
                        if (repo.Categories.Find(m => m.Id == c) != null)
                        {
                            film.Categories.Add(repo.Categories.Find(m => m.Id == c));
                        }
                    }

                }
                if (model.Image != null)
                {
                    byte[] imageData = null;
                    var binaryReader = new BinaryReader(model.Image.InputStream);

                    imageData = binaryReader.ReadBytes(model.Image.ContentLength);
                    binaryReader.Close();
                    MemoryStream ms = new MemoryStream(imageData);
                    Image returnImage = Image.FromStream(ms);
                    ms.Close();
                    ViewBag.Width = returnImage.Width;
                    ViewBag.Height = returnImage.Height;
                    film.Image = imageData;
                }
                repo.Add(film);
                repo.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = repo.Categories;
            return View(model);
        }
        [Authorize(Roles = "moderator")]
        public ActionResult Delete(int id)
        {
            Film f = repo.Films.Find(m => m.Id == id);
            if (f == null)
            {
                return HttpNotFound();
            }
            return View(f);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "moderator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Film f = repo.Films.Find(m => m.Id == id);
            if (f == null)
            {
                return HttpNotFound();
            }
            repo.Remove(id);
            repo.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "moderator")]
        public ActionResult Edit(int? id)
        {
            ViewBag.Categories = repo.Categories;
            Film f = repo.Films.Find(m => m.Id == id);

            return View(f);


        }
        [HttpPost]
        [Authorize(Roles = "moderator")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Film model, int[] selectedCategories, HttpPostedFileBase upload = null)
        {
            if (ModelState.IsValid)
            {
                var product = repo.Films.Find(m => m.Id == model.Id);
                if (product == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                product.Name = model.Name;
                product.Description = model.Description;
                if (selectedCategories != null)
                {
                    product.Categories.Clear();
                    foreach (var c in selectedCategories)
                    {
                        if (repo.Categories.Find(m => m.Id == c) != null)
                        {
                            product.Categories.Add(repo.Categories.Find(m => m.Id == c));
                        }
                    }

                }
                if (upload != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(upload.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(upload.ContentLength);
                    }
                    MemoryStream ms = new MemoryStream(imageData);
                    Image returnImage = Image.FromStream(ms);
                    ViewBag.Width = returnImage.Width;
                    ViewBag.Height = returnImage.Height;
                    product.Image = imageData;
                }


                repo.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }
	}
}