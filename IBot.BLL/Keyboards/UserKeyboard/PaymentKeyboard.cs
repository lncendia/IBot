
using IBot.Core.Entities.Transactions;
using IBot.Core.Interfaces.Services.DTOs;
using Telegram.Bot.Types.ReplyMarkups;

namespace IBot.BLL.Keyboards.UserKeyboard;

public static class PaymentKeyboard
{
    public static readonly InlineKeyboardMarkup Subscribes = new(
        new List<List<InlineKeyboardButton>>
        {
            new() {InlineKeyboardButton.WithCallbackData("➕ Пополнить счёт", "buy")},
            new() {InlineKeyboardButton.WithCallbackData("💵 Мои платежи", "myTransactions_1")}
        });

    public static readonly InlineKeyboardMarkup TopUpAmount =
        new(InlineKeyboardButton.WithCallbackData("➕ Пополнить счёт", "buy"));

    public static InlineKeyboardMarkup MyTransactions(int page, bool hasNext)
    {
        var nav = new List<InlineKeyboardButton>();
        if (page != 1) nav.Add(InlineKeyboardButton.WithCallbackData("⬅", "myTransactions_" + (page - 1)));
        nav.Add(InlineKeyboardButton.WithCallbackData($"<{page.ToString()}>"));
        if (hasNext) nav.Add(InlineKeyboardButton.WithCallbackData("➡", "myTransactions_" + (page + 1)));
        return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>> {nav});
    }
    public static InlineKeyboardMarkup CheckBill(PaymentDto payment) =>
        new(new List<List<InlineKeyboardButton>>
        {
            new() {InlineKeyboardButton.WithUrl("Оплатить", payment.PayUrl.ToString())},
            new() {InlineKeyboardButton.WithCallbackData("Проверить оплату", $"bill_{payment.Id}")},
        });
}