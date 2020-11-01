using System;

namespace Deltek.MyContacts.Models
{
    public interface IContact
    {
        int ContactId { get; set; }
        
        Guid UserId { get; set; }
        
        string EmailAddress { get; set; }
        
        string FirstName { get; set; }
        
        string LastName { get; set; }
        
        string Address1 { get; set; }
        
        string Address2 { get; set; }
        
        string City { get; set; }
        
        string State { get; set; }
        
        string PostalCode { get; set; }

        string FullName { get; }
    }
}