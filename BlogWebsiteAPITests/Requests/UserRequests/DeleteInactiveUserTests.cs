using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlogWebsiteAPI.Requests.UserRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BlogWebsiteAPI.Services;
using System.Threading;

namespace BlogWebsiteAPI.Requests.UserRequests.DeleteInactiveUserTests
{
    [TestClass()]
    public class DeleteInactiveUserTests
    {
        Mock<IUserDataService> _dataService;
        public DeleteInactiveUserTests()
        {
            _dataService = new Mock<IUserDataService>();
        }
        [TestMethod()]
        public void DeleteInactiveUser_SuccessTest()
        {
            //Arrange
            var handler = new DeleteInactiveUser.Handler(_dataService.Object);
            var request = new DeleteInactiveUser.Request(It.IsAny<int>());
            _dataService.Setup(x => x.DeleteUser(request.UserId))
                .Returns(1)
                .Verifiable();
            //Act
            var result = handler.Handle(request, It.IsAny<CancellationToken>()).Result;
            //Assert
            Assert.IsTrue(result.Success == true);
            //Verify
            _dataService.Verify(x => x.DeleteUser(request.UserId), Times.Once);
        }
        [TestMethod()]
        public void DeleteInactiveUser_SqlFailureTest()
        {
            //Arrange
            var handler = new DeleteInactiveUser.Handler(_dataService.Object);
            var request = new DeleteInactiveUser.Request(It.IsAny<int>());
            _dataService.Setup(x => x.DeleteUser(request.UserId))
                .Returns(0)
                .Verifiable();
            //Act
            var result = handler.Handle(request, It.IsAny<CancellationToken>()).Result;
            //Assert
            Assert.IsTrue(result.Success == false);
            //Verify
            _dataService.Verify(x => x.DeleteUser(request.UserId), Times.Once);
        }

    }
}