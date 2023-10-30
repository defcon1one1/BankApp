using BankApp.Core.Dtos;
using BankApp.Core.Hubs;
using BankApp.Core.Models;
using BankApp.Core.Repositories;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace BankApp.Core.Customers.Queries.GetBalanceQuery;
public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto>;
internal class GetBalanceQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IHubContext<NotificationHub> _hubContext;
    public GetBalanceQueryHandler(ICustomerRepository customerRepository, IHubContext<NotificationHub> hubContext)
    {
        _customerRepository = customerRepository;
        _hubContext = hubContext;
    }

    public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        Customer? customer = await _customerRepository.GetByIdAsync(request.Id, cancellationToken);
        if (customer is not null)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"{DateTime.UtcNow} UTC: user {customer.Id} has checked balance", cancellationToken: cancellationToken);
            return CustomerDto.FromCustomer(customer);
        }
        return default!;
    }
}