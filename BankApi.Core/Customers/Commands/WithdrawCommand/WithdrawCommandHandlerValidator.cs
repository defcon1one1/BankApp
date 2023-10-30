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
            .MustAsync(async (id, cancellationToken) => await _customerRepository.CustomerExistsAsync(id))
            .WithMessage("Customer with this ID does not exist.")
            .DependentRules(() =>
            {
                RuleFor(c => c.Amount)
                    .NotEmpty().WithMessage("Amount is required.")
                    .MustAsync(BeValidAmountAsync)
                    .WithMessage("Amount must be a valid number and greater than 0.");

                RuleFor(c => c)
                    .MustAsync(async (command, cancellationToken) => await ValidateWithdrawalAmount(command))
                    .WithMessage("Amount exceeds the customer's balance.");
            });
    }

    private async Task<bool> ValidateWithdrawalAmount(WithdrawCommand command)
    {
        Customer? customer = await _customerRepository.GetByIdAsync(command.Id, CancellationToken.None);
        return customer is not null && command.Amount <= customer.Balance;
    }

    private async Task<bool> BeValidAmountAsync(decimal amount, CancellationToken cancellationToken)
    {
        return await Task.FromResult(amount > 0 && decimal.TryParse(amount.ToString(), out _));
    }

}
