using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace BankApp.Core.Middleware;
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)

    {
        ValidationContext<TRequest> context = new(request);

        List<ValidationFailure> failures = _validators
            .ToAsyncEnumerable()
            .SelectAwait(async validator => await validator.ValidateAsync(context))
            .ToEnumerable()
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .ToList();

        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}