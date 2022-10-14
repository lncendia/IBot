using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using IBot.Core.Entities.Users.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.CallbackQueryCommands;

public class BanQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query,
        ServiceContainer serviceContainer)
    {
        await client.AnswerCallbackQueryAsync(query.Id, "Вы забанены.");
        return;
    }

    public bool Compare(CallbackQuery query, User? user) => user!.IsBanned;
}