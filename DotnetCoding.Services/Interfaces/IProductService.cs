using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotnetCoding.Core.Models;

namespace DotnetCoding.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDetails>> GetAllProducts();
        Task<ProductDetails> GetProductById(int id);
        Task<ProductDetails> CreateProduct(ProductDetails product);
        Task<ProductDetails> UpdateProduct(int id, ProductDetails product);
        Task PushToApprovalQueue(ProductDetails product, string reason);
        Task<IEnumerable<ApprovalQueue>> GetApprovalQueue();
        Task ApproveRequest(int id);
        Task RejectRequest(int id);
    }
}
