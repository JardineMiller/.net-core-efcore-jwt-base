using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Features.ExampleEntities.Commands;
using API.Infrastructure.Exceptions;
using API.UnitTests.Common;
using Shouldly;
using Xunit;

namespace API.UnitTests.Application.Commands.ExampleEntity
{
    public class UpdateExampleEntityCommandTests : CommandTestBase
    {
        [Fact]
        public async Task UpdateExampleEntity_WithCorrectOwner_ShouldUpdateExampleEntityDetails()
        {
            // Arrange
            var sut = new UpdateExampleEntityCommandHandler(this.Context);

            // Act
            var cmd = new UpdateExampleEntityCommand
            {
                EntityId = 1,
                Description = "ExampleEntity 1 - Edited",
                UserId = "0001"
            };

            await sut.Handle(cmd, CancellationToken.None);

            var updatedExampleEntity = this.Context.ExampleEntities.FirstOrDefault(c => c.Id == cmd.EntityId);

            // Assert
            updatedExampleEntity.ShouldNotBeNull();
            updatedExampleEntity.Description.ShouldBe(cmd.Description);
        }

        [Fact]
        public async Task UpdateExampleEntity_WithCorrectOwner_ShouldUpdateExampleEntityAuditDetails()
        {
            // Arrange
            var sut = new UpdateExampleEntityCommandHandler(this.Context);

            // Act
            var cmd = new UpdateExampleEntityCommand
            {
                EntityId = 1,
                Description = "ExampleEntity 1 - Edited",
                UserId = "0001"
            };

            await sut.Handle(cmd, CancellationToken.None);

            var updatedExampleEntity = this.Context.ExampleEntities.FirstOrDefault(c => c.Id == cmd.EntityId);

            // Assert
            updatedExampleEntity.ShouldNotBeNull();
            updatedExampleEntity.ModifiedBy.ShouldBe("User 1");
            updatedExampleEntity.ModifiedOn.ShouldNotBeNull();
        }

        [Fact]
        public void UpdateExampleEntity_WithInvalidOwner_ShouldThrowUserAccessException()
        {
            // Arrange
            var sut = new UpdateExampleEntityCommandHandler(this.Context);

            // Act
            var cmd = new UpdateExampleEntityCommand
            {
                EntityId = 3,
                Description = "ExampleEntity 3 - Edited",
                UserId = "0001"
            };

            Should.Throw<UserAccessViolation>(async () => { await sut.Handle(cmd, CancellationToken.None); });
        }

        [Fact]
        public void EntityId_ShouldThrowNotFoundException()
        {
            // Arrange
            var sut = new UpdateExampleEntityCommandHandler(this.Context);

            // Act
            var cmd = new UpdateExampleEntityCommand
            {
                EntityId = 10,
                Description = "ExampleEntity 3 - Edited",
                UserId = "0001"
            };

            Should.Throw<NotFoundException>(async () => { await sut.Handle(cmd, CancellationToken.None); });
        }
    }
}
