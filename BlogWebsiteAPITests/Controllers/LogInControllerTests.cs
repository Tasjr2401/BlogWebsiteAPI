using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlogWebsiteAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Moq;
using BlogWebsiteAPI.Requests;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebsiteAPI.Controllers.Tests
{
    [TestClass()]
    public class LogInControllerTests
    {
        LogInController _controller;
        Mock<IMediator> _mediatorMock;

        public LogInControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();

            _controller = new LogInController(_mediatorMock.Object);
        }

        [TestMethod()]
        public void AdminCheck_CallsMediatorWithValidRequest()
        {
            RoleCheck.Request request = new RoleCheck.Request();
            RoleCheck.Response response = new RoleCheck.Response("Hey joe");

            // Setup the stuff
            _mediatorMock.Setup(m => m.Send(request, It.IsAny<CancellationToken>())).Returns(Task.FromResult(response)).Verifiable();

            var result = (OkObjectResult)_controller.AdminCheck(request).Result;

            Assert.IsTrue(result.StatusCode == 200);
            Assert.IsTrue(result.Value == response);
            _mediatorMock.Verify(m => m.Send(request, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod()]
        public void AdminCheck_CallsMediatorWithInvalidRequest()
        {

            // Setup the stuff
            _mediatorMock.Setup(m => m.Send(null, It.IsAny<CancellationToken>())).Verifiable();

            var result = (BadRequestObjectResult)_controller.AdminCheck(null).Result;

            Assert.IsTrue(result.GetType() == typeof(BadRequestObjectResult));
            _mediatorMock.Verify(m => m.Send(null, It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}