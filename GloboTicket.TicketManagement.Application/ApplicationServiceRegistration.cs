using Microsoft.Extensions.DependencyInjection;

namespace GloboTicket.TicketManagement.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        // Register MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        
        return services;
    }
}