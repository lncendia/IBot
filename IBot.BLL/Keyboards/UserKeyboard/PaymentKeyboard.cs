
using IBot.Core.Entities.Transactions;
using Telegram.Bot.Types.ReplyMarkups;

namespace IBot.BLL.Keyboards.UserKeyboard;

public static class PaymentKeyboard
{
    public static readonly InlineKeyboardMarkup Subscribes = new(
        new List<List<InlineKeyboardButton>>
        {
            new() {InlineKeyboardButton.WithCallbackData("➕ Пополнить счёт", "buy")},
            new() {InlineKeyboardButton.WithCallbackData("💵 Мои платежи", "paymentsHistory_1")}
        });

    public static readonly InlineKeyboardMarkup PaySubscribe =
        new(InlineKeyboardButton.WithCallbackData("➕ Пополнить счёт", "buy"));

    public static InlineKeyboardMarkup ActivePayments(int page) =>
        new(new List<InlineKeyboardButton>
        {
            InlineKeyboardButton.WithCallbackData("⬅", $"paymentsHistory_{page - 1}"),
            InlineKeyboardButton.WithCallbackData(page.ToString()),
            InlineKeyboardButton.WithCallbackData("➡", $"paymentsHistory_{page + 1}")
        });
    
    
    public static InlineKeyboardMarkup GetTransaction(Transaction transaction, int page, bool hasNext)
    {
        var nav = new List<InlineKeyboardButton>();
        if (page != 1) nav.Add(InlineKeyboardButton.WithCallbackData("⬅", "myTransactions_" + (page - 1)));
        nav.Add(InlineKeyboardButton.WithCallbackData(page.ToString()));
        if (hasNext) nav.Add(InlineKeyboardButton.WithCallbackData("➡", "myTransactions_" + (page + 1)));
        return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>> {nav});
    }
}