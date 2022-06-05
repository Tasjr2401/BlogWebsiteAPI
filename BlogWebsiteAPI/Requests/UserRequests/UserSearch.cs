using MediatR;
using System.Threading;
using System.Threading.Tasks;

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
            public Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {


                throw new System.NotImplementedException();
            }
        }
        public class Response
        {
            public string Username { get; set; }
            public string DisplayName { get; set; }
            public int Id { get; set; }
            public string FullName { get; set; }
        }
    }
}
