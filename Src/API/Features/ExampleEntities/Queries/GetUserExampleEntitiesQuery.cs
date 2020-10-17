using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Data;
using API.Features.ExampleEntities.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Features.ExampleEntities.Queries
{
    public class GetUserExampleEntitiesQuery : IRequest<IEnumerable<ExampleEntityListingResponseModel>>
    {
        public string UserId { get; set; }
    }

    public class GetUserExampleEntitiesQueryHandler : IRequestHandler<GetUserExampleEntitiesQuery, IEnumerable<ExampleEntityListingResponseModel>>
    {
        private readonly ApplicationDbContext dbContext;

        public GetUserExampleEntitiesQueryHandler(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<ExampleEntityListingResponseModel>> Handle(GetUserExampleEntitiesQuery request, CancellationToken cancellationToken)
        {
            return await this.dbContext
                .ExampleEntities
                .Where(c => c.UserId == request.UserId)
                .Select(c => new ExampleEntityListingResponseModel
                {
                    Id = c.Id,
                    ImageUrl = c.ImageUrl,
                    Description = c.Description
                })
                .ToListAsync(cancellationToken: cancellationToken);
        }
    }
}
