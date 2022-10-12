namespace IBot.Core.Interfaces.Services.Exceptions;

public class BillCheckException : Exception
{
    public BillCheckException(Exception? ex) : base("Bill check failed", ex)
    {
    }
}