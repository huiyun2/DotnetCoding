using DotnetCoding.Core.Models;
using DotnetCoding.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetCoding.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbContextClass _dbContext;

        public ProductRepository(DbContextClass dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductDetails> GetById(int id)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Add(ProductDetails product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
        }

        public void Update(ProductDetails product)
        {
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
        }

        public void Remove(ProductDetails product)
        {
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
        }

        public async Task<IEnumerable<ProductDetails>> GetAll()
        {
            return await _dbContext.Products.ToListAsync();
        }
    }
}



