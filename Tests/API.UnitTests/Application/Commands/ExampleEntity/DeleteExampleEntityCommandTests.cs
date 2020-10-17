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
    public class DeleteExampleEntityCommandTests : CommandTestBase
    {
        [Fact]
        public async Task DeleteExampleEntity_SetDeletableProperties()
        {
            // Arrange
            var sut = new DeleteExampleEntityCommandHandler(this.Context);
            var cmd = new DeleteExampleEntityCommand
            {
                EntityId = 1,
                UserId = "0001"
            };

            var deletedExampleEntity = this.Context.ExampleEntities.FirstOrDefault(c => c.Id == cmd.EntityId);
            // Act
            await sut.Handle(cmd, CancellationToken.None);

            // Assert
            deletedExampleEntity.ShouldNotBeNull();
            deletedExampleEntity.IsDeleted.ShouldBe(true);
            deletedExampleEntity.DeletedBy.ShouldBe("User 1");
            deletedExampleEntity.DeletedOn.ShouldNotBeNull();
        }

        [Fact]
        public void DeleteExampleEntity_AsInvalidUser_ShouldThrowUserAccessViolation()
        {
            // Arrange
            var sut = new DeleteExampleEntityCommandHandler(this.Context);
            var cmd = new DeleteExampleEntityCommand
            {
                EntityId = 1,
                UserId = "0002"
            };

            // Act & Assert
            Should.Throw<UserAccessViolation>(async () => { await sut.Handle(cmd, CancellationToken.None); });
        }

        [Fact]
        public void DeleteExampleEntity_WithInvalidId_ShouldThrowNotFoundException()
        {
            // Arrange
            var sut = new DeleteExampleEntityCommandHandler(this.Context);
            var cmd = new DeleteExampleEntityCommand
            {
                EntityId = 100,
                UserId = "0002"
            };

            // Act & Assert
            Should.Throw<NotFoundException>(async () => { await sut.Handle(cmd, CancellationToken.None); });
        }
    }
}
