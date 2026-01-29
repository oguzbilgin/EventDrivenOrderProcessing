using EventDriven.OrderProcessing.Application.Common;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EventDriven.OrderProcessing.Application;
public static class ApplicationRegistrer
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ApplicationRegistrer).Assembly));

        services.AddValidatorsFromAssembly(
            typeof(ApplicationRegistrer).Assembly);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}
