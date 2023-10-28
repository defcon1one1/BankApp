using BankApp.Core.Dtos;
using BankApp.Core.Models;
using BankApp.Core.Repositories;
using MediatR;

namespace BankApp.Core.Customers.Queries.GetBalanceQuery;
public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto>;
internal class GetBalanceQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    public GetBalanceQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetById(request.Id, cancellationToken);
        if (customer is not null)
        {
            return CustomerDto.FromCustomer(customer);
        }
        return default!;
    }
}