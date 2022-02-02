using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlogWebsiteAPI.Requests.UserRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MediatR;
using System.Data.SqlClient;
using System.Threading;
using Microsoft.Extensions.Configuration;
using BlogWebsiteAPI.Services;
using System.Security.Cryptography;

namespace BlogWebsiteAPI.Requests.UserRequests.Tests
{
    [TestClass()]
    public class CreateUserHandlerTests
    {
        Mock<IConfiguration> _config;
        Mock<IUserDataService> _dataService;

        public CreateUserHandlerTests()
        {
            _config = new Mock<IConfiguration>();
            _dataService = new Mock<IUserDataService>();
        }

        [TestMethod()]
        public void CreateUser_UserWasInsertedTest()
        {
            //Arrange
            CreateUser.Request request = new CreateUser.Request("Kazrai", "KermitKing", "Aviet", "Darbi");
            var handler = new CreateUser.Handler(_config.Object, _dataService.Object);
            _dataService.Setup(x =>
                x.InsertNewUser(It.IsAny<CreateUser.Request>(), It.IsAny<byte[]>(), It.IsAny<string>()))
                .Returns(1).Verifiable();

            //Act
            var result = (CreateUser.Response)handler.Handle(request, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsTrue(result.Success == true);

            //Verify
            _dataService.Verify(x =>
                x.InsertNewUser(It.IsAny<CreateUser.Request>(), It.IsAny<byte[]>(), It.IsAny<string>()));
        }

        [TestMethod()]
        public void CreateUser_NullParametersTest()
        {
            //Arrange
            CreateUser.Request request = null;
            var handler = new CreateUser.Handler(_config.Object, _dataService.Object);
            _dataService.Setup(x =>
                x.InsertNewUser(request, It.IsAny<byte[]>(), It.IsAny<string>()))
                .Returns(0)
                .Verifiable();

            //Act
            var result = (CreateUser.Response)handler.Handle(request, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsTrue(result.Success == false);

            //Verify
            _dataService.Verify(x =>
                x.InsertNewUser(request, It.IsAny<byte[]>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod()]
        public void CreateUser_PartialNullParametersTest()
        {
            //Arrange
            CreateUser.Request request = new CreateUser.Request("Kazrai", "KermitKing", null, "Darbi");
            var handler = new CreateUser.Handler(_config.Object, _dataService.Object);
            _dataService.Setup(x =>
                x.InsertNewUser(request, It.IsAny<byte[]>(), It.IsAny<string>()))
                .Returns(0)
                .Verifiable();

            //Act
            var result = (CreateUser.Response)handler.Handle(request, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsTrue(result.Success == false);

            //Verify
            _dataService.Verify(x =>
                x.InsertNewUser(request, It.IsAny<byte[]>(), It.IsAny<string>()), Times.Never);
        }
    }
}