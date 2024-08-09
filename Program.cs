using PhoneDirectory.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

builder.Services
    .AddDatabase(connectionString);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=PhoneDirectory}/{action=All}/{id?}");
});

app.ApplyMigrations();

app.Run();
