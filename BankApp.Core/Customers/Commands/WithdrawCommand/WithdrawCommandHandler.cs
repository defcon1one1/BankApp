using BankApp.Core.Customers.Events;
using BankApp.Core.Models;
using BankApp.Core.Repositories;
using MediatR;

namespace BankApp.Core.Customers.Commands.WithdrawCommand;

public record WithdrawCommand(Guid Id, decimal Amount) : IRequest<bool> { }
internal class WithdrawCommandHandler : IRequestHandler<WithdrawCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IEventRepository _eventRepository;
    private readonly INotificationService _notificationService;
    public WithdrawCommandHandler(ICustomerRepository customerRepository, IEventRepository eventRepository, INotificationService notificationService)
    {
        _customerRepository = customerRepository;
        _eventRepository = eventRepository;
        _notificationService = notificationService;
    }

    public async Task<bool> Handle(WithdrawCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<IEvent> events = await _eventRepository.GetAllByAggregateId(request.Id);

        Customer customer = Customer.LoadFromEvents(request.Id, events);

        if (customer.Balance == customer.Balance - request.Amount) // the action will not proceed if another operation was made during handling the request
        {
            return false;
        }

        bool result = customer.Withdraw(request.Amount);

        if (result)
        {
            FundsWithdrawnEvent? withdrawalEvent = customer.GetPendingEvents().OfType<FundsWithdrawnEvent>().FirstOrDefault();

            if (withdrawalEvent != null)
            {
                await _eventRepository.Add(withdrawalEvent);
                customer.ClearPendingEvents();
                if (await _customerRepository.DeductFromBalanceAsync(request.Id, request.Amount))
                {
                    await _notificationService.SendOperationNotification($"{DateTime.UtcNow} UTC: user {customer.Id} withdrawn {request.Amount}");
                    return true;
                }
            }
        }

        return false;
    }
}