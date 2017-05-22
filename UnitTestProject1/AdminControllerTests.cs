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
    public class AdminControllerTests
    {

        [TestMethod]
        public void Can_Delete_Comment()
        {
            var mock = new Mock<IFilmRepository>();
            Film f1 = new Film { Id = 1, Name = "Film1" };
            Film f2 = new Film { Id = 2, Name = "Film2" };
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    f1,f2
                });
            mock.Setup(m => m.Comments).Returns(new List<Comment>()
                {
                    new Comment{Id=1, UserName="User1", Body="Description", FilmId=1, Date=DateTime.Now, Film=f1}
                });
            var mockUsers = new Mock<IIdentityRepository>();
            ApplicationUser user = new ApplicationUser { Id = "1", Blocked = false, UserName = "UserName1" };
            mockUsers.Setup(u => u.Users).Returns(new List<ApplicationUser>()
                {
                    user
                });
            ModeratorController controller = new ModeratorController(mock.Object, mockUsers.Object);
            controller.DeleteCommentConfirmed(1);
            mock.Verify(a => a.RemoveComment(1));


        }
        [TestMethod]
        public void Cannot_Delete_NonExistant_Comment()
        {
            var mock = new Mock<IFilmRepository>();
            Film f1 = new Film { Id = 1, Name = "Film1" };
            Film f2 = new Film { Id = 2, Name = "Film2" };
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    f1,f2
                });
            mock.Setup(m => m.Comments).Returns(new List<Comment>()
                {
                    new Comment{Id=1, UserName="User1", Body="Description", FilmId=1, Date=DateTime.Now, Film=f1}
                });
            var mockUsers = new Mock<IIdentityRepository>();
            ApplicationUser user = new ApplicationUser { Id = "1", Blocked = false, UserName = "UserName1" };
            mockUsers.Setup(u => u.Users).Returns(new List<ApplicationUser>()
                {
                    user
                });
            ModeratorController controller = new ModeratorController(mock.Object, mockUsers.Object);
            controller.DeleteCommentConfirmed(2);
            mock.Verify(a => a.RemoveComment(2), Times.Never);
        }
        [TestMethod]
        public void Can_Ban_User()
        {
            var mock = new Mock<IFilmRepository>();
            Film f1 = new Film { Id = 1, Name = "Film1" };
            Film f2 = new Film { Id = 2, Name = "Film2" };
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    f1,f2
                });
            var mockUsers = new Mock<IIdentityRepository>();
            ApplicationUser user = new ApplicationUser { Id = "1", Blocked = false, UserName = "UserName1" };
            mockUsers.Setup(u => u.Users).Returns(new List<ApplicationUser>()
                {
                    user
                });

            mockUsers.Setup(u => u.FindById("1")).Returns(mockUsers.Object.Users.Find(a => a.Id == "1"));

            AdminController controller = new AdminController(mock.Object, mockUsers.Object);
            controller.Ban("1");
            Assert.AreEqual(true, user.Blocked);

        }

        [TestMethod]
        public void Can_View_Users_Correctly()
        {
            var mock = new Mock<IFilmRepository>();
            Film f1 = new Film { Id = 1, Name = "Film1" };
            Film f2 = new Film { Id = 2, Name = "Film2" };
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    f1,f2
                });
            var mockUsers = new Mock<IIdentityRepository>();
            var role2 = new IdentityRole { Name = "user" };
            var userRole = new IdentityUserRole { RoleId = "1", Role = role2, };

            ApplicationUser user = new ApplicationUser { Id = "1", Blocked = false, UserName = "UserName1" };
            ApplicationUser user1 = new ApplicationUser { Id = "2", Blocked = false, UserName = "UserName2" };
            user.Roles.Add(userRole);
            user1.Roles.Add(userRole);
            mockUsers.Setup(u => u.Users).Returns(new List<ApplicationUser>()
                {
                    user,
                    user1
                });
            var fakeHttpContext = new Mock<HttpContextBase>();
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            fakeHttpContext.Setup(t => t.User).Returns(principal);
            var controllerContext = new Mock<ControllerContext>();
            controllerContext.Setup(t => t.HttpContext).Returns(fakeHttpContext.Object);
            string role = "user";
            mockUsers.Setup(u => u.GetUsersInRole(role)).Returns(mockUsers.Object.Users.Where(u => u.Roles.Any(r => r.Role.Name == role)).ToList());
            AdminController controller = new AdminController(mock.Object, mockUsers.Object);
            controller.ControllerContext = controllerContext.Object;
            ViewResult res = controller.ViewUsers();
            var model = (List<ApplicationUser>)res.Model;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);

        }

        [TestMethod]
        public void CanPromoteToRole()
        {
            var mock = new Mock<IFilmRepository>();
            Film f1 = new Film { Id = 1, Name = "Film1" };
            Film f2 = new Film { Id = 2, Name = "Film2" };
            mock.Setup(m => m.Films).Returns(new List<Film>()
                {
                    f1,f2
                });
            var mockUsers = new Mock<IIdentityRepository>();
            ApplicationUser user = new ApplicationUser { Id = "1", Blocked = false, UserName = "UserName1" };
            mockUsers.Setup(u => u.Users).Returns(new List<ApplicationUser>()
                {
                    user
                });

            mockUsers.Setup(u => u.FindById("1")).Returns(mockUsers.Object.Users.Find(a => a.Id == "1"));
            mockUsers.Setup(u => u.FindRole("admin")).Returns(new IdentityRole { Id = "1", Name = "admin" });
            AdminController controller = new AdminController(mock.Object, mockUsers.Object);
            controller.PromoteToRole("1","admin");
            mockUsers.Verify(a => a.FindById("1"));
            mockUsers.Verify(a=>a.SaveChanges());

        }




    }
}
