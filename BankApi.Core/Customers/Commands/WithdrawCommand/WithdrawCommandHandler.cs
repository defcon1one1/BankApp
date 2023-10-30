using BankApp.Core.Customers.Events;
using BankApp.Core.Models;
using BankApp.Core.Notifications;
using BankApp.Core.Repositories;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace BankApp.Core.Customers.Commands.WithdrawCommand;

public record WithdrawCommand(Guid Id, decimal Amount) : IRequest<bool> { }
internal class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IEventRepository _eventRepository;
    private readonly IHubContext<NotificationHub> _hubContext;
    public WithdrawCommandHandler(ICustomerRepository customerRepository, IEventRepository eventRepository, IHubContext<NotificationHub> hubContext)
    {
        _customerRepository = customerRepository;
        _eventRepository = eventRepository;
        _hubContext = hubContext;
    }

    public async Task<bool> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<IEvent> events = await _eventRepository.GetAllByAggregateId(request.Id);

        Customer customer = Customer.LoadFromEvents(request.Id, events);

        customer.Withdraw(request.Amount);
        var withdrawalEvent = customer.GetPendingEvents().OfType<FundsWithdrawnEvent>().FirstOrDefault();

        if (withdrawalEvent != null)
        {
            await _eventRepository.Add(withdrawalEvent);
            customer.ClearPendingEvents();
            if (await _customerRepository.DeductFromBalance(request.Id, request.Amount))
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"{DateTime.UtcNow} UTC: user {customer.Id} has made a withdrawal of {request.Amount}", cancellationToken: cancellationToken);
                return true;
            }
        }

        return false;
    }
}