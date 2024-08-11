using PhoneDirectory.Infrastructure;
using PhoneDirectory.Services.Contacts;
using PhoneDirectory.Services.Images;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllersWithViews();
builder.Services.AddTransient<IContactService, ContactService>();
builder.Services.AddTransient<IImageService, ImageService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

builder.Services
    .AddDatabase(connectionString);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.Use(async (context, next) =>
{
    if (context.Request.Method == "POST" && context.Request.Form["_method"] == "PUT")
    {
        context.Request.Method = "PUT";
    }
    await next();
});

app.Use(async (context, next) =>
{
    if (context.Request.Method == "POST" && context.Request.Form["_method"] == "DELETE")
    {
        context.Request.Method = "DELETE";
    }
    await next();
});

app
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Contacts}/{action=All}/{id?}");
});

app.ApplyMigrations();

app.Run();
