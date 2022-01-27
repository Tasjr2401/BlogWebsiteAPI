using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BlogWebsiteAPI.Requests
{
	public class LogIn
	{
		public class Request : IRequest<Response>
		{
			public string Username { get; set; }
			public string Password { get; set; }
		}

		public class Handler : IRequestHandler<Request, Response>
		{
            private readonly IConfiguration _config;
            private readonly IHttpContextAccessor _httpAccessor;

			public Handler(IHttpContextAccessor httpAccessor, IConfiguration config)
			{
				_config = config;
				_httpAccessor = httpAccessor;
			}
			public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
			{
				if(request.Username == "Tim" && request.Password == "Mike")
				{
					var claims = new List<Claim>()
					{
						new Claim(ClaimTypes.NameIdentifier, request.Username),
						new Claim(ClaimTypes.Name, "Tim Stopford"),
						new Claim(ClaimTypes.Role, "Admin")
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
						//var tokenResult = await _httpAccessor.HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "Token");
						return new Response(new JwtSecurityTokenHandler().WriteToken(token));
					} 
					catch(Exception e)
					{
						throw new Exception(e.Message);
					}
				}
				throw new Exception("Incorrect Credentials");
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
