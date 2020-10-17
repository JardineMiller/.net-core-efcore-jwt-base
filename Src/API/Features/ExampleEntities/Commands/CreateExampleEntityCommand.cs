using System.Threading;
using System.Threading.Tasks;
using API.Data;
using API.Data.Models;
using FluentValidation;
using MediatR;
using static API.Data.ValidationConstants.ExampleEntity;

namespace API.Features.ExampleEntities.Commands
{
    /// <summary>
    ///     Param container for the <see cref="CreateExampleEntityCommandHandler" />
    /// </summary>
    public class CreateExampleEntityCommand : IRequest<int>
    {
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
    }

    /// <summary>
    ///     The command handler for the <see cref="CreateExampleEntityCommand" />.
    ///     Creates a <see cref="ExampleEntity" />
    /// </summary>
    public class CreateExampleEntityCommandHandler : IRequestHandler<CreateExampleEntityCommand, int>
    {
        private readonly ApplicationDbContext context;

        public CreateExampleEntityCommandHandler(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> Handle(CreateExampleEntityCommand request, CancellationToken cancellationToken)
        {
            var entity = new ExampleEntity()
            {
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                UserId = request.UserId
            };

            this.context.Add(entity);
            await this.context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }

    /// <summary>
    ///     Validates the properties within the <see cref="CreateExampleEntityCommand" /> to ensure
    ///     we can successfully create a <see cref="ExampleEntity" />
    /// </summary>
    public class CreateExampleEntityCommandValidator : AbstractValidator<CreateExampleEntityCommand>
    {
        public CreateExampleEntityCommandValidator()
        {
            RuleFor(c => c.Description)
                .NotNull()
                .NotEmpty()
                .MaximumLength(MaxDescriptionLength);

            RuleFor(c => c.ImageUrl)
                .NotNull()
                .NotEmpty();

            RuleFor(c => c.UserId)
                .NotEmpty()
                .NotNull();
        }
    }
}
