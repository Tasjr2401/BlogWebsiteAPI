using BlogWebsiteAPI.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlogWebsiteAPI.Requests.UserRequests
{
    public class GiveAdminPermissions
    {
        public class Request : IRequest<Response>
        {
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
            }
        }

        public class Response
        {

        }
    }
}
