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
        const int perPage = 10;
        var products = await serviceContainer.UnitOfWork.ProductRepository.Value.Find(0, perPage);
        await client.SendTextMessageAsync(message.Chat.Id, "Вот, что у нас есть.",
            replyMarkup: ProductKeyboard.Create(products, 1, products.Count == perPage));
    }

    public bool Compare(Message message, User? user) => message.Type == MessageType.Text &&
                                                        message.Text == "👧 Список" && user!.State == State.Main;
}