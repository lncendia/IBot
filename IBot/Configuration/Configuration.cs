using System.ComponentModel.DataAnnotations;

namespace IBot.Configuration;

public class Configuration
{
    [Required(ErrorMessage = "Bot token is not set")]
    public string Token { get; init; } = null!;

    [Required(ErrorMessage = "Payment token is not set")]
    public string PaymentToken { get; init; } = null!;

    [Required(ErrorMessage = "Help address is not set")]
    public string HelpAddress { get; init; } = null!;
}