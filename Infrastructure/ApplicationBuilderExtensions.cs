namespace PhoneDirectory.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using PhoneDirectory.Data;

    public static class ApplicationBuilderExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var services = app.ApplicationServices.CreateScope();

            var dbContext = services.ServiceProvider.GetService<ApplicationDbContext>()!;

            if (dbContext.Database.ProviderName != null)
            {
                dbContext.Database.EnsureCreated();
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

                var countriesNumbersLengths = loader
                    .LoadCountriesNumbersLengths("countriesNumbersLengths.txt");
            }

        }
    }
}
