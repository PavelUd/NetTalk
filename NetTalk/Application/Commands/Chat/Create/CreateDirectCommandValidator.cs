using FluentValidation;

namespace Application.Commands.Chat.Create;

public class CreateDirectCommandValidator : AbstractValidator<CreateDirectCommand>
{
    public CreateDirectCommandValidator()
    {
        RuleFor(command => command.IdOtherUser).NotEmpty();
    }
}