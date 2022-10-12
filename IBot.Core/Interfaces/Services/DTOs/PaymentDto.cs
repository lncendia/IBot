namespace IBot.Core.Interfaces.Services.DTOs;

public class PaymentDto
{
    public PaymentDto(string id, Uri payUrl)
    {
        Id = id;
        PayUrl = payUrl;
    }

    public string Id { get; }
    public Uri PayUrl { get; }
}