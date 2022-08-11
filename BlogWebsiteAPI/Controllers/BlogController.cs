using BlogWebsiteAPI.Requests.BlogRequest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogWebsiteAPI.Controllers
{
	[ApiController]
	[Route("Blog")]
	[EnableCors("BlogClient")]
	public class BlogController : Controller
	{
		private readonly IMediator _mediator;

		public BlogController(IMediator mediator)
		{
			_mediator = mediator;
		}

		// GET: BlogController
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[Authorize]
		[Route("PostBlog")]
		public async Task<IActionResult> PostNewBlog(CreateBlog.Request request)
		{
			if (request == null) return BadRequest("Null Request");
			return Ok(await _mediator.Send(request));
		}

		[HttpGet]
		[Authorize]
		[Route("GetBlogByTitle")]
		public async Task<IActionResult> SearchBlogByTitle(GetBlogByTitle.Request request)
		{
			if (request == null) return BadRequest("Null Request");
			return Ok(await _mediator.Send(request);
		}
	}
}
