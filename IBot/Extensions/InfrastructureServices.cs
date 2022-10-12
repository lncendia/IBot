using IBot.Core.Interfaces.Services.Infrastructure;
using IBot.PaymentSystem;

namespace IBot.Extensions;

public static class InfrastructureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, string paymentToken) =>
        services.AddScoped<IPaymentCreationService, PaymentService>(_ =>
            new PaymentService(paymentToken));
}