using Deltek.MyContacts.Data;
using Deltek.MyContacts.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deltek.MyContacts.Services
{
    public class ContactService
    {
        private readonly ILogger _logger;
        private readonly ContactRepository _contactRepo;
        
        public ContactService(ILogger logger, ContactRepository contactRepository)
        {
            _logger = logger;
            _contactRepo = contactRepository;
        }

        public IList<Contact> GetContacts(Guid userId)
        {
            _logger.Information($"Getting contacts for user {userId}...");

            IList<Contact> contacts = _contactRepo.ReadMany(userId).ToList();

            _logger.Information($"Number of contacts for user {userId}: {contacts.Count}");

            return contacts;
        }

        public async Task<Contact> GetContactAsync(Guid userId, int contactId)
        {
            _logger.Information($"Retrieving contact with ID {contactId}...");

            Contact contact = await _contactRepo.ReadAsync(contactId);

            if (contact == null)
            {
                _logger.Warning($"No contact found for contact ID {contactId}");

                return null;
            }

            if (contact.UserId != userId)
            {
                throw new ArgumentException($"Contact {contact.FullName} ({contact.ContactId} is not registered to user {userId}");
            }

            return contact;
        }

        public async Task<Contact> CreateContactAsync(Guid userId, Contact contact)
        {
            _logger.Information($"Creating new contact for user {userId}...");

            contact.UserId = userId;
            
            contact.ContactId = await _contactRepo.CreateAsync(contact);
            
            _logger.Information($"ID for new contact {contact.FullName}: {contact.ContactId}");

            return contact;
        }

        public async Task<bool> UpdateContactAsync(Guid userId, Contact contact)
        {
            // Testing to make sure contact submitted matches user. If not will throw ArgumentException.
            await GetContactAsync(userId, contact.ContactId);

            _logger.Information($"Updating {contact.FullName} ({contact.ContactId})...");

            contact.UserId = userId;

            int updated = await _contactRepo.UpdateAsync(contact);

            _logger.Information($"Contacts updated: {updated}");

            return updated == 1;
        }

        public async Task<bool> DeleteContactAsync(Guid userId, int contactId)
        {
            // Testing to make sure contact submitted matches user. If not will throw ArgumentException.
            Contact contact = await GetContactAsync(userId, contactId);

            _logger.Information($"Deleting contact {contact.FullName} ({contact.ContactId})...");

            int deleted = await _contactRepo.DeleteAsync(contactId);
            
            _logger.Information($"Contacts deleted: {deleted}");

            return deleted == 1;
        }
    }
}