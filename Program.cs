using PhoneDirectory.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

builder.Services
    .AddControllersWithViews();

builder.Services
    .ConfigureServices(connectionString);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app
    .AddCustomMiddlewares()
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Contacts}/{action=All}/{id?}");
    });

app.ApplyMigrations();

app.Run();