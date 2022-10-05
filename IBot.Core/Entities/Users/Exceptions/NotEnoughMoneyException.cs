namespace IBot.Core.Entities.Users.Exceptions;

public class NotEnoughMoneyException:Exception
{
    public NotEnoughMoneyException():base("Not enough money")
    { }
}