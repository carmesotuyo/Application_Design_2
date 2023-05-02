using System;
using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using BlogsApp.WebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace WebApi.Test
{
    [TestClass]
    public class LogControllerTest
    {
        private Mock<ILogLogic> logLogicMock;
        private LogController controller;
        HttpContext httpContext;
        private User user;


        [TestInitialize]
        public void InitTest()
        {
            logLogicMock = new Mock<ILogLogic>(MockBehavior.Strict);
            controller = new LogController(logLogicMock.Object);
            user = new User();

            httpContext = new DefaultHttpContext();
            httpContext.Items["user"] = user;

            ControllerContext controllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
            controller = new LogController(logLogicMock.Object)
            {
                ControllerContext = controllerContext
            };
        }

        //get logs ok
        //get logs unauthorized
    }
}

