using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotnetCoding.Core.Models;

namespace DotnetCoding.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDetails>> GetAllProducts();
        Task<ProductDetails> GetProductById(int id);
        Task<IEnumerable<ProductDetails>> GetFilteredProducts(string productName, decimal? minPrice, decimal? maxPrice, DateTime? startDate, DateTime? endDate);
        Task<ProductDetails> CreateProduct(ProductDetails product);
        Task<ProductDetails> UpdateProduct(int id, ProductDetails product);
        Task<bool> DeleteProduct(int id);
        Task PushToApprovalQueue(ProductDetails product, string reason);
        Task<IEnumerable<ApprovalQueue>> GetApprovalQueue();
        Task ApproveRequest(int id);
        Task RejectRequest(int id);
    }
}

