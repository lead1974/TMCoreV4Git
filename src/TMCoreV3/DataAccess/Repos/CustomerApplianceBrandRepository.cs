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
    public class CustomerApplianceBrandRepository : ICustomerApplianceBrandRepository
    {
        private TMDbContext _context;
        private ILogger<CustomerApplianceBrand> _logger;
        private readonly UserManager<AuthUser> _userManager;
        private ICustomerApplianceTypeRepository _customerApplianceTypeRepo;

        public CustomerApplianceBrandRepository(
            TMDbContext context, 
            UserManager<AuthUser> userManager,
            ILogger<CustomerApplianceBrand> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public void Add(CustomerApplianceBrand newCustomerApplianceType)
        {
            _context.Add(newCustomerApplianceType);
        }

        public void Update(CustomerApplianceBrand newCustomerApplianceType)
        {
            _context.Update(newCustomerApplianceType);
        }
        public void Remove(CustomerApplianceBrand newCustomerApplianceType)
        {
            _context.Remove(newCustomerApplianceType);
        }

        public IEnumerable<CustomerApplianceBrand> GetAll()
        {
            try
            {
                return _context.CustomerApplianceBrands.OrderBy(t => t.Sequence); 
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get customer appliance brands from database", ex);
                return null;
            }
        }

        public CustomerApplianceBrand FindById(int Id)
        {
            return _context.CustomerApplianceBrands
                           .Where(p => p.CustomerApplianceBrandId == Id)
                           .FirstOrDefault();
        }

        public CustomerApplianceBrand FindByName(string customerApplianceBrand)
        {
            return _context.CustomerApplianceBrands
                           .Where(p => p.Name.Contains(customerApplianceBrand))
                           .FirstOrDefault();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
