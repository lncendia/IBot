using IBot.BLL.Interfaces;
using IBot.BLL.Keyboards.UserKeyboard;
using IBot.Core.Entities.Users.Enums;
using IBot.Core.Interfaces.Services.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = IBot.Core.Entities.Users.User;

namespace IBot.BLL.TextCommands;

public class EnterAmountCommand : ITextCommand
{
    public async Task Execute(ITelegramBotClient client, User? user, Message message, ServiceContainer serviceContainer)
    {
        if (!int.TryParse(message.Text, out var amount))
        {
            await client.SendTextMessageAsync(message.From!.Id, "Введите число!", replyMarkup: MainKeyboard.Main);
            return;
        }

        if (amount < 100)
        {
            await client.SendTextMessageAsync(message.From!.Id,
                "Слишком маленькая сумма. Минимум 100 рублей.", replyMarkup: MainKeyboard.Main);
            return;
        }

        try
        {
            var payment = await serviceContainer.PaymentCreationService.CreateBillAsync(user!.Id, amount);

            user.State = State.Main;
            await serviceContainer.UnitOfWork.UserRepository.Value.UpdateAsync(user);
            await serviceContainer.UnitOfWork.SaveAsync();
            await client.SendTextMessageAsync(message.From!.Id,
                $"💸 Оплата подписки на сумму {amount}₽.\n📆 Дата: {DateTime.Now:dd.MMM.yyyy}\n❌ Статус: Не оплачено.\n\n💳 Оплатите счет.",
                replyMarkup: PaymentKeyboard.CheckBill(payment));
        }
        catch (BillCreationException)
        {
            await client.SendTextMessageAsync(message.From!.Id,
                "Произошла ошибка при создании счета. Попробуйте еще раз.", replyMarkup: MainKeyboard.Main);
        }
    }

    public bool Compare(Message message, User? user) =>
        message.Type == MessageType.Text && user!.State == State.EnterAmount;
}