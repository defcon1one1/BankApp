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

    public DepositCommandHandler(ICustomerRepository customerRepository, IEventRepository eventRepository)
    {
        _customerRepository = customerRepository;
        _eventRepository = eventRepository;
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
                return true;
            }
        }

        return false;
    }
}