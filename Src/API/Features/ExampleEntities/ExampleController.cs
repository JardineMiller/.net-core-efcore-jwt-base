using System.Collections.Generic;
using System.Threading.Tasks;
using API.Features.ExampleEntities.Commands;
using API.Features.ExampleEntities.Models;
using API.Features.ExampleEntities.Queries;
using API.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.ExampleEntities
{
    [Authorize]
    public class ExampleController : ApiController
    {
        private readonly ICurrentUserService currentUserService;

        public ExampleController(ICurrentUserService currentUserService)
        {
            this.currentUserService = currentUserService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExampleEntityListingResponseModel>>> GetCurrentUserExampleEntities()
        {
            var query = new GetUserExampleEntitiesQuery {UserId = this.currentUserService.GetId()};
            var entities = await this.Mediator.Send(query);

            return Ok(entities);
        }

        [HttpGet]
        [Route("{entityId}")]
        public async Task<ActionResult<ExampleEntityDetailsResponseModel>> GetEntityDetails(int entityId)
        {
            var query = new GetExampleEntityDetailsQuery {EntityId = entityId};
            var entity = await this.Mediator.Send(query);

            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateExampleEntityRequestModel model)
        {
            var command = new CreateExampleEntityCommand
            {
                UserId = this.currentUserService.GetId(),
                ImageUrl = model.ImageUrl,
                Description = model.Description
            };

            var id = await this.Mediator.Send(command);
            return Created(nameof(Create), id);
        }

        [HttpPut]
        public async Task<ActionResult> Update(UpdateExampleEntityRequestModel model)
        {
            var command = new UpdateExampleEntityCommand
            {
                EntityId = model.Id,
                UserId = this.currentUserService.GetId(),
                Description = model.Description
            };

            await this.Mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        [Route("{entityId}")]
        public async Task<ActionResult> Delete(int entityId)
        {
            var command = new DeleteExampleEntityCommand
            {
                EntityId = entityId,
                UserId = this.currentUserService.GetId()
            };

            await this.Mediator.Send(command);
            return Ok();
        }
    }
}
