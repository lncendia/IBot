using Telegram.Bot.Types;

namespace IBot.Core.Interfaces.Services.BusinessLogic;

public interface IUpdateHandler
{
    Task HandleAsync(Update update);
}