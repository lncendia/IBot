using IBot.Core.Interfaces.Services;
using IBot.Core.Interfaces.Services.BusinesLogic;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace IBot.WEB.Controllers;

public class PaymentController : ControllerBase
{

    public PaymentController(IUpdateHandler updateService) => _updateService = updateService;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Update update)
    {
        await _updateService.HandleAsync(update);
        return Ok();
    }
}