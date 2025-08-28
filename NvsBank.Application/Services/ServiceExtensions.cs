using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
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

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}