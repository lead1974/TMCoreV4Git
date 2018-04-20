using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TMCoreV3.DataAccess.Models.Customer;
using TMCoreV3.DataAccess.Models.User;

namespace TMCoreV3.DataAccess.Repos
{
    public class ContactRepository:IContactRepository
    {
        private TMDbContext _context;
        private ILogger<CustomerRepository> _logger;
        private UserManager<AuthUser> _userManager;

        public ContactRepository(TMDbContext context,
            UserManager<AuthUser> userManager,
            ILogger<CustomerRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }
        public void Add(Contact newContact)
        {
            _context.Add(newContact);
        }

        public void Update(Contact theContact)
        {
            _context.Update(theContact);
        }

        public void Remove(Contact theContact)
        {
            _context.Remove(theContact);
        }

        public IEnumerable<Contact> GetAll()
        {
            try
            {
                return _context.Contacts.OrderByDescending(t => t.DateCreated);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get Contacts from database", ex);
                return null;
            }
        }

        public Contact FindById(int Id)
        {
            return _context.Contacts
                           .Where(p => p.ContactId == Id)
                           .FirstOrDefault();
        }

        public Contact FindByName(string Name)
        {
            return _context.Contacts
                           .Where(p => p.Name.Contains(Name))
                           .FirstOrDefault();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
