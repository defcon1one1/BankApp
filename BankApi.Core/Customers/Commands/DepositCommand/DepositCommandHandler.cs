using BankApp.Core.Models;
using BankApp.Core.Repositories;
using MediatR;

namespace BankApp.Core.Customers.Commands.DepositCommand;

public record DepositCommand(Guid Id, decimal Amount) : IRequest<bool> { }
public class DepositCommandHandler : IRequestHandler<DepositCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    public DepositCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<bool> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetCustomerById(request.Id, CancellationToken.None);
        if (customer is not null)
        {
            await _customerRepository.AddToBalance(request.Id, request.Amount);
            return true;
        }
        return false;
    }
}
