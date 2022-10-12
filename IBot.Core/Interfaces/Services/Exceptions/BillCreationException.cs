namespace IBot.Core.Interfaces.Services.Exceptions;

public class BillCreationException : Exception
{
    public BillCreationException(Exception? ex) : base("Error creating bill", ex)
    {
    }
}