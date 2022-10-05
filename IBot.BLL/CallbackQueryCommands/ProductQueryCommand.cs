using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.CallbackQueryCommands;

public class ProductQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query,
        ServiceContainer serviceContainer)
    {
        if (user!.State != State.Main)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы должны быть в главное меню.");
            return;
        }

        var id = Guid.Parse(query.Data![9..]);

        var product = await serviceContainer.UnitOfWork.ProductRepository.Value.GetAsync(id);
        if (product == null)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Предложение не найдено.");
            return;
        }

        await client.SendPhotoAsync(query.From.Id, new InputOnlineFile(product.PreviewId),
            $"<b>Имя:</b> <code>{product.Name}</code>\n</b>Цена:</b> <code>{product.Cost}</code> руб.",
            ParseMode.Html, replyMarkup: ProductKeyboard.BuyProduct(product));
    }

    public bool Compare(CallbackQuery query, User? user) => query.Data!.StartsWith("product_");
}