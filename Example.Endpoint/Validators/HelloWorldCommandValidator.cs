using Example.Endpoint.Commands;
using FluentValidation;

namespace Example.Endpoint.Validators;

public class HelloWorldCommandValidator : AbstractValidator<HelloWorldCommand>
{
    public HelloWorldCommandValidator()
    {
        RuleFor(x => x.Message).NotNull();
    }
}