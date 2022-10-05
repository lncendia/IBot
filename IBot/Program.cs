var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews().AddNewtonsoftJson();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("tgwebhook",
        builder.Configuration["BotConfiguration:Token"],
        new {controller = "Bot", action = "Post"});
    endpoints.MapControllerRoute(
        "default",
        builder.Configuration["BotConfiguration:Token"] + "/{controller=Proxy}/{action=Index}/{id?}");
    endpoints.MapControllers();
});
app.Run();