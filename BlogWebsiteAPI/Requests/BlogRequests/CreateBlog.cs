using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using BlogWebsiteAPI.Models;

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

			public Handler(IMediator mediator)
			{
				_mediator = mediator;
			}
			public Task<Response> Handle(Request request, CancellationToken cancellationToken)
			{
				UserInfo.Response user = _mediator.Send(request.RequestContext).Result;

				Blog blog = new Blog(request.BlogTitle, user.Name, request.BlogContent, DateTime.Now);

				//make a dataservice reference to post to sql server

				throw new NotImplementedException();
			}
		}

		public class Response
		{
			public int BlogId { get; set; }
		}
	}
}
