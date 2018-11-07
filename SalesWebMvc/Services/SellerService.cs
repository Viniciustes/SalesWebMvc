using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Exception;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            await _context.AddAsync(seller);
            await _context.SaveChangesAsync();
        }

        public Seller FindById(int id)
        {
            return _context.Seller.Include(x => x.Department).FirstOrDefault(x => x.Id == id);
        }

        public void Remove(int id)
        {
            try
            {
                var seller = _context.Seller.Find(id);

                _context.Seller.Remove(seller);
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
               throw  new IntegrityException(e.Message);
            }
        }

        public void Update(Seller seller)
        {
            if (!_context.Seller.Any(x => x.Id == seller.Id))
                throw new NotFoundException("Id not found!");

            try
            {
                _context.Update(seller);
                _context.SaveChanges();
            }
            catch (DbConcurrencyException ex)
            {
                throw new DbConcurrencyException("Error: " + ex.Message);
            }
        }
    }
}
