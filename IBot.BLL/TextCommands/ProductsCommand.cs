using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.TextCommands;

public class ProductsCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message,
        ServiceContainer serviceContainer)
    {
        var keyboard = user!.IsAdmin ? ProductKeyboard.ProductsAdmin : ProductKeyboard.Products;
        await client.SendTextMessageAsync(message.Chat.Id, "<b>Выберите, что вы хотите сделать.</b>", ParseMode.Html,
            replyMarkup: keyboard);
    }

    public bool Compare(Message message, User? user) => message.Type == MessageType.Text &&
                                                        message.Text == "👧 Покупки" && user!.State == State.Main;
}