using FluentValidation;

namespace Application.Commands.Chat.Create;

public class CreateGroupCommandValidator : AbstractValidator<CreateGroupCommand>
{
    public CreateGroupCommandValidator()
    {
        RuleFor(command => command.Name).NotEmpty();
    }
}