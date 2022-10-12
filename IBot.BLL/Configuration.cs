namespace IBot.BLL;

public class Configuration
{
    public Configuration(string helpUsername) => HelpUsername = helpUsername;

    public string HelpUsername { get; }
}