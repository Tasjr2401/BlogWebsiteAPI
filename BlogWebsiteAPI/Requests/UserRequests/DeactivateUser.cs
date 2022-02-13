using BlogWebsiteAPI.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlogWebsiteAPI.Requests.UserRequests
{
    public class DeactivateUser
    {
        public class Request : IRequest<Response>
        {
            public Request(int userIdToDelete, string reasonForDeactivation)
            {
                UserIdToDelete = userIdToDelete;
                ReasonForDeactivation = reasonForDeactivation;
            }
            public int UserIdToDelete { get; set; }
            public string ReasonForDeactivation { get; set; }
        }
        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IMediator _mediator;
            private readonly IUserDataService _dataService;

            public Handler(IMediator mediator, IUserDataService dataService)
            {
                _mediator = mediator;
                _dataService = dataService;
            }
            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var deactivatorUsername = _mediator.Send(new UserInfo.Request()).Result;
                var deactivatorId = _dataService.GetUserId(deactivatorUsername.Username);
                if (deactivatorId == 0)
                    return Task.FromResult(new Response(false));

                var result = _dataService.DeactivateUser(request.UserIdToDelete, deactivatorId, request.ReasonForDeactivation);

                if (result == 0)
                    return Task.FromResult(new Response(false));

                return Task.FromResult(new Response(true));
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
