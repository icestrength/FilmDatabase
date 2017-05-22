using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using FilmDatabase.Models;
using FilmDatabase.Controllers;
using System.Web.Mvc;
using System.Data.Entity;
using System.Web;
using System.Drawing;
using System.Security.Principal;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
namespace UnitTestProject1
{
    [TestClass]
    public class NavigationControllerTests
    {
        [TestMethod]
        public void CanViewCategoryList()
        {
            var mock = new Mock<IFilmRepository>();
            var c1 = new Category { Id = 1, Name = "Triller" };
            var c2 = new Category { Id = 2, Name = "Horror" };
            var c3 = new Category { Id = 3, Name = "Comedy" };
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    new Film{Id=1,Name="Film1", Categories={c1}},
                    new Film{Id=2,Name="Film2",Categories={c1,c2}}
                });
            mock.Setup(m => m.Categories).Returns(new List<Category>()
                {
                    c1,c2,c3
                });
            NavigationController controller = new NavigationController(mock.Object);
            string[] results = ((IEnumerable<string>)controller.Menu().Model).ToArray();
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Comedy");
            Assert.AreEqual(results[1], "Horror");
        }
        [TestMethod]
        public void SelectsCorrectCategory()
        {
            var mock = new Mock<IFilmRepository>();
            var c1 = new Category { Id = 1, Name = "Triller" };
            var c2 = new Category { Id = 2, Name = "Horror" };
            var c3 = new Category { Id = 3, Name = "Comedy" };
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    new Film{Id=1,Name="Film1", Categories={c1}},
                    new Film{Id=2,Name="Film2",Categories={c1,c2}}
                });
            mock.Setup(m => m.Categories).Returns(new List<Category>()
                {
                    c1,c2,c3
                });
            NavigationController controller = new NavigationController(mock.Object);
            string categoryToSelect = "Thriller";
            string result =controller.Menu(categoryToSelect).ViewBag.SelectedCategory;
            Assert.AreEqual(categoryToSelect, result);
            
        }
        [TestMethod]
        public void CorrectSearchString()
        {
            var mock = new Mock<IFilmRepository>();
            var c1 = new Category { Id = 1, Name = "Triller" };
            var c2 = new Category { Id = 2, Name = "Horror" };
            var c3 = new Category { Id = 3, Name = "Comedy" };
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    new Film{Id=1,Name="Film1", Categories={c1}},
                    new Film{Id=2,Name="Film2",Categories={c1,c2}}
                });
            mock.Setup(m => m.Categories).Returns(new List<Category>()
                {
                    c1,c2,c3
                });
            NavigationController controller = new NavigationController(mock.Object);
            string filmToSelect = "Film1";
            string result = controller.Search(filmToSelect).ViewBag.Search;
            Assert.AreEqual(filmToSelect, result);
        }
    }
}
