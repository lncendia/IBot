using System.Net;
using IBot.Core.Interfaces.Services.DTOs;
using IBot.Core.Interfaces.Services.Exceptions;
using IBot.Core.Interfaces.Services.Infrastructure;
using Newtonsoft.Json;
using Qiwi.BillPayments.Client;
using Qiwi.BillPayments.Model;
using Qiwi.BillPayments.Model.In;
using Qiwi.BillPayments.Model.Out;
using RestSharp;

namespace IBot.PaymentSystem;

public class PaymentService : IPaymentCreationService
{
    private readonly string _paymentToken;

    public PaymentService(string paymentToken) => _paymentToken = paymentToken;

    public async Task<PaymentDto> CreateBillAsync(Guid userId, decimal amount)
    {
        var client = BillPaymentsClientFactory.Create(_paymentToken);
        var response = await client.CreateBillAsync(
            new CreateBillInfo
            {
                BillId = Guid.NewGuid().ToString(),
                Amount = new MoneyAmount
                {
                    ValueDecimal = amount,
                    CurrencyEnum = CurrencyEnum.Rub
                },
                ExpirationDateTime = DateTime.Now.AddDays(5),
                Customer = new Customer {Account = userId.ToString()}
            });
        return new PaymentDto(response.BillId, response.PayUrl);
    }

    public async Task<TransactionDto> GetPaymentAsync(string id)
    {
        RestClient httpClient = new($"https://api.qiwi.com/partner/bill/v1/bills/{id}");
        var request = new RestRequest();
        request.AddHeader("Accept", "application/json");
        request.AddHeader("Authorization", $"Bearer {_paymentToken}");
        var response1 = await httpClient.ExecuteAsync(request);
        if (response1.StatusCode != HttpStatusCode.OK) throw new BillCheckException(response1.ErrorException);
        var response = JsonConvert.DeserializeObject<BillResponse>(response1.Content!);
        if (response?.Status.ValueString != "PAID") throw new BillNotPaidException();
        return new TransactionDto(response.Amount.ValueDecimal,
            TimeZoneInfo.ConvertTimeToUtc(response.Status.ChangedDateTime, TimeZoneInfo.Local),
            Guid.Parse(response.Customer.Account));
    }
}