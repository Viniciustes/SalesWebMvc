using System;
using System.Collections.Generic;
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

            var listDepartments = new List<Department> { new Department("Computers"), new Department("Books") };

            var listSellers = new List<Seller>
            {
                new Seller("Vinícius", "viniciustes@gmail.com", 1000.00, new DateTime(1981, 09, 30), listDepartments.First()),
                new Seller("Valentina", "valentina@gmail.com", 2000.00, new DateTime(2012, 04, 16), listDepartments.Last())
            };

            var listSalesRecords = new List<SalesRecord>
            {
                new SalesRecord(DateTime.Now, 500.0, SaleStatus.Canceled, listSellers.First()),
                new SalesRecord(DateTime.Now, 1500.0, SaleStatus.Billed, listSellers.Last()),
                new SalesRecord(DateTime.Now, 2500.0, SaleStatus.Pending, listSellers.First()),
                new SalesRecord(DateTime.Now, 3500.0, SaleStatus.Billed, listSellers.Last()),
                new SalesRecord(DateTime.Now, 400.0, SaleStatus.Canceled, listSellers.First()),
                new SalesRecord(DateTime.Now, 5500.0, SaleStatus.Billed, listSellers.Last())
            };

            _context.Department.AddRange(listDepartments);
            _context.Seller.AddRange(listSellers);
            _context.SalesRecord.AddRange(listSalesRecords);
            _context.SaveChanges();
        }
    }
}
