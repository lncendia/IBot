using IBot.Core.Entities.Products;
using Telegram.Bot.Types.ReplyMarkups;

namespace IBot.BLL.Keyboards.UserKeyboard;

public static class ProductKeyboard
{
    public static readonly InlineKeyboardMarkup Products = new(
        new List<List<InlineKeyboardButton>>
        {
            new() {InlineKeyboardButton.WithCallbackData("➕ Показать список", "products_1")},
            new() {InlineKeyboardButton.WithCallbackData("💵 Мои покупки", "myProducts_1")},
        });

    public static readonly InlineKeyboardMarkup ProductsAdmin = new(
        new List<List<InlineKeyboardButton>>
        {
            new() {InlineKeyboardButton.WithCallbackData("➕ Показать список", "products_1")},
            new() {InlineKeyboardButton.WithCallbackData("💵 Мои покупки", "myProducts_1")},
            new() {InlineKeyboardButton.WithCallbackData("⬇ Загрузить", "uploadProduct")}
        });

    public static InlineKeyboardMarkup Create(IEnumerable<Product> products, int page, bool hasNext)
    {
        var list = products.Select((t, i) => new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData($"{i + 1}. {t.Name} {GetEmoji()}", $"product_{t.Id.ToString()}")
            })
            .ToList();

        var nav = new List<InlineKeyboardButton>();
        if (page != 1) nav.Add(InlineKeyboardButton.WithCallbackData("⬅", "products_" + (page - 1)));
        nav.Add(InlineKeyboardButton.WithCallbackData($"<{page.ToString()}>"));
        if (hasNext) nav.Add(InlineKeyboardButton.WithCallbackData("➡", "products_" + (page + 1)));

        list.AddRange(new List<List<InlineKeyboardButton>> {nav});
        return new InlineKeyboardMarkup(list);
    }

    public static InlineKeyboardMarkup BuyProduct(Product product, bool isAdmin)
    {
        var list = new List<List<InlineKeyboardButton>>
            {new() {InlineKeyboardButton.WithCallbackData("💸 Купить", $"buy_{product.Id}")}};
        if (isAdmin)
            list.Add(new List<InlineKeyboardButton>
                {InlineKeyboardButton.WithCallbackData("🗑 Удалить", $"delete_{product.Id}")});
        return new InlineKeyboardMarkup(list);
    }

    public static InlineKeyboardMarkup MyProducts(Product? product, int page, bool hasNext)
    {
        var list = new List<List<InlineKeyboardButton>>();
        var nav = new List<InlineKeyboardButton>();
        if (page != 1) nav.Add(InlineKeyboardButton.WithCallbackData("⬅", "myProducts_" + (page - 1)));
        nav.Add(InlineKeyboardButton.WithCallbackData($"<{page.ToString()}>"));
        if (hasNext) nav.Add(InlineKeyboardButton.WithCallbackData("➡", "myProducts_" + (page + 1)));

        list.Add(nav);
        if (product == null) return new InlineKeyboardMarkup(list);
        
        var download = new List<InlineKeyboardButton>
            {InlineKeyboardButton.WithCallbackData("⬇ Скачать", $"get_{product.Id}")};
        list.Insert(0, download);

        return new InlineKeyboardMarkup(list);
    }

    private static string GetEmoji() => Emoji[new Random().Next(0, Emoji.Length)];

    private static readonly string[] Emoji =
        {"💖", "💘", "💟", "💝", "💞", "🔮", "💌", "🌌", "🌃", "👄", "💋", "🌸", "👅", "🍃", "☀", "💍"};
}