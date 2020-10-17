using System.Threading;
using System.Threading.Tasks;
using API.Features.ExampleEntities.Queries;
using API.Infrastructure.Exceptions;
using API.UnitTests.Common;
using Shouldly;
using Xunit;

namespace API.UnitTests.Application.Queries.ExampleEntity
{
    public class GetExampleEntityDetailsQueryTests : QueryTestBase
    {
        [Fact]
        public void GetDetails_WithInvalidId_ShouldThrowNotFoundException()
        {
            // Arrange
            var sut = new GetExampleEntityDetailsQueryHandler(this.Context);
            var invalidExampleEntityId = 10;

            // Assert
            Should.Throw<NotFoundException>(async () =>
            {
                // Act
                var result
                    = await sut.Handle(new GetExampleEntityDetailsQuery {EntityId = invalidExampleEntityId}, CancellationToken.None); });
        }

        [Fact]
        public async Task GetDetails_WithValidId_ShouldReturnCorrectExampleEntity()
        {
            // Arrange
            var sut = new GetExampleEntityDetailsQueryHandler(this.Context);
            var validExampleEntityId = 1;

            // Act
            var result
                = await sut.Handle(new GetExampleEntityDetailsQuery {EntityId = validExampleEntityId}, CancellationToken.None);

            // Assert
            result.Id.ShouldBe(validExampleEntityId);

            result.Description.ShouldNotBeNull();
            result.ImageUrl.ShouldNotBeNull();
            result.UserId.ShouldBe("0001");
            result.UserName.ShouldBe("User 1");
        }
    }
}
