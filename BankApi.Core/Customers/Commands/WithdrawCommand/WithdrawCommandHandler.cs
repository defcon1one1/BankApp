using BankApp.Core.Models;
using BankApp.Core.Repositories;
using MediatR;

namespace BankApp.Core.Customers.Commands.WithdrawCommand;

public record WithdrawCommand(Guid Id, decimal Amount) : IRequest<bool> { }
internal class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    public WithdrawCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<bool> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetCustomerById(request.Id, CancellationToken.None);
        if (customer is not null)
        {
            await _customerRepository.DeductFromBalance(request.Id, request.Amount);
            return true;
        }
        return false;
    }
}