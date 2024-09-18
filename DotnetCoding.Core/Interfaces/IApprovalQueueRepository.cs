using DotnetCoding.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetCoding.Core.Interfaces
{
    /// <summary>
    /// Interface defining the repository operations for the ApprovalQueue entities.
    /// </summary>
    public interface IApprovalQueueRepository : IGenericRepository<ApprovalQueue>
    {
        /// <summary>
        /// Retrieves an ApprovalQueue entry by its unique identifier.
        /// </summary>
        /// <param name="id">The ID of the approval queue entry to retrieve.</param>
        /// <returns>A task that when awaited returns the ApprovalQueue entry if found, otherwise null.</returns>
        Task<ApprovalQueue> GetById(int id);

        /// <summary>
        /// Adds a new ApprovalQueue entry to the repository.
        /// </summary>
        /// <param name="approvalQueue">The approval queue entry to add.</param>
        void Add(ApprovalQueue approvalQueue);

        /// <summary>
        /// Updates an existing ApprovalQueue entry in the repository.
        /// </summary>
        /// <param name="approvalQueue">The approval queue entry with updated information.</param>
        void Update(ApprovalQueue approvalQueue);

        /// <summary>
        /// Removes an ApprovalQueue entry from the repository.
        /// </summary>
        /// <param name="approvalQueue">The approval queue entry to remove.</param>
        void Remove(ApprovalQueue approvalQueue);
    }
}
