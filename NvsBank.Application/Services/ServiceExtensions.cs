using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NvsBank.Application.Shared.Behavior;
using NvsBank.Application.UseCases.Employee.Commands.CreateCustomer;

namespace NvsBank.Application.Services;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureApplicationApp(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(ServiceExtensions).Assembly);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(CreateCustomerHandler).Assembly
        ));
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

}