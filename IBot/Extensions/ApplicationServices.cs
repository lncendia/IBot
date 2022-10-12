using IBot.BLL.Services;
using IBot.Core.Interfaces.Repositories;
using IBot.Core.Interfaces.Services.BusinessLogic;
using IBot.Core.Interfaces.Services.Infrastructure;
using Telegram.Bot;

namespace IBot.Extensions;

public static class ApplicationServices
{
    public static void AddApplicationServices(this IServiceCollection services, string token, string helpName)
    {
        services.AddHttpClient("tgwebhook").AddTypedClient<ITelegramBotClient>(httpClient
            => new TelegramBotClient(token, httpClient));
        
        services.AddScoped<IUpdateHandler, UpdateHandler>(x =>
            new UpdateHandler(x.GetRequiredService<ITelegramBotClient>(),
                x.GetRequiredService<IPaymentCreationService>(), x.GetRequiredService<ILogger<UpdateHandler>>(),
                x.GetRequiredService<IUnitOfWork>(), helpName));
    }
}