using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WebApplication1;
using WebApplication1.Controllers;
using System.Web.Routing;
using System.Web;
using System.Security.Principal;
using WebApplication1.Tests.Model;

namespace WebApplication1.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Index", result.ViewName);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        public class MockHttpContext : HttpContextBase
        {
            private readonly IPrincipal _user = new GenericPrincipal(new GenericIdentity("someUser"), null /* roles */ );

            public override IPrincipal User
            {
                get
                {
                    return _user;
                }
                set
                {
                    base.User = value;
                }
            }
        }


        private static HomeController GetController()
        { 
            // Arrange
            HomeController controller = new HomeController();

            controller.ControllerContext = new ControllerContext()
            {
                Controller = controller,
                RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
            };
            return controller;
        }

        WorkTask GetTask(Guid id, string Name, string Desc, DateTime DueDate, string Status, string Priority)
        {
            return new WorkTask
            {
                Id = id,
                TaskName = Name,
                TaskDescription = Desc,
                TaskDueDate = DueDate,
                TaskStatus = Status,
                TaskPriority = Priority
            };
        }

        [TestMethod]
        public void GetTasks()
        {
            WorkTask task1 = GetTask(Guid.NewGuid(), "TaskTest1", "Test1", DateTime.Now.Date.AddDays(1), "Posted", "Low");
            WorkTask task2 = GetTask(Guid.NewGuid(), "TaskTest2", "Test2", DateTime.Now.Date.AddDays(3), "Processed", "High");

            WorkTasksTests test = new WorkTasksTests();
            test.insert(task1);
            test.insert(task2);

            var controller = GetController();
            var result = controller.Index();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
