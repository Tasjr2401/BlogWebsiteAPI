using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using BlogWebsiteAPI.Requests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlogWebsiteAPI.Requests.UserRequests;

namespace BlogWebsiteAPI
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddMediatR(typeof(LogIn.Handler).Assembly);

			services.AddHttpContextAccessor();

			services.AddCors(c =>
			{
				c.AddPolicy("BlogClient", options =>
				{
					options.WithOrigins("http://localhost:3000");
					options.AllowAnyHeader();
				});
			});
			services.AddAuthentication(options =>
            {
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				options.RequireAuthenticatedSignIn = false;
            }).AddJwtBearer(options =>
			{
				options.ForwardDefault = JwtBearerDefaults.AuthenticationScheme;
				options.Audience = "http://localhost:3000";
				options.ClaimsIssuer = "https://localhost:44389";
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					RequireSignedTokens = true,
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					//ValidAudience = "http://localhost:3000/",
					ValidIssuer = "https://localhost:44389",
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Token").GetSection("SecurityKey").Value)),
				};
			});
			services.AddAuthorization(options =>
			{
;				options.AddPolicy("AdminRequired", policyOptions => policyOptions.RequireRole("Admin"));
				options.AddPolicy("BasicAccess", policyOptions => policyOptions.RequireRole("Base, Admin"));
            });
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors();

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
