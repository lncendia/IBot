using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.CallbackQueryCommands;

public class MyProductsQueryCommand : ICallbackQueryCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, CallbackQuery query,
        ServiceContainer serviceContainer)
    {
        if (user!.State != State.Main)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы должны быть в главное меню.");
            return;
        }

        var page = int.Parse(query.Data![12..]);
        if (page < 1)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Вы в конце списка.");
            return;
        }

        var product = await serviceContainer.UnitOfWork.ProductRepository.Value.GetAsync(user.Products[page - 1]);
        if (product == null)
        {
            await client.AnswerCallbackQueryAsync(query.Id, "Предложение не найдено.");
            return;
        }

        if (query.Message!.Type != MessageType.Photo)
        {
            await client.SendPhotoAsync(query.From.Id, new InputOnlineFile(product.PreviewId),
                $"<b>Имя:</b> <code>{product.Name}</code>", ParseMode.Html,
                replyMarkup: ProductKeyboard.GetProduct(product, page, page <= user.Products.Count));
        }
        else
        {
            var r1 = client.EditMessageCaptionAsync(query.From.Id, query.Message.MessageId,
                $"<b>Имя:</b> <code>{product.Name}</code>", ParseMode.Html,
                replyMarkup: ProductKeyboard.GetProduct(product, page, page <= user.Products.Count));
            var r2 = client.EditMessageMediaAsync(query.From.Id, query.Message.MessageId,
                new InputMediaPhoto(new InputMedia(product.PreviewId)));
            await Task.WhenAll(r1, r2);
        }
    }

    public bool Compare(CallbackQuery query, User? user) => query.Data!.StartsWith("myProducts_");
}