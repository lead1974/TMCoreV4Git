using TMCoreV3.DataAccess.Models.User;
using TMCoreV3.DataAccess.Models.Customer;
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
    public class CustomerApplianceTypeRepository : ICustomerApplianceTypeRepository
    {
        private TMDbContext _context;
        private ILogger<CustomerApplianceTypeRepository> _logger;
        private readonly UserManager<AuthUser> _userManager;
        private ICustomerApplianceTypeRepository _customerApplianceTypeRepo;

        public CustomerApplianceTypeRepository(
            TMDbContext context, 
            UserManager<AuthUser> userManager,
            ILogger<CustomerApplianceTypeRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public void Add(CustomerApplianceType newCustomerApplianceType)
        {
            _context.Add(newCustomerApplianceType);
        }

        public void Update(CustomerApplianceType newCustomerApplianceType)
        {
            _context.Update(newCustomerApplianceType);
        }
        public void Remove(CustomerApplianceType newCustomerApplianceType)
        {
            _context.Remove(newCustomerApplianceType);
        }

        public IEnumerable<CustomerApplianceType> GetAll()
        {
            try
            {
                return _context.CustomerApplianceTypes.OrderBy(t => t.Sequence); 
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get customer appliance types from database", ex);
                return null;
            }
        }

        public IEnumerable<CustomerApplianceType> GetAllWithBrands()
        {
            try
            {
                return _context.CustomerApplianceTypes
                .Include(c => c.CustomerApplianceBrands)
                .OrderBy(t => t.Sequence)
                .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get CustomerApplianceTypes with their Brands from database", ex);
                return null;
            }
        }

        public CustomerApplianceType FindById(int Id)
        {
            return _context.CustomerApplianceTypes.Include(c => c.CustomerApplianceBrands)
                           .Where(p => p.CustomerApplianceTypeId == Id)
                           .FirstOrDefault();
        }

        public CustomerApplianceType FindByName(string customerApplianceType)
        {
            return _context.CustomerApplianceTypes.Include(p => p.CustomerApplianceBrands)
                           .Where(p => p.Name.Contains(customerApplianceType))
                           .FirstOrDefault();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
