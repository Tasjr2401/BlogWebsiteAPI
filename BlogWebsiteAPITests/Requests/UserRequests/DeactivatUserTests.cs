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

namespace BlogWebsiteAPI.Requests.UserRequests.DeactivateUser.Tests
{
    [TestClass()]
    public class DeactivatUserTests
    {
        Mock<IMediator> _mediator;
        Mock<IUserDataService> _dataService;
        public DeactivatUserTests()
        {
            _mediator = new Mock<IMediator>();
            _dataService = new Mock<IUserDataService>();
        }
        [TestMethod()]
        public void DeactivateUser_SuccessTest()
        {
            //Arrange

            //Act
            //Assert
            //Verify
        }
    }
}