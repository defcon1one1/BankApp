namespace BankApp.Core.Extensions;
public static class ServiceCollectionExtension
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(ServiceCollectionExtension).Assembly);
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssemblyContaining(typeof(ServiceCollectionExtension), includeInternalTypes: true);
    }
}
