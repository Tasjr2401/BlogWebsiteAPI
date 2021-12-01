using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebsiteAPI.Controllers
{
	
	[ApiController]
	[Route("LogIn")]
	public class LogInController : Controller
	{
		[EnableCors("BlogClient")]
		[HttpGet]
		[Route("Test")]
		public string Test()
		{
			return "The test worked!";
		}
	}
}
