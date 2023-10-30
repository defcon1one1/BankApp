using BankApp.Core.Customers.Events;
using BankApp.Core.Models;
using BankApp.Core.Notifications;
using BankApp.Core.Repositories;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace BankApp.Core.Customers.Commands.DepositCommand;

public record DepositCommand(Guid Id, decimal Amount) : IRequest<bool> { }

public class DepositCommandHandler : IRequestHandler<DepositCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IHubContext<NotificationHub> _hubContext;

    public DepositCommandHandler(ICustomerRepository customerRepository, IEventRepository eventRepository, IHubContext<NotificationHub> hubContext)
    {
        _customerRepository = customerRepository;
        _eventRepository = eventRepository;
        _hubContext = hubContext;
    }

    public async Task<bool> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<IEvent> events = await _eventRepository.GetAllByAggregateId(request.Id);

        Customer customer = Customer.LoadFromEvents(request.Id, events);
        customer.Deposit(request.Amount);

        FundsDepositedEvent? depositEvent = customer.GetPendingEvents().OfType<FundsDepositedEvent>().FirstOrDefault();

        if (depositEvent is not null)
        {
            await _eventRepository.Add(depositEvent);
            customer.ClearPendingEvents();

            if (await _customerRepository.AddToBalance(request.Id, request.Amount))
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", (request.Amount < 10000) ? $"{DateTime.UtcNow} UTC: user {customer.Id} has made a deposit of {request.Amount}" : "ciekawe skąd wziął na to pieniążki ;)", cancellationToken: cancellationToken);
                return true;
            }
        }

        return false;
    }
}
