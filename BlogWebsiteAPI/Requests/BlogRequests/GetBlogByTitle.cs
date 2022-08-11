using BlogWebsiteAPI.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlogWebsiteAPI.Requests.BlogRequests
{
	public class GetBlogByTitle
	{
		public class Request : IRequest<Response>
		{
			public Request(string titleSearch)
			{
				TitleSearch = titleSearch;
			}

			public string TitleSearch { get; set; }
		}

		public class Handler : IRequestHandler<Request, Response>
		{
			public Task<Response> Handle(Request request, CancellationToken cancellationToken)
			{
				throw new System.NotImplementedException();
			}
		}

		public class Response
		{
			public Response(Blog blog)
			{
				blogObject = blog;
			}
			public Blog blogObject { get; set; }
		}
	}
}
