using FluentValidation;

namespace BankApp.Core.Customers.Commands.DepositCommand;
internal class DepositCommandHandlerValidator : AbstractValidator<DepositCommand>
{
    public DepositCommandHandlerValidator()
    {
        RuleFor(c => c.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
    }
}
