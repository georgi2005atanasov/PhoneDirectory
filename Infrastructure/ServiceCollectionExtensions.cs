namespace PhoneDirectory.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using PhoneDirectory.Data;
    using PhoneDirectory.Services.Contact;
    using PhoneDirectory.Services.Export;
    using PhoneDirectory.Services.Image;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>
                    (options => options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services,
            string connectionString)
        {
            services
                .AddTransient<IContactService, ContactService>()
                .AddTransient<IImageService, ImageService>()
                .AddTransient<IExportService, ExportService>()
                .AddDatabase(connectionString);

            services.AddHostedService<ContactCleanupService>();

            return services;
        }
    }
}
