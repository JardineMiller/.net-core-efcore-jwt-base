using System.Threading.Tasks;
using API.Features.Identity.Commands;
using API.Features.Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.Identity
{
    public class IdentityController : ApiController
    {
        [HttpPost]
        [Route(nameof(Register))]
        public async Task<ActionResult> Register(RegisterUserCommand cmd)
        {
            await this.Mediator.Send(cmd);
            return Ok();
        }

        [HttpPost]
        [Route(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginCommand cmd)
        {
            var response = await this.Mediator.Send(cmd);
            return Ok(response);
        }
    }
}
