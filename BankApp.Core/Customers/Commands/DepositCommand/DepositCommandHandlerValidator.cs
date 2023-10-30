using BankApp.Core.Repositories;
using FluentValidation;

namespace BankApp.Core.Customers.Commands.DepositCommand;
internal class DepositCommandHandlerValidator : AbstractValidator<DepositCommand>
{
    private readonly ICustomerRepository _customerRepository;
    public DepositCommandHandlerValidator(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;

        RuleFor(c => c.Id)
            .MustAsync(async (id, cancellationToken) => await _customerRepository.CustomerExistsAsync(id))
            .WithMessage("Customer with this ID does not exist.")
            .DependentRules(() =>
            {
                RuleFor(c => c.Amount)
                    .GreaterThan(0).WithMessage("Withdrawal amount must be greater than 0.");
            });
    }
}