using BankApp.Core.Customers.Commands.WithdrawCommand;
using BankApp.Core.Models;
using BankApp.Core.Repositories;
using FluentValidation;

internal class WithdrawCommandHandlerValidator : AbstractValidator<WithdrawCommand>
{
    private readonly ICustomerRepository _customerRepository;

    public WithdrawCommandHandlerValidator(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;

        RuleFor(c => c.Id)
            .MustAsync(async (id, cancellationToken) => await _customerRepository.CustomerExists(id))
            .WithMessage("Customer with this ID does not exist.");

        WhenAsync(async (command, cancellationToken) => await _customerRepository.CustomerExists(command.Id), () =>
        {
            RuleFor(c => c.Amount).GreaterThan(0).WithMessage("Withdrawal amount must be greater than 0.");
            RuleFor(c => c)
                .MustAsync(async (command, cancellationToken) => await ValidateWithdrawalAmount(command))
                .WithMessage("Withdrawal amount exceeds the customer's balance.");
        });
    }


    private async Task<bool> ValidateWithdrawalAmount(WithdrawCommand command)
    {
        // here the customer cannot be null because its existence is validated prior to the amount validation
        Customer? customer = await _customerRepository.GetCustomerById(command.Id, CancellationToken.None);
        if (command.Amount <= customer.Balance)
        {
            return true;
        }

        return false;
    }
}
