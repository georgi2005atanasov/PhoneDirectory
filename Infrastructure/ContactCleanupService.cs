namespace PhoneDirectory.Infrastructure
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.EntityFrameworkCore;
    using PhoneDirectory.Data;
    using PhoneDirectory.Services.Contact;

    public class ContactCleanupService : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;

        public ContactCleanupService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DeleteMarkedContactsAsync();

                // change this due to the requirements
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task DeleteMarkedContactsAsync()
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var contacts = await dbContext.Contacts
                    .IgnoreQueryFilters()
                    .Where(c => c.IsDeleted)
                    .ToListAsync();

                if (contacts.Any())
                {
                    await using var transaction = await dbContext.Database.BeginTransactionAsync();

                    try
                    {
                        var contactsToDelete = from contact in dbContext.Contacts.IgnoreQueryFilters()
                                               where contact.IsDeleted
                                               select new
                                               {
                                                   Contact = contact,
                                                   Image = dbContext.Images.IgnoreQueryFilters().FirstOrDefault(x => x.ContactId == contact.Id),
                                                   Address = dbContext.Addresses.IgnoreQueryFilters().FirstOrDefault(x => x.Id == contact.AddressId)
                                               };

                        foreach (var entry in contactsToDelete)
                        {
                            if (entry.Image != null)
                            {
                                dbContext.Images.Remove(entry.Image);
                            }

                            if (entry.Address != null)
                            {
                                dbContext.Addresses.Remove(entry.Address);
                            }

                            dbContext.Contacts.Remove(entry.Contact);
                        }

                        await dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                    }
                }
            }
        }
    }
}
