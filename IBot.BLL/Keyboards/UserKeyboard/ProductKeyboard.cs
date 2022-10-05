using IBot.Core.Entities.Products;
using Telegram.Bot.Types.ReplyMarkups;

namespace IBot.BLL.Keyboards.UserKeyboard;

public static class ProductKeyboard
{
    public static InlineKeyboardMarkup Create(IEnumerable<Product> products, int page, bool hasNext)
    {
        var list = products.Select((t, i) => new List<InlineKeyboardButton>
                {InlineKeyboardButton.WithCallbackData($"{i + 1}. {t} {GetEmoji()}", $"product_{t.Id.ToString()}")})
            .ToList();

        var nav = new List<InlineKeyboardButton>();
        if (page != 1) nav.Add(InlineKeyboardButton.WithCallbackData("⬅", "productsPrev_" + (page - 1)));
        nav.Add(InlineKeyboardButton.WithCallbackData(page.ToString()));
        if (hasNext) nav.Add(InlineKeyboardButton.WithCallbackData("➡", "products_" + (page + 1)));

        list.AddRange(new List<List<InlineKeyboardButton>> {nav});
        return new InlineKeyboardMarkup(list);
    }

    public static InlineKeyboardMarkup BuyProduct(Product product) =>
        new(InlineKeyboardButton.WithCallbackData("💸 Купить", $"buy_{product.Id}"));

    public static InlineKeyboardMarkup GetProduct(Product product, int page, bool hasNext)
    {
        var nav = new List<InlineKeyboardButton>();
        if (page != 1) nav.Add(InlineKeyboardButton.WithCallbackData("⬅", "myProducts_" + (page - 1)));
        nav.Add(InlineKeyboardButton.WithCallbackData(page.ToString()));
        if (hasNext) nav.Add(InlineKeyboardButton.WithCallbackData("➡", "myProducts_" + (page + 1)));

        var download = new List<InlineKeyboardButton>
            {InlineKeyboardButton.WithCallbackData("⬇ Скачать", $"get_{product.Id}")};
        var list = new List<List<InlineKeyboardButton>> {download, nav};
        return new InlineKeyboardMarkup(list);
    }

    private static char GetEmoji() => EmojiList[new Random().Next(0, EmojiList.Length)];

    private const string EmojiList = "💖💘💟💝💞🔮💌🌌🎆🌠🔥💥🌈☀️🌊💁‍♀️💄💋💍👄👅";
}