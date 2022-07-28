using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogWebsiteAPI.Controllers
{
	public class BlogController : Controller
	{
		// GET: BlogController
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[Route("PostBlog")]
		public Task<IActionResult> PostNewBlog(CreateBlog.Request)
		{
			return Ok();
		}
	}
}
