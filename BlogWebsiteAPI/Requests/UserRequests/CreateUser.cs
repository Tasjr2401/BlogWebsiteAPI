using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;
using BlogWebsiteAPI.Models;
using BlogWebsiteAPI.Services;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace BlogWebsiteAPI.Requests.UserRequests
{
    public class CreateUser
    {
        public class Request : IRequest<Response>
        {
            public Request(string username, string password, string firstname, string lastname)
            {
                Username = username;
                Password = password;
                FirstName = firstname;
                LastName = lastname;
            }
            [Required]
            public string Username { get; set; }
            [Required]
            public string Password { get; set; }
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }    

            public bool HasNullProperty()
            {
                if(Username == null || Password == null || FirstName == null || LastName == null)
                    return true;
                else
                    return false;
            }
        
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IUserDataService _dataService;
            private readonly IConfiguration _config;

            public Handler(IConfiguration config, IUserDataService dataService)
            {
                _dataService = dataService;
                _config = config;
            }
            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request == null || request.HasNullProperty())
                {
                    return Task.FromResult(new Response(false));
                }
                var salt = RandomNumberGenerator.GetBytes(12);
                var hashedPassword = UserRequestFunctions.PasswordHash(request.Password, salt);
                var rowsAffected = _dataService.InsertNewUser(request, salt, hashedPassword);
                if (rowsAffected > 0)
                {
                    return Task.FromResult(new Response(true));
                }else
                {
                    return Task.FromResult(new Response(false));
                }
             
            }
        }

        public class Response
        {
            public Response(bool success)
            {
                Success = success;
            }
            public bool Success { get; set; }
        }
    }
}
