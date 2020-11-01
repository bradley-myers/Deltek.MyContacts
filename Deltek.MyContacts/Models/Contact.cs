using System;

namespace Deltek.MyContacts.Models
{
    public class Contact : IContact
    {
        public int ContactId { get; set; }
        
        // TODO: Hide in response.
        public Guid UserId { get; set; }
        
        public string EmailAddress { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Address1 { get; set; }
        
        public string Address2 { get; set; }
        
        public string City { get; set; }
        
        public string State { get; set; }
        
        public string PostalCode { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }
}