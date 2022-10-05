using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using IBot.Core.Entities.Users.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.CallbackQueryCommands;

public class BuyProductQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query,
        ServiceContainer serviceContainer)
    {
        if (user!.State != State.Main)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы должны быть в главное меню.");
            return;
        }
        var id = Guid.Parse(query.Data![5..]);

        var product = await serviceContainer.UnitOfWork.ProductRepository.Value.GetAsync(id);
        if (product == null)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Предложение не найдено.");
            return;
        }

        try
        {
            user.BuyProduct(product);
        }
        catch (NotEnoughMoneyException)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "На вашем счету недостаточно средств.");
            return;
        }

        await serviceContainer.UnitOfWork.UserRepository.Value.UpdateAsync(user);
        await serviceContainer.UnitOfWork.SaveAsync();
        
        await client.AnswerCallbackQueryAsync(query.Id, "Успешно.");
        await client.SendDocumentAsync(query.From.Id, new InputOnlineFile(product.DataId));
    }

    public bool Compare(CallbackQuery query, User? user)
    {
        return query.Data!.StartsWith("buy_");
    }
}