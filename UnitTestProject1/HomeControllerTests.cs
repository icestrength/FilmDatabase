using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using FilmDatabase.Models;
using FilmDatabase.Controllers;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading;
using System.Web;
using System.Drawing;
using System.Security.Principal;
using System.IdentityModel.Claims;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
namespace FilmDatabase.Tests
{
    /// <summary>
    /// Summary description for HomeControllerTests
    /// </summary>
    [TestClass]
    public class HomeControllerTests
    {
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Can_Add_Comment()
        {
            var mock = new Mock<IFilmRepository>();
            Film f1 = new Film { Id = 1, Name = "Film1" };
            Film f2 = new Film { Id = 2, Name = "Film2" };
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                   f1,
                   f2
                });
            mock.Setup(m => m.Comments).Returns(new List<Comment>());

            var comment = new Comment { Body = "description" };
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);
            var controller = new HomeController(mock.Object);
            controller.ControllerContext = controllerContext.Object;
            controller.CreateComment(comment);
            mock.Verify(a => a.AddComment(comment));
            mock.Verify(a => a.SaveChanges());
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
            HomeController controller = new HomeController(mock.Object);
            ViewResult result = controller.Index();
            Assert.AreEqual(2, result.ViewBag.Count);

        }

        [TestMethod]
        public void Can_Put_Mark()
        {
            var mock = new Mock<IFilmRepository>();
            Film f1 = new Film { Id = 1, Name = "Film1" };
            Film f2 = new Film { Id = 2, Name = "Film2" };
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                   f1,
                   f2
                });
            mock.Setup(m => m.Comments).Returns(new List<Comment>());
            mock.Setup(m => m.Include()).Returns(mock.Object.Films);
            var comment = new Comment { Body = "description" };
            mock.Setup(m => m.Marks).Returns(new List<Mark>());
            var mark = new Mark { MarkValue = 5, FilmId = 1 };
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);
            var controller = new HomeController(mock.Object);

            controller.ControllerContext = controllerContext.Object;

            controller.PutMark(mark);

            mock.Verify(a => a.AddMark(mark));
            mock.Verify(a => a.SaveChanges());
        }
     
        [TestMethod]
        public void CanViewFilmsByCategory()
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

            HomeController controller = new HomeController(mock.Object);
            ViewResult result = controller.Filter(new string[] { "Triller" }) as ViewResult;
            Assert.AreEqual(2, result.ViewBag.Count);
        }
        [TestMethod]
        public void CanSearchByName()
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

            HomeController controller = new HomeController(mock.Object);
            ViewResult result = controller.Index("Film",false,null);
            Assert.AreEqual(2, result.ViewBag.Count);
        }
        [TestMethod]
        public void CannotSearchNonExistant()
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

            HomeController controller = new HomeController(mock.Object);
            ViewResult result = controller.Index("search", false, null);
            Assert.AreEqual(0, result.ViewBag.Count);
        }
        [TestMethod]
        public void CanViewCorrectDetails()
        {
            var mock = new Mock<IFilmRepository>();
            Film f1 = new Film { Id = 1, Name = "Film1" };
            Film f2 = new Film { Id = 2, Name = "Film2" };
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                   f1,
                   f2
                });
            mock.Setup(m => m.Comments).Returns(new List<Comment>());
            mock.Setup(m => m.Include()).Returns(mock.Object.Films);
            var comment = new Comment { Body = "description" };
            mock.Setup(m => m.Marks).Returns(new List<Mark>());
            var mark = new Mark { MarkValue = 5, FilmId = 20 };
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);
           
            var mockUsers = new Mock<IIdentityRepository>();
            ApplicationUser user = new ApplicationUser { Id = "1", Blocked = false, UserName = "UserName1" };
            mockUsers.Setup(u => u.Users).Returns(new List<ApplicationUser>()
                {
                    user
                });
            var controller = new HomeController(mock.Object,mockUsers.Object);
            controller.ControllerContext = controllerContext.Object;

           var res= controller.Details(1) as ViewResult;
            Film expected=(Film)res.Model;
           Assert.IsNotNull(res.Model);
           Assert.AreEqual(expected.Name, "Film1");
        }
        [TestMethod]
        public void CannotViewNonExistantDetails()
        {
            var mock = new Mock<IFilmRepository>();
            Film f1 = new Film { Id = 1, Name = "Film1" };
            Film f2 = new Film { Id = 2, Name = "Film2" };
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                   f1,
                   f2
                });
            mock.Setup(m => m.Comments).Returns(new List<Comment>());
            mock.Setup(m => m.Include()).Returns(mock.Object.Films);
            var comment = new Comment { Body = "description" };
            mock.Setup(m => m.Marks).Returns(new List<Mark>());
            var mark = new Mark { MarkValue = 5, FilmId = 20 };
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);

            var mockUsers = new Mock<IIdentityRepository>();
            ApplicationUser user = new ApplicationUser { Id = "1", Blocked = false, UserName = "UserName1" };
            mockUsers.Setup(u => u.Users).Returns(new List<ApplicationUser>()
                {
                    user
                });
            var controller = new HomeController(mock.Object, mockUsers.Object);
            controller.ControllerContext = controllerContext.Object;

            var res = controller.Details(4) as ViewResult;
            Assert.IsNull(res);
        }
      
    }
}
