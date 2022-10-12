using IBot.Core.Interfaces.Services.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace IBot.WEB.Controllers;

public class BotController : ControllerBase
{
    private readonly IUpdateHandler _updateService;

    public BotController(IUpdateHandler updateService) => _updateService = updateService;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        await _updateService.HandleAsync(update);
        return Ok();
    }
}