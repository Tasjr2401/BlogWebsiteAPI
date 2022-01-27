using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlogWebsiteAPI.Requests
{
	public class RoleCheck
	{
		public class Request : IRequest<Response>
		{

		}

		public class Handler : IRequestHandler<Request, Response>
		{
			public Task<Response> Handle(Request request, CancellationToken cancellationToken)
			{
				return Task.FromResult(new Response("It worked Hurahh!"));
			}
		}

		public class Response
		{
			public string Message { get; set; }
			public Response(string message)
			{
				Message = message;
			}
		}
	}
}
