using Example.Messages.Commands;
using FluentValidation;

namespace Example.Endpoint.Validators;

public class SendSmsCommandValidator : AbstractValidator<SendSmsCommand>
{
    public SendSmsCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotNull();
        RuleFor(x => x.SmsText).NotNull();
        RuleFor(x => x.TelephoneNumber).NotNull();
    }
}