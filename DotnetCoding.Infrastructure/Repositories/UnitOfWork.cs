using System;
using System.Threading.Tasks;
using DotnetCoding.Core.Interfaces;

namespace DotnetCoding.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContextClass _dbContext;

        public IProductRepository Products { get; private set; }
        public IApprovalQueueRepository ApprovalQueue { get; private set; }

        // Constructor with dependency injection
        public UnitOfWork(DbContextClass dbContext,
                          IProductRepository productRepository,
                          IApprovalQueueRepository approvalQueueRepository)
        {
            _dbContext = dbContext;
            Products = productRepository;
            ApprovalQueue = approvalQueueRepository;
        }

        // Save changes asynchronously
        public async Task CompleteAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
        }
    }
}

