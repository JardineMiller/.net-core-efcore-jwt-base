using System.Threading;
using System.Threading.Tasks;
using API.Data;
using API.Data.Models;
using API.Features.ExampleEntities.Models;
using API.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Features.ExampleEntities.Queries
{
    public class GetExampleEntityDetailsQuery : IRequest<ExampleEntityDetailsResponseModel>
    {
        public int EntityId { get; set; }
    }

    public class
        GetExampleEntityDetailsQueryHandler : IRequestHandler<GetExampleEntityDetailsQuery, ExampleEntityDetailsResponseModel>
    {
        private readonly ApplicationDbContext dbContext;

        public GetExampleEntityDetailsQueryHandler(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ExampleEntityDetailsResponseModel> Handle(GetExampleEntityDetailsQuery request,
            CancellationToken cancellationToken)
        {
            var entity = await this.dbContext
                .ExampleEntities
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == request.EntityId, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ExampleEntity), request.EntityId);
            }

            return new ExampleEntityDetailsResponseModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                ImageUrl = entity.ImageUrl,
                Description = entity.Description,
                UserName = entity.User.UserName
            };
        }
    }
}
