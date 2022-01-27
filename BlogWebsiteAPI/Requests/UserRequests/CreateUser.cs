using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using BlogWebsiteAPI.Models;
using BlogWebsiteAPI.Services;

namespace BlogWebsiteAPI.Requests.UserRequests
{
    public class CreateUser
    {
        public class Request : IRequest<Response>
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }        }

        public class Handler : IRequestHandler<Request, Response>
        {
            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = new User(request.Username, request.FirstName, request.LastName);
                var salt = RandomNumberGenerator.GetBytes(12);
                var hashedPassword = UserRequestFunctions.PasswordHash(request.Password, salt);
                //sql create User
                //sql retreive user Id
                //sql create UserLogIn with password and salt
                return Task.FromResult(new Response());
            }
        }

        public class Response
        {

        }
    }
}
