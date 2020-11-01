using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Deltek.MyContacts.Data;
using Deltek.MyContacts.Models;
using Deltek.MyContacts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Deltek.MyContacts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactController : Controller
    {
        private readonly Guid _adminUserId = MyContactsConstants.AdminUserId;
        private readonly ILogger _logger;
        private readonly ContactService _contactService;
        
        public ContactController(ILogger logger, ContactService contactService)
        {
            _logger = logger;
            _contactService = contactService;
        }
        
        [HttpGet]
        public IActionResult ReadContacts()
        {
            try
            {
                IList<Contact> contacts = _contactService.GetContacts(_adminUserId);
                
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unidentified error retrieving contacts for user {_adminUserId}.");
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{contactId:int}")]
        public async Task<IActionResult> ReadContact(int contactId)
        {
            try
            {
                Contact contact = await _contactService.GetContactAsync(_adminUserId, contactId);

                if (contact == null)
                {
                    return BadRequest();
                }

                return Ok(contact);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unidentified error retrieving contact with ID {contactId}.");
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(Contact contact)
        {
            try
            {
                contact = await _contactService.CreateContactAsync(_adminUserId, contact);

                return Created(new Uri($"{Request.Path}/{contact.ContactId}", UriKind.Relative), contact);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unidentified error creating contact ${contact.FullName}.");
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpPut]
        public async Task<IActionResult> UpdateContact(Contact contact)
        {
            try
            {
                bool updated = await _contactService.UpdateContactAsync(_adminUserId, contact);

                if (!updated)
                {
                    throw new DataException($"Error updating contact {contact.FullName}");
                }

                return Ok();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (DataException dataEx)
            {
                _logger.Error(dataEx.Message);
                
                return StatusCode(StatusCodes.Status500InternalServerError, dataEx.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unidentified error updating contact {contact.FullName}.");
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{contactId:int}")]
        public async Task<IActionResult> DeleteContact(int contactId)
        {
            try
            {
                bool deleted = await _contactService.DeleteContactAsync(_adminUserId, contactId);

                if (!deleted)
                {
                    throw new DataException($"Error deleting contact with ID {contactId}");
                }

                return Ok();
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (DataException dataEx)
            {
                _logger.Error(dataEx.Message);
                
                return StatusCode(StatusCodes.Status500InternalServerError, dataEx.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Unidentified error deleting contact with ID {contactId}.");
                
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}