using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Features.ExampleEntities.Commands;
using API.UnitTests.Common;
using Shouldly;
using Xunit;

namespace API.UnitTests.Application.Commands.ExampleEntity
{
    public class CreateExampleEntityCommandTests : CommandTestBase
    {
        [Fact]
        public async Task CreateExampleEntity_WithValidUser_ShouldReturnExampleEntityId()
        {
            // Arrange
            var sut = new CreateExampleEntityCommandHandler(this.Context);

            // Act
            var command = new CreateExampleEntityCommand
            {
                Description = "Test Entity 1",
                ImageUrl = "test-entity-1.jpg",
                UserId = "0001"
            };

            // Assert
            var result = await sut.Handle(command, CancellationToken.None);
            var entity = this.Context
                .ExampleEntities
                .FirstOrDefault(c => c.Id == result);

            entity.ShouldNotBeNull();
            entity.Id.ShouldBe(result);

            entity.ImageUrl.ShouldBe(command.ImageUrl);
            entity.UserId.ShouldBe(command.UserId);
        }

        [Fact]
        public async Task CreateExampleEntity_WithValidUser_ExampleEntityHasCorrectDetails()
        {
            // Arrange
            var sut = new CreateExampleEntityCommandHandler(this.Context);

            // Act
            var command = new CreateExampleEntityCommand
            {
                Description = "Test ExampleEntity 1",
                ImageUrl = "test-entity-1.jpg",
                UserId = "0001"
            };

            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            var entity = this.Context
                .ExampleEntities
                .FirstOrDefault(c => c.Id == result);

            entity.ShouldNotBeNull();
            entity.Id.ShouldBe(result);
            entity.ImageUrl.ShouldBe(command.ImageUrl);
            entity.UserId.ShouldBe(command.UserId);
        }

        [Fact]
        public async Task CreateExampleEntity_WithValidUser_ExampleEntityHasCorrectAuditingDetails()
        {
            // Arrange
            var sut = new CreateExampleEntityCommandHandler(this.Context);

            // Act
            var command = new CreateExampleEntityCommand
            {
                Description = "Test ExampleEntity 1",
                ImageUrl = "test-entity-1.jpg",
                UserId = "0001"
            };

            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            var entity = this.Context
                .ExampleEntities
                .FirstOrDefault(c => c.Id == result);

            entity.ShouldNotBeNull();

            entity.CreatedOn.ShouldNotBeNull();
            entity.CreatedBy.ShouldBe("User 1");

            entity.ModifiedBy.ShouldBeNull();
            entity.ModifiedOn.ShouldBeNull();

            entity.DeletedBy.ShouldBeNull();
            entity.DeletedOn.ShouldBeNull();
        }
    }
}
