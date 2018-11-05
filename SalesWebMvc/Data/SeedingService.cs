using System;
using System.Linq;
using SalesWebMvc.Models;
using SalesWebMvc.Models.Enums;

namespace SalesWebMvc.Data
{
    public class SeedingService
    {
        private readonly SalesWebMvcContext _context;

        public SeedingService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            if (_context.Department.Any() || _context.Seller.Any() || _context.SalesRecord.Any())
                return;

            var department = new Department("Computers");

            var seller = new Seller("Vinícius", "viniciustes@gmail.com", 1000.00, new DateTime(1981, 09, 30), department);

            var salesRecord = new  SalesRecord(DateTime.Now, 500.0, SaleStatus.Billed, seller);

            _context.Department.Add(department);
            _context.Seller.Add(seller);
            _context.SalesRecord.Add(salesRecord);
            _context.SaveChanges();
        }
    }
}
