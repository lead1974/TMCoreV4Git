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
    public class CustomerCouponRepository : ICustomerCouponRepository
    {
        private TMDbContext _context;
        private ILogger<CustomerCouponRepository> _logger;
        private readonly UserManager<AuthUser> _userManager;
        private ICustomerApplianceTypeRepository _customerCouponRepo;

        public CustomerCouponRepository(
            TMDbContext context, 
            UserManager<AuthUser> userManager,
            ILogger<CustomerCouponRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public void Add(CustomerCoupon newCustomerCoupon)
        {
            _context.Add(newCustomerCoupon);
        }

        public void Update(CustomerCoupon newCustomerCoupon)
        {
            _context.Update(newCustomerCoupon);
        }
        public void Delete(CustomerCoupon newCustomerCoupon)
        {
            _context.Remove(newCustomerCoupon);
        }

        public IEnumerable<CustomerCoupon> GetAll()
        {
            try
            {
                return _context.CustomerCoupons.OrderByDescending(t => t.Sequence); 
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get customer coupons from database", ex);
                return null;
            }
        }

        public IEnumerable<CustomerCoupon> GetAllNonExpired()
        {
            try
            {
                return _context.CustomerCoupons.Where(t=>t.ExpirationDate>=DateTime.UtcNow).OrderByDescending(t => t.Sequence);
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get customer coupons from database", ex);
                return null;
            }
        }

        public CustomerCoupon FindById(int Id)
        {
            return _context.CustomerCoupons
                           .Where(p => p.CustomerCouponId == Id)
                           .FirstOrDefault();
        }

        public CustomerCoupon FindByName(string customerCoupon)
        {
            return _context.CustomerCoupons
                           .Where(p => p.Name.Contains(customerCoupon))
                           .FirstOrDefault();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
