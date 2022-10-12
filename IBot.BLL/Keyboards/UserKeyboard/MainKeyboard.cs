using Telegram.Bot.Types.ReplyMarkups;

namespace IBot.BLL.Keyboards.UserKeyboard;

public static class MainKeyboard
{
    public static readonly ReplyKeyboardMarkup MainReplyKeyboard = new(new List<List<KeyboardButton>>
    {
        new() {new KeyboardButton("👧 Покупки")},
        new() {new KeyboardButton("💸 Баланс"), new KeyboardButton("🤝 Поддержка")}
    })
    {
        ResizeKeyboard = true,
        InputFieldPlaceholder = "Нажмите на нужную кнопку"
    };


    public static InlineKeyboardMarkup Back(string query) =>
        new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("🔙 Назад", $"back_{query}"));

    public static readonly InlineKeyboardMarkup Main =
        new(InlineKeyboardButton.WithCallbackData("⭐ В главное меню", "mainMenu"));
}