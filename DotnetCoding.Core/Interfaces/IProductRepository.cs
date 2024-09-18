using DotnetCoding.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DotnetCoding.Core.Interfaces
{
    /// <summary>
    /// Interface for repository operations specifically for ProductDetails entities.
    /// </summary>
    public interface IProductRepository : IGenericRepository<ProductDetails>
    {
        /// <summary>
        /// Retrieves a single product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>A task that when awaited returns the product if found; otherwise, null.</returns>
        Task<ProductDetails> GetById(int id);

        /// <summary>
        /// Adds a new product to the repository.
        /// </summary>
        /// <param name="product">The product to add.</param>
        void Add(ProductDetails product);

        /// <summary>
        /// Updates an existing product in the repository.
        /// </summary>
        /// <param name="product">The product with updated information.</param>
        void Update(ProductDetails product);

        /// <summary>
        /// Removes a product from the repository.
        /// </summary>
        /// <param name="product">The product to remove.</param>
        void Remove(ProductDetails product);
    }
}

