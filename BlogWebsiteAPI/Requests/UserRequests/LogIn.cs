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
				if (!_dataService.UsernameExistsCheck(request.Username))
					throw new Exception("No Existing User");

				var passwordCheckData = _dataService.GetPasswordVerificationRequirements(request.Username);
				var user = _dataService.GetUser(passwordCheckData.UserId);

				if (UserRequestFunctions.PasswordHash(request.Password, passwordCheckData.Salt) != passwordCheckData.HashedPassword)
					throw new Exception("Incorrect Password");

				var claims = new List<Claim>()
				{
					new Claim(ClaimTypes.NameIdentifier, request.Username),
					new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
					new Claim(ClaimTypes.Role, user.Role)
				};

				//var claimsIdentity = new ClaimsIdentity(claims);
				//var principal = new ClaimsPrincipal(claimsIdentity);
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
			public Response(string token)
			{
				Token = token;
			}
		}
	}
}
