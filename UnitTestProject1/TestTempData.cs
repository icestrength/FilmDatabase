using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FilmDatabase.Tests
{
    public class TestTempDataHttpContext : HttpContextBase
    {
        private TestTempDataHttpSessionState _sessionState =
            new TestTempDataHttpSessionState();

        public override HttpSessionStateBase Session
        {
            get
            {
                return _sessionState;
            }
        }
    }

    // HttpSessionState for TempData that uses a custom
    // session object.
    public class TestTempDataHttpSessionState : HttpSessionStateBase
    {
        // This string is "borrowed" from the ASP.NET MVC source code
        private string TempDataSessionStateKey = "__ControllerTempData";
        private object _tempDataObject;

        public override object this[string name]
        {
            get
            {
                Assert.AreEqual<string>(
                    TempDataSessionStateKey,
                    name,
                    "Wrong session key used");
                return _tempDataObject;
            }
            set
            {
                Assert.AreEqual<string>(
                    TempDataSessionStateKey,
                    name,
                    "Wrong session key used");
                _tempDataObject = value;
            }
        }
    }
}