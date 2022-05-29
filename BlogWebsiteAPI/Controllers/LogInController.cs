using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using BlogWebsiteAPI.Requests;
using Microsoft.AspNetCore.Authorization;
using BlogWebsiteAPI.Requests.UserRequests;

namespace BlogWebsiteAPI.Controllers
{
	
	[ApiController]
	[EnableCors("BlogClient")]
	[Route("LogIn")]
	public class LogInController : Controller
	{
		private readonly IMediator _mediator;

		public LogInController (IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		[Route("Test")]
		public LogIn.Response Test()
		{
			return new LogIn.Response("It worked");
		}

		[HttpPost]
		[Route("Validate")]
		public async Task<IActionResult> LogInUser(LogIn.Request request)
		{
			if (request == null)
				return BadRequest("Username and Password were not provided.");
			return Ok(await _mediator.Send(request));
		}

		[HttpPost]
		[Route("CreateUser")]
		public async Task<IActionResult> CreateUser(CreateUser.Request request)
        {
			return Ok(await _mediator.Send(request));
        }

		[HttpGet]
		[Route("UserInfo")]
		[Authorize]
		public async Task<IActionResult> UserInfo()
		{
			return Ok(await _mediator.Send(new UserInfo.Request()));
		}

		[HttpGet]
		[Route("AdminCheck")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AdminCheck(RoleCheck.Request request)
		{
			if (request == null)
            {
				return BadRequest("No data provided. You are really smart. Try again");
            }

			return Ok(await _mediator.Send(request));
		}
	}
}
