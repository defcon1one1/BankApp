using BankApp.Core.Models;
using BankApp.Core.Repositories;
using MediatR;

namespace BankApp.Core.Customers.Queries.GetBalanceQuery;
public record GetBalanceQuery(Guid Id) : IRequest<decimal>;
internal class GetBalanceQueryHandler : IRequestHandler<GetBalanceQuery, decimal>
{
    private readonly ICustomerRepository _customerRepository;
    public GetBalanceQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<decimal> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetCustomerById(request.Id, cancellationToken);
        if (customer is not null)
        {
            return customer.Balance;
        }
        return default;
    }
}