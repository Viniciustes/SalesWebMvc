using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from salesRecord in _context.SalesRecord select salesRecord;

            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date.Date >= minDate.Value.Date);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date.Date <= maxDate.Value.Date);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from salesRecord in _context.SalesRecord select salesRecord;

            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date.Date >= minDate.Value.Date);
            }

            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date.Date <= maxDate.Value.Date);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date)
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }
    }
}
