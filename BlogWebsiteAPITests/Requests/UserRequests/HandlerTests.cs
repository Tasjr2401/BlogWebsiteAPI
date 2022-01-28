using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlogWebsiteAPI.Requests.UserRequests.CreateUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MediatR;

namespace BlogWebsiteAPI.Requests.UserRequests.CreateUser.Tests
{
    [TestClass()]
    public class HandlerTests
    {
        [TestMethod()]
        public void CreateUserTest()
        {
            var _mediator = new Mock<IMediator>();

            Assert.Fail();
        }
    }
}