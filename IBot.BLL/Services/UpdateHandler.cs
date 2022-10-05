using IBot.BLL.CallbackQueryCommands;
using IBot.BLL.Interfaces;
using IBot.BLL.TextCommands;
using IBot.Core.Interfaces.Repositories;
using IBot.Core.Interfaces.Services;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace IBot.BLL.Services;

public class UpdateHandler : IUpdateHandler<Update>
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UpdateHandler> _logger;
    private readonly ServiceContainer _serviceContainer;

    public UpdateHandler(ITelegramBotClient botClient, ILogger<UpdateHandler> logger, IUnitOfWork uw)
    {
        _botClient = botClient;
        _logger = logger;
        _serviceContainer = new ServiceContainer(uw);
    }

    private static readonly List<ITextCommand> TextCommands = new()
    {
        new StartCommand(),
        new BanCommand(),
        new SendKeyboardCommand(),
        new EnterMessageToMailingCommand(),
        new AdminMailingCommand(),

        //Do not depend on the state
        new HelpCommand(),
        new PaymentCommand(),
    };

    private static readonly List<ICallbackQueryCommand> CallbackQueryCommands = new()
    {
        new BillQueryCommand(),
        new MainMenuQueryCommand(),
        new MyPaymentsQueryCommand(),
        new BuySubscribeQueryCommand(),
    };

    public async Task HandleAsync(Update update)
    {
        var handler = update.Type switch
        {
            UpdateType.Message => BotOnMessageReceived(update.Message!),
            UpdateType.CallbackQuery => BotOnCallbackQueryReceived(update.CallbackQuery!),
            _ => Task.CompletedTask
        };

        try
        {
            await handler;
        }
        catch (Exception exception)
        {
            HandleErrorAsync(update, exception);
        }
    }

    private void HandleErrorAsync(Update update, Exception ex) => _logger.LogError(ex, "Update id: {Id}", update.Id);

    private async Task BotOnCallbackQueryReceived(CallbackQuery updateCallbackQuery)
    {
        var user = await _serviceContainer.UnitOfWork.UserRepository.Value.GetByTelegramIdAsync(updateCallbackQuery.From.Id);

        var command = CallbackQueryCommands.FirstOrDefault(command => command.Compare(updateCallbackQuery, user));
        if (command != null)
            await command.Execute(_botClient, user, updateCallbackQuery, _serviceContainer);
    }

    private async Task BotOnMessageReceived(Message updateMessage)
    {
        var user = await _serviceContainer.UnitOfWork.UserRepository.Value.GetByTelegramIdAsync(updateMessage.From!.Id);
        var command = TextCommands.FirstOrDefault(command => command.Compare(updateMessage, user));
        if (command != null)
            await command.Execute(_botClient, user, updateMessage, _serviceContainer);
    }
}