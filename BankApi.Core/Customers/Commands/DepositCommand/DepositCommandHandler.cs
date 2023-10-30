using BankApp.Core.Customers.Events;
using BankApp.Core.Models;
using BankApp.Core.Repositories;
using MediatR;

namespace BankApp.Core.Customers.Commands.DepositCommand;

public record DepositCommand(Guid Id, decimal Amount) : IRequest<bool> { }

public class DepositCommandHandler : IRequestHandler<DepositCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IEventRepository _eventRepository;
    private readonly INotificationService _notificationService;

    public DepositCommandHandler(ICustomerRepository customerRepository, IEventRepository eventRepository, INotificationService notificationService)
    {
        _customerRepository = customerRepository;
        _eventRepository = eventRepository;
        _notificationService = notificationService;
    }

    public async Task<bool> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        IEnumerable<IEvent> events = await _eventRepository.GetAllByAggregateId(request.Id);

        Customer customer = Customer.LoadFromEvents(request.Id, events);

        if (customer.Balance == customer.Balance - request.Amount) // the action will not proceed if another operation was made during handling the request
        {
            return false;
        }

        bool result = customer.Deposit(request.Amount);
        if (result)
        {

            FundsDepositedEvent? depositEvent = customer.GetPendingEvents().OfType<FundsDepositedEvent>().FirstOrDefault();

            if (depositEvent is not null)
            {

                await _eventRepository.Add(depositEvent);
                customer.ClearPendingEvents();

                if (await _customerRepository.AddToBalanceAsync(request.Id, request.Amount)) // ensure database operation
                {
                    await _notificationService.SendOperationNotification($"{DateTime.UtcNow} UTC: user {customer.Id} deposited {request.Amount}");
                    return true;
                }
            }
        }

        return false;
    }
}
