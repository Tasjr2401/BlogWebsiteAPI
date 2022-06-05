using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlogWebsiteAPI.Requests.UserRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BlogWebsiteAPI.Services;
using MediatR;
using System.Threading;

namespace BlogWebsiteAPI.Requests.UserRequests.RoleUpdateTests
{
    [TestClass()]
    public class UpdateRoleHandlerTests
    {
        Mock<IUserDataService> _dataService;
        Mock<IMediator> _mediator;
        public UpdateRoleHandlerTests()
        {
            _dataService = new Mock<IUserDataService>();
            _mediator = new Mock<IMediator>();
        }
        [TestMethod()]
        public void RoleUpdate_SuccessTest()
        {
            //Arrange
            var handler = new RoleUpdate.Handler(_dataService.Object, _mediator.Object);
            var request = new RoleUpdate.Request(It.IsAny<int>(), "Admin");
            var response = new UserInfo.Response("KermitKing", "KazraiTheRat123", "Admin");
            _mediator.Setup(m => m.Send(It.IsAny<UserInfo.Request>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response))
                .Verifiable();
            _dataService.Setup(x => x.GetUserId(It.IsAny<string>()))
                .Returns(It.IsAny<int>())
                .Verifiable();
            _dataService.Setup(x => x.GetUserRole(It.IsAny<int>()))
                .Returns("Base")
                .Verifiable();
            _dataService.Setup(x => x.UpdateUserRole(It.IsAny<int>(), It.IsAny<int>(), "Admin"))
                .Returns(1)
                .Verifiable();
            //Act
            var result = handler.Handle(request, It.IsAny<CancellationToken>());
            //Assert
            Assert.IsTrue(result.Result.Success);
            //Verify
            _mediator.Verify(m => m.Send(It.IsAny<UserInfo.Request>(), It.IsAny<CancellationToken>()), Times.Once);
            _dataService.Verify(x => x.GetUserId(It.IsAny<string>()), Times.Once);
            _dataService.Verify(x => x.GetUserRole(It.IsAny<int>()), Times.Once);
            _dataService.Verify(x => x.UpdateUserRole(It.IsAny<int>(), It.IsAny<int>(), "Admin"), Times.Once);
        }
        [TestMethod()]
        public void RoleUpdate_SameRoleTest()
        {
            //Arrange
            var handler = new RoleUpdate.Handler(_dataService.Object, _mediator.Object);
            var request = new RoleUpdate.Request(It.IsAny<int>(), "Base");
            var response = new UserInfo.Response("KermitKing", "KazraiTheRat123", "Admin");
            _mediator.Setup(m => m.Send(It.IsAny<UserInfo.Request>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response))
                .Verifiable();
            _dataService.Setup(x => x.GetUserId(It.IsAny<string>()))
                .Returns(It.IsAny<int>())
                .Verifiable();
            _dataService.Setup(x => x.GetUserRole(It.IsAny<int>()))
                .Returns("Base")
                .Verifiable();
            _dataService.Setup(x => x.UpdateUserRole(It.IsAny<int>(), It.IsAny<int>(), "Admin"))
                .Returns(1)
                .Verifiable();
            //Act
            var result = handler.Handle(request, It.IsAny<CancellationToken>());
            //Assert
            Assert.IsTrue(result.Result.Success == false);
            //Verify
            _mediator.Verify(m => m.Send(It.IsAny<UserInfo.Request>(), It.IsAny<CancellationToken>()), Times.Once);
            _dataService.Verify(x => x.GetUserId(It.IsAny<string>()), Times.Once);
            _dataService.Verify(x => x.GetUserRole(It.IsAny<int>()), Times.Once);
            _dataService.Verify(x => x.UpdateUserRole(It.IsAny<int>(), It.IsAny<int>(), "Admin"), Times.Never);
        }
        [TestMethod()]
        public void RoleUpdate_NoRowsAddedTest()
        {
            //Arrange
            var handler = new RoleUpdate.Handler(_dataService.Object, _mediator.Object);
            var request = new RoleUpdate.Request(It.IsAny<int>(), "Admin");
            var response = new UserInfo.Response("KermitKing", "KazraiTheRat123", "Admin");
            _mediator.Setup(m => m.Send(It.IsAny<UserInfo.Request>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(response))
                .Verifiable();
            _dataService.Setup(x => x.GetUserId(It.IsAny<string>()))
                .Returns(It.IsAny<int>())
                .Verifiable();
            _dataService.Setup(x => x.GetUserRole(It.IsAny<int>()))
                .Returns("Base")
                .Verifiable();
            _dataService.Setup(x => x.UpdateUserRole(It.IsAny<int>(), It.IsAny<int>(), "Admin"))
                .Returns(0)
                .Verifiable();
            //Act
            var result = handler.Handle(request, It.IsAny<CancellationToken>());
            //Assert
            Assert.IsTrue(result.Result.Success == false);
            //Verify
            _mediator.Verify(m => m.Send(It.IsAny<UserInfo.Request>(), It.IsAny<CancellationToken>()), Times.Once);
            _dataService.Verify(x => x.GetUserId(It.IsAny<string>()), Times.Once);
            _dataService.Verify(x => x.GetUserRole(It.IsAny<int>()), Times.Once);
            _dataService.Verify(x => x.UpdateUserRole(It.IsAny<int>(), It.IsAny<int>(), "Admin"), Times.Once);
        }
    }
}