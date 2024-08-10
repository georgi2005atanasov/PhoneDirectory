namespace PhoneDirectory.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using PhoneDirectory.Data;
    using PhoneDirectory.Data.Models;

    public static class ApplicationBuilderExtensions
    {
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
