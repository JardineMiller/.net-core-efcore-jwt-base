using System.Threading;
using System.Threading.Tasks;
using API.Data;
using API.Data.Models;
using API.Infrastructure.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static API.Data.ValidationConstants.ExampleEntity;

namespace API.Features.ExampleEntities.Commands
{
    /// <summary>
    /// Param container passed to <see cref="UpdateExampleEntityCommandHandler"/> for execution
    /// </summary>
    public class UpdateExampleEntityCommand : IRequest
    {
        public string UserId { get; set; }
        public int EntityId { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// The command handler for <see cref="UpdateExampleEntityCommand"/>.
    /// Updates a <see cref="ExampleEntity"/>
    /// </summary>
    public class UpdateExampleEntityCommandHandler : IRequestHandler<UpdateExampleEntityCommand>
    {
        private readonly ApplicationDbContext context;

        public UpdateExampleEntityCommandHandler(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <exception cref="NotFoundException">Thrown when an entity cannot be found with the provided id</exception>
        /// <exception cref="UserAccessViolation">Thrown when an entity cannot be changed by the provided user</exception>
        public async Task<Unit> Handle(UpdateExampleEntityCommand request, CancellationToken cancellationToken)
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

            entity.Description = request.Description;
            await this.context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

    /// <summary>
    /// Validates the properties within <see cref="UpdateExampleEntityCommand"/> to ensure that
    /// a <see cref="ExampleEntity"/> can be safely updated
    /// </summary>
    public class UpdateExampleEntityCommandValidator : AbstractValidator<UpdateExampleEntityCommand>
    {
        public UpdateExampleEntityCommandValidator()
        {
            RuleFor(c => c.Description)
                .NotNull()
                .NotEmpty()
                .MaximumLength(MaxDescriptionLength);

            RuleFor(c => c.EntityId)
                .NotNull()
                .GreaterThanOrEqualTo(1);

            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotNull();
        }
    }
}
