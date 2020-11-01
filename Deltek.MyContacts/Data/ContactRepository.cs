using Deltek.MyContacts.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Deltek.MyContacts.Data
{
    public class ContactRepository : MyContactsRepositoryBase
    {
        public ContactRepository(MyContactsDbContext myContactsDbContext) : base(myContactsDbContext) {}

        public IQueryable<Contact> ReadMany(Guid userId)
        {
            return _dbContext.Contacts.Where(c => c.UserId == userId);
        }

        public async Task<Contact> ReadAsync(int contactId)
        {
            return await _dbContext.Contacts.FindAsync(contactId);
        }

        public async Task<int> CreateAsync(Contact newContact)
        {
            await _dbContext.AddAsync(newContact);

            await SaveAsync();

            return newContact.ContactId;
        }
        
        public async Task<int> UpdateAsync(Contact contact)
        { 
            _dbContext.Entry(contact).State = EntityState.Modified;

            return await SaveAsync();
        }

        public async Task<int> DeleteAsync(int contactId)
        {
            Contact contact = await ReadAsync(contactId);

            if (contact == null)
            {
                return -1;
            }

            _dbContext.Remove(contact);

            return await SaveAsync();
        }
    }
}