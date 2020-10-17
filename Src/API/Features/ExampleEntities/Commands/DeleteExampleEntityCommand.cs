using System.Threading;
using System.Threading.Tasks;
using API.Data;
using API.Data.Models;
using API.Infrastructure.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace API.Features.ExampleEntities.Commands
{
    /// <summary>
    ///     Param container that is executed by <see cref="DeleteExampleEntityCommandHandler" />
    /// </summary>
    public class DeleteExampleEntityCommand : IRequest
    {
        public int EntityId { get; set; }
        public string UserId { get; set; }
    }

    /// <summary>
    ///     The command handler for <see cref="DeleteExampleEntityCommand" />.
    ///     Deletes a <see cref="ExampleEntity" />
    /// </summary>
    public class DeleteExampleEntityCommandHandler : IRequestHandler<DeleteExampleEntityCommand>
    {
        private readonly ApplicationDbContext context;

        public DeleteExampleEntityCommandHandler(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <exception cref="NotFoundException">Thrown when an entity cannot be found with the provided id</exception>
        /// <exception cref="UserAccessViolation">Thrown when an entity cannot be changed by the provided user</exception>
        public async Task<Unit> Handle(DeleteExampleEntityCommand request, CancellationToken cancellationToken)
        {
            var entity = await this.context
                .ExampleEntities
                .FirstOrDefaultAsync(c => c.Id == request.EntityId, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ExampleEntity), request.EntityId);
            }

            if (entity.UserId != request.UserId)
            {
                throw new UserAccessViolation(nameof(ExampleEntity), request.UserId);
            }

            this.context.Remove(entity);
            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

    /// <summary>
    ///     Validates the properties within <see cref="DeleteExampleEntityCommand" /> to ensure
    ///     that a <see cref="ExampleEntity" /> can be safely deleted
    /// </summary>
    public class DeleteExampleEntityCommandValidator : AbstractValidator<DeleteExampleEntityCommand>
    {
        public DeleteExampleEntityCommandValidator()
        {
            RuleFor(c => c.EntityId)
                .NotNull()
                .GreaterThanOrEqualTo(1);

            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotNull();
        }
    }
}
