using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.Net.Http.Headers;

namespace BlogWebsiteAPI.Requests
{
	public class UserInfo
	{
		public class Request : IRequest<Response>
		{

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
			public Task<Response> Handle(Request request, CancellationToken cancellationToken)
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var valdiationParams = new TokenValidationParameters()
                {
					RequireSignedTokens = true,
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidIssuer = "https://localhost:44389",
					ValidAudience = "http://localhost:3000",
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Token").GetSection("SecurityKey").Value)),
				};
				AuthenticationHeaderValue.TryParse(_httpAccessor.HttpContext.Request.Headers[HeaderNames.Authorization], out AuthenticationHeaderValue headerValue);
				var token = headerValue.Parameter;
				SecurityToken outToken;
				var value = tokenHandler.ValidateToken(token, valdiationParams, out outToken);
				return Task.FromResult(new Response(value.FindFirstValue(ClaimTypes.Name), value.FindFirstValue(ClaimTypes.NameIdentifier), value.FindFirstValue(ClaimTypes.Role)));
			}
		}

		public class Response
		{
			public string Name { get; set; }
            public string Username { get; set; }
            public string Role { get; set; }
            public Response(string name, string username, string role)
			{
				Name = name;
				Username = username;
				Role = role;
			}
		}
	}
}
