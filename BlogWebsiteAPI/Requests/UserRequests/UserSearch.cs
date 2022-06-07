using MediatR;
using System.Threading;
using System.Threading.Tasks;
using BlogWebsiteAPI.Services;
using BlogWebsiteAPI.Models;
using System.Collections.Generic;
using System;

namespace BlogWebsiteAPI.Requests.UserRequests
{
    public class UserSearch
    {
        public class Request : IRequest<Response>
        {
            public string SearchInput { get; set; }
            public Request()
            {

            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
			private readonly IUserDataService _dataService;

			public Handler(IUserDataService dataService)
			{
                _dataService = dataService;
			}
            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if(request == null || request.SearchInput == "")
                    return Task.FromResult(new Response("Request was empty."));

                var results = new List<User>();
				try
				{
                    results = _dataService.SearchUser(request.SearchInput);
				} catch(Exception er)
				{
                    return Task.FromResult(new Response("Server Error: " + er.Message));
				}

                return Task.FromResult(new Response(results));
            }
        }
        public class Response
        {
			public List<User> SearchResults { get; set; }
			public string ErrorMessage { get; set; }
            public Response(List<User> users)
            {
                SearchResults = users;
            }
            public Response(string errorMessage)
            {
                ErrorMessage = errorMessage;
            }
        }
    }
}
