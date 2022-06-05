using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlogWebsiteAPI.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BlogWebsiteAPI.Requests.UserRequests
{
	public class LogIn
	{
		public class Request : IRequest<Response>
		{
			public Request(string username, string password)
            {
				Username = username;
				Password = password;
            }
			public string Username { get; set; }
			public string Password { get; set; }
		}

		public class Handler : IRequestHandler<Request, Response>
		{
            private readonly IConfiguration _config;
            private readonly IUserDataService _dataService;

            public Handler(IConfiguration config, IUserDataService dataService)
			{
				_config = config;
				_dataService = dataService;
			}
			public Task<Response> Handle(Request request, CancellationToken cancellationToken)
			{
				var passwordCheckData = _dataService.GetPasswordVerificationRequirements(request.Username);
				if (passwordCheckData.UserId <= 0)
					return Task.FromResult(new Response(null, "This Username does not exist"));
				var hashedPswd = UserRequestFunctions.PasswordHash(request.Password, passwordCheckData.Salt);
				if (hashedPswd != passwordCheckData.HashedPassword)
					return Task.FromResult(new Response(null, "Incorrect Password"));

				var user = _dataService.GetUser(passwordCheckData.UserId);

				var claims = new List<Claim>()
				{
					new Claim(ClaimTypes.NameIdentifier, request.Username),
					new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
					new Claim(ClaimTypes.Role, user.Role)
				};

				string issuer = _config.GetSection("Token").GetSection("Issuer").Value;
				string audience = _config.GetSection("Token").GetSection("Audience").Value;
				var signingCredentials = new SigningCredentials(
						new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
						_config.GetSection("Token")
						.GetSection("SecurityKey").Value)), SecurityAlgorithms.HmacSha256);
				var token = new JwtSecurityToken(
					issuer: issuer,
					audience: audience,
					claims: claims,
					expires: DateTime.UtcNow.AddHours(2),
					signingCredentials: signingCredentials
					);
				try
				{
					return Task.FromResult(new Response(new JwtSecurityTokenHandler().WriteToken(token)));
				}
				catch (Exception e)
				{
					throw new Exception(e.Message);
				}
			}
		}

		public class Response
		{
			public string Token { get; set; }
            public string ErrorMessage { get; set; }
			public Response(string token, string errorMessage)
            {
				Token = token;
				ErrorMessage = errorMessage;
            }
            public Response(string token)
			{
				Token = token;
			}
		}
	}
}
