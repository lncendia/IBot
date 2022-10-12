using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Products;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.TextCommands;

public class EnterProductNameAndAmountCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message, ServiceContainer serviceContainer)
    {
        if (string.IsNullOrEmpty(user!.TempProductPreview) || string.IsNullOrEmpty(user.TempProduct))
        {
            await client.SendTextMessageAsync(message.From!.Id, "Произошла ошибка. Вернитесь в главное меню.",
                replyMarkup: MainKeyboard.Main);
            return;
        }

        var data = message.Text!.Split(':', 2);

        if (data.Length != 2 || decimal.TryParse(data[0], out var amount))
        {
            await client.SendTextMessageAsync(message.From!.Id, "Введите число.", replyMarkup: MainKeyboard.Main);
            return;
        }

        var product = new Product(user.TempProductPreview, user.TempProduct, data[1], amount);
        user.State = State.Main;
        await Task.WhenAll(serviceContainer.UnitOfWork.ProductRepository.Value.AddAsync(product),
            serviceContainer.UnitOfWork.UserRepository.Value.UpdateAsync(user));
        
        await serviceContainer.UnitOfWork.SaveAsync();
        await client.SendTextMessageAsync(message.From!.Id, "Продукт успешно добавлен.");
    }

    public bool Compare(Message message, User? user) =>
        message.Type == MessageType.Text && user!.State == State.EnterNameAndAmountAdmin && user.IsAdmin;
}