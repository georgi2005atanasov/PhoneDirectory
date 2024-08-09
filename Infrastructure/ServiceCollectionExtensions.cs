namespace PhoneDirectory.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using PhoneDirectory.Data;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>
                    (options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
