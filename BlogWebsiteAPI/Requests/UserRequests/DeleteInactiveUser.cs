using BlogWebsiteAPI.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlogWebsiteAPI.Requests.UserRequests
{
    public class DeleteInactiveUser
    {
        public class Request : IRequest<Response>
        {
            public Request(int userId)
            {
                UserId = userId;
            }
            public int UserId { get; set; }
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
                var result = _dataService.DeleteUser(request.UserId);
                if (result > 0)
                    return Task.FromResult(new Response(true));

                return Task.FromResult(new Response(false));
            }
        }
        public class Response
        {
            public Response(bool success)
            {
                Success = success;
            }
            public bool Success { get; set; }
        }
    }
}
