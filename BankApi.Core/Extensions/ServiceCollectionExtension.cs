using BankApp.Core.Middleware;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BankApp.Core.Extensions;
public static class ServiceCollectionExtension
{
    public static void AddCore(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(ServiceCollectionExtension).Assembly);
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssemblyContaining(typeof(ServiceCollectionExtension), includeInternalTypes: true);
    }
}
