using BankApp.Core.Models;
using BankApp.Core.Repositories;
using FluentValidation;

namespace BankApp.Core.Customers.Commands.WithdrawCommand;

internal class WithdrawCommandHandlerValidator : AbstractValidator<WithdrawCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public WithdrawCommandHandlerValidator(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;

        RuleFor(c => c.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(c => c)
            .MustAsync(async (command, cancellationToken) => await ValidateWithdrawalAmount(command))
            .WithMessage("Withdrawal amount exceeds the customer's balance.");
    }

    private async Task<bool> ValidateWithdrawalAmount(WithdrawCommand command)
    {
        Customer? customer = await _customerRepository.GetCustomerById(command.Id, CancellationToken.None);
        if (customer is not null && command.Amount < customer.Balance)
        {
            return true;
        }

        return false;
    }
}