using System.ComponentModel.DataAnnotations;
using System.Globalization;
using IBot.Extensions;
using Configuration = IBot.Configuration.Configuration;

var culture = CultureInfo.GetCultureInfo("ru-RU");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;
CultureInfo.CurrentCulture = culture;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.GetSection("BotConfiguration").Get<Configuration>();

var validation = new ValidationContext(configuration, null, null);
Validator.ValidateObject(configuration, validation, true);

builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Services.AddApplicationServices(configuration.Token, configuration.HelpAddress);
builder.Services.AddInfrastructureServices(configuration.PaymentToken);
builder.Services.AddPersistenceServices(builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("tgwebhook",
        configuration.Token,
        new {controller = "Bot", action = "Post"});
});
app.Run();