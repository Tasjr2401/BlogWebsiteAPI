using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlogWebsiteAPI.Requests.UserRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Moq;
using BlogWebsiteAPI.Services;
using System.Threading;

namespace BlogWebsiteAPI.Requests.UserRequests.DeactivateUserTest
{
    [TestClass()]
    public class DeactivateUserTests
    {
        Mock<IMediator> _mediator;
        Mock<IUserDataService> _dataService;
        public DeactivateUserTests()
        {
            _mediator = new Mock<IMediator>();
            _dataService = new Mock<IUserDataService>();
        }
        [TestMethod()]
        public void DeactivateUser_SuccessTest()
        {
            //Arrange
            var request = new DeactivateUser.Request(It.IsAny<int>(), It.Is<string>(x => x.Length < 500));
            var userInfo = new UserInfo.Response("KazraiTheRat123", "KermitKing", "Admin");
            var deactivatorId = 115586;
            var handler = new DeactivateUser.Handler(_mediator.Object, _dataService.Object);
            _mediator.Setup(x => x.Send(It.IsAny<UserInfo.Request>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(userInfo))
                .Verifiable();
            _dataService.Setup(x => x.GetUserId(userInfo.Username))
                .Returns(deactivatorId)
                .Verifiable();
            _dataService.Setup(x => x.DeactivateUser(request.UserIdToDelete, deactivatorId, request.ReasonForDeactivation))
                .Returns(1)
                .Verifiable();
            //Act
            var result = handler.Handle(request, It.IsAny<CancellationToken>());
            //Assert
            Assert.IsTrue(result.Result.Success == true);
            //Verify
            _mediator.Verify(x => x.Send(It.IsAny<UserInfo.Request>(), It.IsAny<CancellationToken>()), Times.Once);
            _dataService.Verify(x => x.GetUserId(userInfo.Username), Times.Once);
            _dataService.Verify(x => x.DeactivateUser(request.UserIdToDelete, deactivatorId, request.ReasonForDeactivation), Times.Once);
        }
        [TestMethod()]
        public void DeactivateUser_UsernameNullTest()
        {
            //Arrange
            var request = new DeactivateUser.Request(It.IsAny<int>(), It.Is<string>(x => x.Length < 500));
            var userInfo = new UserInfo.Response(null, "KermitKing", "Admin");
            var deactivatorId = 0;
            var handler = new DeactivateUser.Handler(_mediator.Object, _dataService.Object);
            _mediator.Setup(x => x.Send(It.IsAny<UserInfo.Request>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(userInfo))
                .Verifiable();
            _dataService.Setup(x => x.GetUserId(userInfo.Username))
                .Returns(deactivatorId)
                .Verifiable();
            _dataService.Setup(x => x.DeactivateUser(request.UserIdToDelete, deactivatorId, request.ReasonForDeactivation))
                .Returns(0)
                .Verifiable();
            //Act
            var result = handler.Handle(request, It.IsAny<CancellationToken>());
            //Assert
            Assert.IsTrue(result.Result.Success == false);
            //Verify
            _mediator.Verify(x => x.Send(It.IsAny<UserInfo.Request>(), It.IsAny<CancellationToken>()), Times.Once);
            _dataService.Verify(x => x.GetUserId(userInfo.Username), Times.Once);
            _dataService.Verify(x => x.DeactivateUser(request.UserIdToDelete, deactivatorId, request.ReasonForDeactivation), Times.Never);
        }
        [TestMethod()]
        public void DeactivateUser_NoRowsAddedTest()
        {
            //Arrange
            var request = new DeactivateUser.Request(It.IsAny<int>(), It.Is<string>(x => x.Length < 500));
            var userInfo = new UserInfo.Response("KazraiTheRat123", "KermitKing", "Admin");
            var deactivatorId = 154654;
            var handler = new DeactivateUser.Handler(_mediator.Object, _dataService.Object);
            _mediator.Setup(x => x.Send(It.IsAny<UserInfo.Request>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(userInfo))
                .Verifiable();
            _dataService.Setup(x => x.GetUserId(userInfo.Username))
                .Returns(deactivatorId)
                .Verifiable();
            _dataService.Setup(x => x.DeactivateUser(request.UserIdToDelete, deactivatorId, request.ReasonForDeactivation))
                .Returns(0)
                .Verifiable();
            //Act
            var result = handler.Handle(request, It.IsAny<CancellationToken>());
            //Assert
            Assert.IsTrue(result.Result.Success == false);
            //Verify
            _mediator.Verify(x => x.Send(It.IsAny<UserInfo.Request>(), It.IsAny<CancellationToken>()), Times.Once);
            _dataService.Verify(x => x.GetUserId(userInfo.Username), Times.Once);
            _dataService.Verify(x => x.DeactivateUser(request.UserIdToDelete, deactivatorId, request.ReasonForDeactivation), Times.Once);
        }
    }
}