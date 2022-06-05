using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlogWebsiteAPI.Requests.UserRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BlogWebsiteAPI.Services;
using Microsoft.Extensions.Configuration;
using BlogWebsiteAPI.Models;
using System.Security.Cryptography;
using System.Threading;

namespace BlogWebsiteAPI.Requests.UserRequests.LogInUserTests
{
    [TestClass()]
    public class LogInUserHandlerTests
    {
        Mock<IConfiguration> _config;
        Mock<IUserDataService> _dataService;
        public LogInUserHandlerTests()
        {
            _config = new Mock<IConfiguration>();
            _dataService = new Mock<IUserDataService>();
        }
        [TestMethod()]
        public void LogInUser_ValidLogInTest()
        {
            //Arrange
            var password = "KermitKing";
            LogIn.Request request = new LogIn.Request("KazraiTheRat123!", password);
            LogIn.Handler handler = new LogIn.Handler(_config.Object, _dataService.Object);
            var salt = RandomNumberGenerator.GetBytes(12);
            var hashedPassword = UserRequestFunctions.PasswordHash(password, salt);
            var passCheckModel = new UserPasswordCheckModel(hashedPassword, salt, 1);
            _dataService.Setup(x => x.GetPasswordVerificationRequirements(It.IsAny<string>()))
                .Returns(passCheckModel)
                .Verifiable();
            _dataService.Setup(x => x.GetUser(It.IsAny<int>()))
                .Returns(new User("KazraiTheRat123!", "Aviet", "Darbi", "Admin"))
                .Verifiable();
            _config.Setup(c => c.GetSection("Token").GetSection("Issuer").Value)
                .Returns("https://localhost:44389")
                .Verifiable();
            _config.Setup(c => c.GetSection("Token").GetSection("Audience").Value)
                .Returns("http://localhost:3000")
                .Verifiable();
            _config.Setup(c => c.GetSection("Token").GetSection("SecurityKey").Value)
                .Returns("this_should_chamge_maybe_sometime234612")
                .Verifiable();

            //Act
            var result = (LogIn.Response)handler.Handle(request, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsTrue(result.Token != null && result.ErrorMessage == null);

            //Verify
            _dataService.Verify(x => x.GetPasswordVerificationRequirements(It.IsAny<string>()), Times.Once);
            _dataService.Verify(x => x.GetUser(It.IsAny<int>()), Times.Once);

            _config.Verify(c => c.GetSection("Token").GetSection("Issuer").Value, Times.Once);
            _config.Verify(c => c.GetSection("Token").GetSection("Audience").Value, Times.Once);
            _config.Verify(c => c.GetSection("Token").GetSection("SecurityKey").Value, Times.Once);
        }

        [TestMethod()]
        public void LogInUser_UsernameDoesNotExistTest()
        {
            //Arrange
            var password = "KermitKing";
            LogIn.Request request = new LogIn.Request("KazraiTheRat123!", password);
            LogIn.Handler handler = new LogIn.Handler(_config.Object, _dataService.Object);
            var salt = RandomNumberGenerator.GetBytes(12);
            var hashedPassword = UserRequestFunctions.PasswordHash(password, salt);

            _dataService.Setup(x => x.GetPasswordVerificationRequirements(It.IsAny<string>()))
                .Returns(new UserPasswordCheckModel(hashedPassword, salt, 0))
                .Verifiable();
            _dataService.Setup(x => x.GetUser(It.IsAny<int>()))
                .Returns(new User("KazraiTheRat123!", "Aviet", "Darbi", "Admin"))
                .Verifiable();
            _config.Setup(c => c.GetSection("Token").GetSection("Issuer").Value)
                .Returns("https://localhost:44389")
                .Verifiable();
            _config.Setup(c => c.GetSection("Token").GetSection("Audience").Value)
                .Returns("http://localhost:3000")
                .Verifiable();
            _config.Setup(c => c.GetSection("Token").GetSection("SecurityKey").Value)
                .Returns("this_should_chamge_maybe_sometime234612")
                .Verifiable();

            //Act
            var result = (LogIn.Response)handler.Handle(request, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsTrue(result.Token == null && result.ErrorMessage != null);

            //Verify
            _dataService.Verify(x => x.GetPasswordVerificationRequirements(It.IsAny<string>()), Times.Once);
            _dataService.Verify(x => x.GetUser(It.IsAny<int>()), Times.Never);

            _config.Verify(c => c.GetSection("Token").GetSection("Issuer").Value, Times.Never);
            _config.Verify(c => c.GetSection("Token").GetSection("Audience").Value, Times.Never);
            _config.Verify(c => c.GetSection("Token").GetSection("SecurityKey").Value, Times.Never);
        }

        [TestMethod()]
        public void LogInUser_PasswordsDoNotMatchTest()
        {
            //Arrange
            var password = "KermitKing";
            LogIn.Request request = new LogIn.Request("KazraiTheRat123!", password);
            LogIn.Handler handler = new LogIn.Handler(_config.Object, _dataService.Object);
            var salt = RandomNumberGenerator.GetBytes(12);
            var hashedPassword = UserRequestFunctions.PasswordHash("SomeIncorrectUsername", salt);

            _dataService.Setup(x => x.GetPasswordVerificationRequirements(It.IsAny<string>()))
                .Returns(new UserPasswordCheckModel(hashedPassword, salt, 1))
                .Verifiable();
            _dataService.Setup(x => x.GetUser(It.IsAny<int>()))
                .Returns(new User("KazraiTheRat123!", "Aviet", "Darbi", "Admin"))
                .Verifiable();
            _config.Setup(c => c.GetSection("Token").GetSection("Issuer").Value)
                .Returns("https://localhost:44389")
                .Verifiable();
            _config.Setup(c => c.GetSection("Token").GetSection("Audience").Value)
                .Returns("http://localhost:3000")
                .Verifiable();
            _config.Setup(c => c.GetSection("Token").GetSection("SecurityKey").Value)
                .Returns("this_should_chamge_maybe_sometime234612")
                .Verifiable();

            //Act
            var result = (LogIn.Response)handler.Handle(request, It.IsAny<CancellationToken>()).Result;

            //Assert
            Assert.IsTrue(result.Token == null && result.ErrorMessage != null);

            //Verify
            _dataService.Verify(x => x.GetPasswordVerificationRequirements(It.IsAny<string>()), Times.Once);
            _dataService.Verify(x => x.GetUser(It.IsAny<int>()), Times.Never);

            _config.Verify(c => c.GetSection("Token").GetSection("Issuer").Value, Times.Never);
            _config.Verify(c => c.GetSection("Token").GetSection("Audience").Value, Times.Never);
            _config.Verify(c => c.GetSection("Token").GetSection("SecurityKey").Value, Times.Never);
        }
    }
}