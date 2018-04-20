using TMCoreV3.DataAccess.Models.User;
using TMCoreV3.DataAccess.Models.Invoice;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMCoreV3.DataAccess.Repos
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private TMDbContext _context;
        private ILogger<InvoiceRepository> _logger;
        private readonly UserManager<AuthUser> _userManager;

        public InvoiceRepository(TMDbContext context, 
            UserManager<AuthUser> userManager,
            ILogger<InvoiceRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public void Add(Invoice newInvoice)
        {
            _context.Add(newInvoice);
        }

        public void Update(Invoice theInvoice)
        {
            _context.Update(theInvoice);
        }

        public void Remove(Invoice theInvoice)
        {
            _context.Remove(theInvoice);
        }

        public IEnumerable<Invoice> GetAll()
        {
            try
            {
                return _context.Invoices.OrderByDescending(t => t.DateCreated);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get Invoices from database", ex);
                return null;
            }
        }

        public Invoice FindById(int Id)
        {
            return _context.Invoices
                           .Where(p => p.InvoiceId == Id)
                           .FirstOrDefault();
        }

        public Invoice FindByName(string InvoiceName)
        {
            return _context.Invoices
                           .Where(p => p.FirstName.Contains(InvoiceName) || p.LastName.Contains(InvoiceName))
                           .FirstOrDefault();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
