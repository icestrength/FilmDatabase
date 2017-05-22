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
using System.Web.Routing;
using System.ComponentModel.DataAnnotations;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
namespace UnitTestProject3
{
    [TestClass]
    public class AccountControllerTests
    {


        [TestMethod]
        public void Registration_ValidModel()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>());
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            var controller = new AccountController(userManager);

            controller.SetFakeControllerContext("~/Somewhere/");
            var mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(am => am.SignOut());
            mockAuthenticationManager.Setup(am => am.SignIn());

            controller.AuthenticationManager = mockAuthenticationManager.Object;
            var model = new RegisterViewModel
                {
                    UserName = "User12",
                    Name = "Carl",
                    Surname = "Mendoza",
                    Email = "mail",
                    City = "Kyjv",
                    Age = 21,
                    Password = "123456",
                    ConfirmPassword = "123456"

                };
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, results, true);
            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);
            // Act


            // Assert
            Assert.AreEqual(0, results.Count);


        }

        [TestMethod]
        public void Registration_InvalidModel()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>());
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            var controller = new AccountController(userManager);
            controller.SetFakeControllerContext("~/Somewhere/");
            var mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(am => am.SignOut());
            mockAuthenticationManager.Setup(am => am.SignIn());


            controller.AuthenticationManager = mockAuthenticationManager.Object;
            var model = new RegisterViewModel
            {

                Password = "123456",
                ConfirmPassword = "1234567"

            };
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, results, true);
            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);
            // Act
            var result =
                controller.Register(model).Result;

            Assert.IsNotNull(result);
            Assert.AreEqual(6, results.Count);
            Assert.AreEqual("Требуется поле User name.", results[0].ErrorMessage);
            Assert.AreEqual("Требуется поле Name.", results[1].ErrorMessage);


        }
        [TestMethod]
        public void Register()
        {
            Mock<UserManager<ApplicationUser>> manager = new Mock<UserManager<ApplicationUser>>(new UserStore<ApplicationUser>());
            ApplicationUser appUser = new ApplicationUser()
            {
                Email = "test@gmail.com",
                Name = "TestName",
                Surname = "TestSurname",
                UserName = "TestUser"
            };
            manager.Setup(m => m.FindByNameAsync(It.IsAny<string>())).Returns(Task.FromResult(appUser));
            manager.Setup(m => m.FindAsync(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(appUser));
            Task.Run(async () =>
            {
                ApplicationUser testUser = null;
                RegisterViewModel model = new RegisterViewModel
                {
                    UserName = "User12",
                    Name = "Carl",
                    Surname = "Mendoza",
                    Email = "mail",
                    City = "Kyjv",
                    Age = 21,
                    Password = "123456",
                    ConfirmPassword = "123456"

                };

                manager.Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Callback<ApplicationUser, string>((value, password) => { testUser = value; });

                var controller = new AccountController(manager.Object);

                try
                {
                    await controller.Register(model);

                    Assert.Fail();
                }
                catch (Exception) { }



                Assert.IsNotNull(testUser);
                Assert.AreEqual(testUser.Name, model.Name);
                Assert.AreEqual(testUser.Surname, model.Surname);
                Assert.AreEqual(testUser.Email, model.Email);
            }).GetAwaiter().GetResult();
        }
        [TestMethod]
        public void EditInfo_InvalidModel()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>());
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            var controller = new AccountController(userManager);
            controller.SetFakeControllerContext("~/Somewhere/");
            var mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(am => am.SignOut());
            mockAuthenticationManager.Setup(am => am.SignIn());


            controller.AuthenticationManager = mockAuthenticationManager.Object;
            var model = new EditUserViewModel
            {
                Name = "1234",
                Surname = "a",
                Email = "dsa"

            };
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, results, true);
            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);
            // Act

            Assert.IsNotNull(results);
            Assert.AreEqual(3, results.Count);
            Assert.AreEqual("Використовуйте лише букви будь-ласка", results[0].ErrorMessage);
            Assert.AreEqual("Прізвище має мати довжину 3 як мінімум", results[1].ErrorMessage);
            Assert.AreEqual("Некоректна Email-адреса", results[2].ErrorMessage);
        }

        [TestMethod]
        public void EditInfo_ValidModel()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>());
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            var controller = new AccountController(userManager);
            controller.SetFakeControllerContext("~/Somewhere/");
            var mockAuthenticationManager = new Mock<IAuthenticationManager>();
            mockAuthenticationManager.Setup(am => am.SignOut());
            mockAuthenticationManager.Setup(am => am.SignIn());


            controller.AuthenticationManager = mockAuthenticationManager.Object;
            var model = new EditUserViewModel
            {
                Name = "abcd",
                Surname = "afda",
                Email = "dsa@mail.eu"

            };
            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, results, true);
            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);
            // Act

            Assert.IsNotNull(results);
            Assert.AreEqual(0, results.Count);

        }






    }
}
