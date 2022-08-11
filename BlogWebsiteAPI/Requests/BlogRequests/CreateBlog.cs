using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using BlogWebsiteAPI.Models;
using BlogWebsiteAPI.Services;

namespace BlogWebsiteAPI.Requests.BlogRequest
{
	public class CreateBlog
	{
		public class Request: IRequest<Response>
		{
			public Request()
			{
				RequestContext = new UserInfo.Request();
			}
			public string BlogContent { get; set; }
			public string BlogTitle { get; set; }
			public UserInfo.Request RequestContext { get; set; }
		}

		public class Handler : IRequestHandler<Request, Response>
		{
			private readonly IMediator _mediator;
			private readonly IBlogDataService _blogService;

			public Handler(IMediator mediator, IBlogDataService blogService)
			{
				_mediator = mediator;
				_blogService = blogService;
			}
			public Task<Response> Handle(Request request, CancellationToken cancellationToken)
			{
				UserInfo.Response user = _mediator.Send(request.RequestContext).Result;

				var blogId = _blogService.CreateBlog(request.BlogTitle, user.UserId, request.BlogContent);
				if (blogId == 0) throw new Exception("Blog not posted no Id found.");

				var blog = _blogService.GetBlogById(blogId);
				if (blog == null) throw new Exception("Blog not found");

				return Task.FromResult(new Response(blog));

				throw new NotImplementedException();
			}
		}

		public class Response
		{
			public Response(Blog blogInput)
			{
				blog = blogInput;
			}
			public Blog blog { get; set; }
		}
	}
}
