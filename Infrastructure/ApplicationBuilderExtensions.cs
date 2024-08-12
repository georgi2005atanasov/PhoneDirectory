namespace PhoneDirectory.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using PhoneDirectory.Data;
    using PhoneDirectory.Data.Models;
    using PhoneDirectory.Utilities;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder AddCustomMiddlewares(this IApplicationBuilder app)
            => app.Use(async (context, next) =>
             {
                 if (context.Request.Method == "POST" && context.Request.Form["_method"] == "PUT")
                 {
                     context.Request.Method = "PUT";
                 }
                 await next();
             })
            .Use(async (context, next) =>
            {
                if (context.Request.Method == "POST" && context.Request.Form["_method"] == "DELETE")
                {
                    context.Request.Method = "DELETE";
                }
                await next();
            });

        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var services = app.ApplicationServices.CreateScope();

            var dbContext = services.ServiceProvider.GetService<ApplicationDbContext>()!;

            if (dbContext.Database.ProviderName != null)
            {
                dbContext.Database.Migrate();

                dbContext.SeedDatabase(services);
            }
        }

        public static void SeedDatabase(this ApplicationDbContext dbContext, IServiceScope services)
        {
            var loader = new XmlDataLoader();

            if (!dbContext.Countries.Any())
            {
                var countries = loader
                    .LoadCountries("countries.txt");

                dbContext.Countries
                .AddRange(countries.Select(x => new Country
                {
                    Name = x.Name,
                    IsoCode = x.IsoCode,
                    CountryPrefix = x.PhonePrefix
                }));

                dbContext.SaveChanges();
            }
        }
    }
}
