using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Data;
using API.Features.ExampleEntities.Models;
using API.Features.ExampleEntities.Queries;
using API.UnitTests.Common;
using Shouldly;
using Xunit;

namespace API.UnitTests.Application.Queries.ExampleEntity
{
    public class GetUserExampleEntitiesQueryHandlerTests
    {
        public GetUserExampleEntitiesQueryHandlerTests()
        {
            var fixture = new QueryTestBase();
            this.context = fixture.Context;
        }

        private readonly ApplicationDbContext context;

        [Fact]
        public async Task GetUserExampleEntities_WithInvalidUserId_ShouldReturnNoExampleEntities()
        {
            var sut = new GetUserExampleEntitiesQueryHandler(this.context);

            // Arrange
            var invalidId = "INVALID";

            // Act
            var result
                = await sut.Handle(new GetUserExampleEntitiesQuery {UserId = invalidId}, CancellationToken.None);

            // Assert
            result.ShouldBeEmpty();
        }

        [Fact]
        public async Task GetUserExampleEntities_WithValidUserId_ShouldReturnUsersExampleEntities()
        {
            var sut = new GetUserExampleEntitiesQueryHandler(this.context);

            // Arrange
            var validUserId = "0001";

            // Act
            var result
                = await sut.Handle(new GetUserExampleEntitiesQuery {UserId = validUserId}, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<List<ExampleEntityListingResponseModel>>();
            result.Count().ShouldBe(2);
        }
    }
}
