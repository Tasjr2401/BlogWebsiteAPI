using BlogWebsiteAPI.Models;
using BlogWebsiteAPI.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlogWebsiteAPI.Requests.UserRequests
{
    public class RoleUpdate
    {
        public class Request : IRequest<Response>
        {
            public Request()
            {

            }
            public Request(int promoteeId, string newRole)
            {
                PromoteeUserId = promoteeId;
                NewRole = newRole;
            }
            public int PromoteeUserId { get; set; }
            public string NewRole { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly IMediator _mediator;
            private readonly IUserDataService _dataService;

            public Handler(IUserDataService dataService, IMediator mediator)
            {
                _mediator = mediator;
                _dataService = dataService;
            }
            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                UserInfo.Response promoterUserInfo = _mediator.Send(new UserInfo.Request()).Result;
                var promoterUserId = _dataService.GetUserId(promoterUserInfo.Username);
                var currentRole = _dataService.GetUserRole(request.PromoteeUserId);
                if (currentRole == request.NewRole || Enum.IsDefined(typeof(ValidRoles), request.NewRole) == false)
                    return Task.FromResult(new Response(false));

                var result = _dataService.UpdateUserRole(promoterUserId, request.PromoteeUserId, request.NewRole);

                if(result == 0)
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
