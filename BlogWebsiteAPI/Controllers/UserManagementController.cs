using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using MediatR;
using BlogWebsiteAPI.Requests.UserRequests;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BlogWebsiteAPI.Controllers
{
    [ApiController]
    [Route("UserManagement")]
    [EnableCors("BlogClient")]
    public class UserManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("UpdateRole")]
        public async Task<IActionResult> UpdateUserRole(RoleUpdate.Request request)
        {
            if (request == null)
                return BadRequest("Request was empty.");

            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("DeactivateUser")]
        public async Task<IActionResult> DeactivateUser(DeactivateUser.Request request)
        {
            if (request == null)
                return BadRequest("Request was empty");
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("DeleteInactiveUser")]
        public async Task<IActionResult> DeleteInactiveUser(DeleteInactiveUser.Request request)
        {
            if (request == null)
                return BadRequest("Request was empty");
            return Ok(await _mediator.Send(request));
        }
    }
}
