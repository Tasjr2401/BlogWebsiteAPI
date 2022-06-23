using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using MediatR;
using BlogWebsiteAPI.Requests.UserRequests;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using BlogWebsiteAPI.Services;

namespace BlogWebsiteAPI.Controllers
{
    [ApiController]
    [Route("UserManagement")]
    [EnableCors("BlogClient")]
    public class UserManagementController : ControllerBase
    {
        private readonly IMediator _mediator;
		private readonly IKeyVaultManagement _secretManager;

		public UserManagementController(IMediator mediator, IKeyVaultManagement secretManager)
        {
            _mediator = mediator;
            _secretManager = secretManager;
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

        [HttpGet]
        [Authorize]
        [Route("UserSearch")]
        public async Task<IActionResult> SearchForUser(UserSearch.Request request)
        {
            if (request == null)
                return BadRequest("Request was empty");
            return Ok(await _mediator.Send(request));
        }

#if DEBUG
		[HttpGet]
		[Route("TestVaultSecret")]
        public async Task<IActionResult> TestVaultSecret([FromQuery]string secretName)
        {
            if (secretName == null)
                return BadRequest("Request was empty");
            return Ok(await _secretManager.GetSecret(secretName));
        }
#endif
    }
}
