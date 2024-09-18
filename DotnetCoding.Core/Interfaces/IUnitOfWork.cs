

namespace DotnetCoding.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IApprovalQueueRepository ApprovalQueue { get; }

        Task CompleteAsync();  // Save changes to the database
    }
}
