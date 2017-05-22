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
namespace FilmDatabase.Tests
{

    [TestClass]
    public class ModeratorControllerTests
    {

        [TestMethod]
        public void Can_Delete_Film()
        {
            var mock = new Mock<IFilmRepository>();
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    new Film{Id=1,Name="Film1"},
                    new Film{Id=2,Name="Film2"}
                });
            var mockUsers = new Mock<IIdentityRepository>();
            ApplicationUser user = new ApplicationUser { Id = "1", Blocked = false, UserName = "UserName1" };
            mockUsers.Setup(u => u.Users).Returns(new List<ApplicationUser>()
                {
                    user
                });
            ModeratorController controller = new ModeratorController(mock.Object, mockUsers.Object);
            controller.DeleteConfirmed(2);
            mock.Verify(a => a.Remove(2));
            mock.Verify(a => a.SaveChanges());

        }

        [TestMethod]
        public void Cannot_Delete_Nonexistent_Film()
        {
            var mock = new Mock<IFilmRepository>();
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    new Film{Id=1,Name="Film1"},
                    new Film{Id=2,Name="Film2"}
                });
            var mockUsers = new Mock<IIdentityRepository>();
            ApplicationUser user = new ApplicationUser { Id = "1", Blocked = false, UserName = "UserName1" };
            mockUsers.Setup(u => u.Users).Returns(new List<ApplicationUser>()
                {
                    user
                });
            ModeratorController controller = new ModeratorController(mock.Object, mockUsers.Object);
            controller.DeleteConfirmed(3);
            mock.Verify(a => a.Remove(3), Times.Never);
            mock.Verify(a => a.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void Can_Edit_Film()
        {
            var mock = new Mock<IFilmRepository>();
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    new Film{Id=1,Name="Film1"},
                    new Film{Id=2,Name="Film2"}
                });
            var mockUsers = new Mock<IIdentityRepository>();
            ApplicationUser user = new ApplicationUser { Id = "1", Blocked = false, UserName = "UserName1" };
            mockUsers.Setup(u => u.Users).Returns(new List<ApplicationUser>()
                {
                    user
                });
            ModeratorController controller = new ModeratorController(mock.Object, mockUsers.Object);
            ActionResult a = controller.Edit(1) as ActionResult;
            ViewResult result = (ViewResult)a;
            Film f1 = result.ViewData.Model as Film;
            Assert.AreEqual(1, f1.Id);
        }

        [TestMethod]
        public void Cannot_Edit_Nonexistant_Film()
        {
            var mock = new Mock<IFilmRepository>();
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    new Film{Id=1,Name="Film1"},
                    new Film{Id=2,Name="Film2"}
                });
            var mockUsers = new Mock<IIdentityRepository>();
            ApplicationUser user = new ApplicationUser { Id = "1", Blocked = false, UserName = "UserName1" };
            mockUsers.Setup(u => u.Users).Returns(new List<ApplicationUser>()
                {
                    user
                });
            ModeratorController controller = new ModeratorController(mock.Object, mockUsers.Object);
            ActionResult a = controller.Edit(5) as ActionResult;
            ViewResult result = (ViewResult)a;
            Film f1 = result.ViewData.Model as Film;
            Assert.IsNull(f1);
        }

        [TestMethod]
        public void Can_Create_Film()
        {
            var mock = new Mock<IFilmRepository>();
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    new Film{Id=1,Name="Film1"},
                    new Film{Id=2,Name="Film2"}
                });
            mock.Setup(m => m.Categories).Returns(new List<Category>()
                {
                    new Category{Id=1,Name="Triller"},
                    new Category{Id=2, Name="Horror"},
                    new Category{Id=3,Name="Comedy"}
                });

            var mockUsers = new Mock<IIdentityRepository>();
            ApplicationUser user = new ApplicationUser { Id = "1", Blocked = false, UserName = "UserName1" };
            mockUsers.Setup(u => u.Users).Returns(new List<ApplicationUser>()
                {
                    user
                });
            ModeratorController controller = new ModeratorController(mock.Object, mockUsers.Object);
            FilmViewModel model = new FilmViewModel { Id = 3, Name = "Film3", Description = "Description", CategoryId = new int[] { 1, 2 } };
            controller.Create(model);
            mock.Verify(a => a.SaveChanges());

        }

        [TestMethod]
        public void Cannot_Create_Invalid_Film()
        {
            var mock = new Mock<IFilmRepository>();
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    new Film{Id=1,Name="Film1"},
                    new Film{Id=2,Name="Film2"}
                });
            mock.Setup(m => m.Categories).Returns(new List<Category>()
                {
                    new Category{Id=1,Name="Triller"},
                    new Category{Id=2, Name="Horror"},
                    new Category{Id=3,Name="Comedy"}
                });
            var mockUsers = new Mock<IIdentityRepository>();
            ApplicationUser user = new ApplicationUser { Id = "1", Blocked = false, UserName = "UserName1" };
            mockUsers.Setup(u => u.Users).Returns(new List<ApplicationUser>()
                {
                    user
                });
            ModeratorController controller = new ModeratorController(mock.Object, mockUsers.Object);
            FilmViewModel model = new FilmViewModel { Id = 3, Name = "Film3" };
            controller.Create(model);
            mock.Verify(a => a.SaveChanges(), Times.Never);
        }
        [TestMethod]
        public void Can_View_Films_Correctly()
        {
            var mock = new Mock<IFilmRepository>();
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    new Film{Id=1,Name="Film1"},
                    new Film{Id=2,Name="Film2"}
                });
            var mockUsers = new Mock<IIdentityRepository>();
            ApplicationUser user = new ApplicationUser { Id = "1", Blocked = false, UserName = "UserName1" };
            mockUsers.Setup(u => u.Users).Returns(new List<ApplicationUser>()
                {
                    user
                });
            ModeratorController controller = new ModeratorController(mock.Object, mockUsers.Object);
            ViewResult result = controller.Index();
            Assert.AreEqual(2, result.ViewBag.Count);

        }

        [TestMethod]
        public void Can_Edit_Categories()
        {
            var mock = new Mock<IFilmRepository>();
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    new Film{Id=1,Name="Film1"},
                    new Film{Id=2,Name="Film2"}
                });
            Category c1 = new Category { Id = 1, Name = "Triller" };
            Category c2 = new Category { Id = 2, Name = "Horror" };
            Category c3 = new Category { Id = 3, Name = "Comedy" };
            mock.Setup(m => m.Categories).Returns(new List<Category>()
                {
                   c1,c2,c3
                    
                });
            var mockUsers = new Mock<IIdentityRepository>();
            ApplicationUser user = new ApplicationUser { Id = "1", Blocked = false, UserName = "UserName1" };
            mockUsers.Setup(u => u.Users).Returns(new List<ApplicationUser>()
                {
                    user
                });
            ModeratorController controller = new ModeratorController(mock.Object, mockUsers.Object);
            controller.Edit(new Film { Id = 1, Name = "Film1" }, new int[] { 1, 2 });

            Film f1 = mock.Object.Films[0];
            Assert.AreEqual(true, f1.Categories.Contains(c1));
            Assert.AreEqual(true, f1.Categories.Contains(c2));
            Assert.IsFalse(f1.Categories.Contains(c3));

        }
    

       


    }
}
