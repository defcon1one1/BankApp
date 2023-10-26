using BankApp.Core.Repositories;
using FluentValidation;

namespace BankApp.Core.Customers.Queries.GetBalanceQuery;
internal class GetBalanceQueryValidator : AbstractValidator<GetCustomerByIdQuery>
{
    private readonly ICustomerRepository _customerRepository;
    public GetBalanceQueryValidator(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;

        RuleFor(c => c.Id)
            .MustAsync(async (id, cancellationToken) => await _customerRepository.CustomerExists(id))
            .WithMessage("Customer with this ID does not exist.");
    }

}
