using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetCoding.Core.Interfaces;
using DotnetCoding.Core.Models;
using DotnetCoding.Services.Interfaces;

namespace DotnetCoding.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProductDetails>> GetAllProducts()
        {
            return await _unitOfWork.Products.GetAll();
        }

        public async Task<ProductDetails> GetProductById(int id)
        {
            return await _unitOfWork.Products.GetById(id);
        }

        public async Task<IEnumerable<ProductDetails>> GetFilteredProducts(string productName, decimal? minPrice, decimal? maxPrice, DateTime? startDate, DateTime? endDate)
        {
            var products = await GetAllProducts();

            if (!string.IsNullOrEmpty(productName))
                products = products.Where(p => p.ProductName.Contains(productName));

            if (minPrice.HasValue)
                products = products.Where(p => p.ProductPrice >= minPrice.Value);

            if (maxPrice.HasValue)
                products = products.Where(p => p.ProductPrice <= maxPrice.Value);

            if (startDate.HasValue)
                products = products.Where(p => p.PostedDate >= startDate.Value);

            if (endDate.HasValue)
                products = products.Where(p => p.PostedDate <= endDate.Value);

            return products.OrderByDescending(p => p.PostedDate);
        }

        public async Task<ProductDetails> CreateProduct(ProductDetails product)
        {
            if (product.ProductPrice > 10000)
                throw new Exception("Product price cannot exceed $10,000.");

            _unitOfWork.Products.Add(product);
            await _unitOfWork.CompleteAsync();
            return product;
        }

        public async Task<ProductDetails> UpdateProduct(int id, ProductDetails product)
        {
            var existingProduct = await GetProductById(id);
            if (existingProduct == null)
                return null;

            if (product.ProductPrice > existingProduct.ProductPrice * 1.5m)
                throw new Exception("Price increase too high.");

            existingProduct.ProductName = product.ProductName;
            existingProduct.ProductDescription = product.ProductDescription;
            existingProduct.ProductPrice = product.ProductPrice;
            existingProduct.ProductStatus = product.ProductStatus;

            _unitOfWork.Products.Update(existingProduct);
            await _unitOfWork.CompleteAsync();
            return existingProduct;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await GetProductById(id);
            if (product == null)
                return false;

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task PushToApprovalQueue(ProductDetails product, string reason)
        {
            var approvalEntry = new ApprovalQueue
            {
                ProductId = product.Id,
                Reason = reason,
                RequestDate = DateTime.UtcNow
            };
            _unitOfWork.ApprovalQueue.Add(approvalEntry);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<ApprovalQueue>> GetApprovalQueue()
        {
            return await _unitOfWork.ApprovalQueue.GetAll();
        }

        public async Task ApproveRequest(int id)
        {
            var approvalEntry = await _unitOfWork.ApprovalQueue.GetById(id);
            if (approvalEntry == null)
                return;

            _unitOfWork.ApprovalQueue.Remove(approvalEntry);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RejectRequest(int id)
        {
            var approvalEntry = await _unitOfWork.ApprovalQueue.GetById(id);
            if (approvalEntry == null)
                return;

            _unitOfWork.ApprovalQueue.Remove(approvalEntry);
            await _unitOfWork.CompleteAsync();
        }
    }
}

