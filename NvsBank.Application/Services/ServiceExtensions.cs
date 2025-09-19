using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NvsBank.Application.Shared.Behavior;
using NvsBank.Application.UseCases.Customer.Commands;
using NvsBank.Infrastructure.Database;

namespace NvsBank.Application.Services;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureApplicationApp(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(ServiceExtensions).Assembly);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(CreateCustomer).Assembly
        ));
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        

        return services;
    }

}