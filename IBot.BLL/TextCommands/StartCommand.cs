using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.TextCommands;

public class StartCommand : ITextCommand
{
    // ReSharper disable once RedundantAssignment
    public async Task Execute(ITelegramBotClient client, User? user, Message message,
        ServiceContainer serviceContainer)
    {
        user = new User(message.From!.Id);
        await serviceContainer.UnitOfWork.UserRepository.Value.AddAsync(user);
        await serviceContainer.UnitOfWork.SaveAsync();
        await client.SendStickerAsync(message.From.Id,
            new InputOnlineFile("CAACAgIAAxkBAAEDh2ZhwNXpm0Vikt-5J5yPWTbDPeUwvwAC-BIAAkJOWUoAAXOIe2mqiM0jBA"),
            replyMarkup: MainKeyboard.MainReplyKeyboard);
        await client.SendTextMessageAsync(message.Chat.Id,
            "Привет", replyMarkup: PaymentKeyboard.TopUpAmount);
    }

    public bool Compare(Message message, User? user) => user is null;
}