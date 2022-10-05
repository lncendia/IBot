using IBot.BLL.Interfaces;
using IBot.Core.Entities.Users.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.TextCommands;

public class EnterMessageToMailingCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message,
        ServiceContainer serviceContainer)
    {
        var data = message.Text!.Split(':', 2);
        int page = 1;
        if (data.Length != 2 || !int.TryParse(data[0], out page))
        {
            await client.SendTextMessageAsync(message.From!.Id,
                $"Неверный формат команды. Правильный формат: <code>1:Текст рассылки</code>.", ParseMode.Html);
        }

        var users = await serviceContainer.UnitOfWork.UserRepository.Value.Find((page-1)*500, 500);
        IEnumerable<Task<Message>> tasks = new List<Task<Message>>();
        switch (message.Type)
        {
            case MessageType.Text:
                tasks = users.Select(user1 =>
                    client.SendTextMessageAsync(user1.TelegramId, message.Text!, ParseMode.Html));
                break;
            case MessageType.Photo:
                tasks = users.Select(user1 => client.SendPhotoAsync(user1.TelegramId,
                    new InputMedia(message.Photo!.Last().FileId), message.Caption, ParseMode.Html));
                break;
            case MessageType.Audio:
                tasks = users.Select(user1 =>
                    client.SendAudioAsync(user1.TelegramId, new InputMedia(message.Audio!.FileId), parseMode: ParseMode.Html));
                break;
            case MessageType.Video:
                tasks = users.Select(user1 => client.SendVideoAsync(user1.TelegramId, new InputMedia(message.Video!.FileId),
                    caption: message.Caption, parseMode: ParseMode.Html));
                break;
            case MessageType.Voice:
                tasks = users.Select(user1 =>
                    client.SendVoiceAsync(user1.TelegramId, new InputMedia(message.Voice!.FileId), parseMode: ParseMode.Html));
                break;
            case MessageType.Document:
                tasks = users.Select(user1 => client.SendDocumentAsync(user1.TelegramId,
                    new InputMedia(message.Document!.FileId), caption: message.Caption, parseMode: ParseMode.Html));
                break;
            case MessageType.Sticker:
                tasks = users.Select(user1 =>
                    client.SendStickerAsync(user1.TelegramId, new InputMedia(message.Sticker!.FileId)));
                break;
        }

        var task = Task.WhenAll(tasks);
        try
        {
            await task;
            await client.SendTextMessageAsync(message.From!.Id,
                $"Сообщение было успешно отправлено {users.Count} пользователю(ям). Вы в главном меню.");
        }
        catch (Exception)
        {
            var exceptionsCount = task.Exception?.InnerExceptions.Count ?? 0;
            await client.SendTextMessageAsync(message.From!.Id,
                $"Сообщение было отправлено {users.Count - exceptionsCount} пользователю(ям). У {exceptionsCount} пользователя(ей) возникла ошибка. Вы в главном меню.");
        }

        GC.Collect();
        user!.State = State.Main;
        await serviceContainer.UnitOfWork.UserRepository.Value.UpdateAsync(user);
        await serviceContainer.UnitOfWork.SaveAsync();
    }

    public bool Compare(Message message, User? user) => user!.State == State.MailingAdmin;
}