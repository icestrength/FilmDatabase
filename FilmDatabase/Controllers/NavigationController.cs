using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FilmDatabase.Models;
using FilmDatabase.Filters;
namespace FilmDatabase.Controllers
{
    [Culture]
    public class NavigationController : Controller
    {
        IFilmRepository repo;
        public NavigationController()
        {
            repo = new FilmRepository();
            //     TempData["Pressed"] = "false";

        }
        public NavigationController(IFilmRepository context)
        {
            repo = context;
            //    TempData["Pressed"] = "false";
        }
        //
        // GET: /Navigation/
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            List<string> categories = repo.Categories.Select(x => x.Name).Distinct().OrderBy(x => x).ToList();
            return PartialView(categories);
        }

        public PartialViewResult Search(string search)
        {
            ViewBag.Search = search;
            return PartialView();
        }
        public ActionResult ShowGenres(bool pressed)
        {
            if (!pressed)
            {
                TempData["Pressed"] = "true";

            }
            else
            {
                TempData["Pressed"] = "false";
            }
            TempData.Keep("Pressed");
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            // Список культур
            List<string> cultures = new List<string>() { "ru", "en", "de" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }
            // Сохраняем выбранную культуру в куки
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;   // если куки уже установлено, то обновляем значение
            else
            {

                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }


    }
}