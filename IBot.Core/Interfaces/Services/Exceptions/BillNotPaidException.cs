namespace IBot.Core.Interfaces.Services.Exceptions;

public class BillNotPaidException:Exception

{
    public BillNotPaidException() : base("Bill not paid")
    {
    }

}