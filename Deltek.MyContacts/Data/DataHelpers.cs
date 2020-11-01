using System;
using Deltek.MyContacts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Deltek.MyContacts.Data
{
    public class DataHelpers
    {
        public static void SeedData()
        {
            ILogger logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            
            logger.Information("Seeding data...");
            
            Guid adminUserId = MyContactsConstants.AdminUserId;
            Guid readonlyUserId = MyContactsConstants.ReadOnlyUserId;
            
            ServiceCollection services = new ServiceCollection();
            
            services.AddDbContext<MyContactsDbContext>(options => options.UseInMemoryDatabase("MyContacts"));

            using ServiceProvider serviceProvider = services.BuildServiceProvider();

            using IServiceScope scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            
            MyContactsDbContext context = scope.ServiceProvider.GetService<MyContactsDbContext>();
            
            Contact contact1 = new Contact
            {
                UserId = readonlyUserId,
                FirstName = "John",
                LastName = "Doe",
                Address1 = "123 Main St.",
                City = "Anytown",
                State = "WA",
                PostalCode = "12345",
                EmailAddress = "jdoe33@myemail.com"
            };

            context.Contacts.Add(contact1);
            
            logger.Information($"Added {contact1.FullName}");
            
            Contact contact2 = new Contact
            {
                UserId = adminUserId,
                FirstName = "Jane",
                LastName = "Doe",
                Address1 = "5432 Park Ave.",
                City = "My City",
                State = "MI",
                PostalCode = "48211",
                EmailAddress = "jane.doe@hotmail.com"
            };

            context.Contacts.Add(contact2);
            
            logger.Information($"Added {contact2.FullName}");
            
            Contact contact3 = new Contact
            {
                UserId = adminUserId,
                FirstName = "Fred",
                LastName = "Smith",
                Address1 = "7788 Wall St.",
                Address2 = "Apt. 2",
                City = "Lexington",
                State = "KY",
                PostalCode = "41180",
                EmailAddress = "fsmith567@gmail.com"
            };

            context.Contacts.Add(contact3);
            
            logger.Information($"Added {contact3.FullName}");

            context.SaveChanges();
        }
    }
}