using System;
using System.Collections.Generic;
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
            // Retrieve all products from the repository
            return await _unitOfWork.Products.GetAll();
        }

        public async Task<ProductDetails> GetProductById(int id)
        {
            // Retrieve a single product by ID
            return await _unitOfWork.Products.GetById(id);
        }

        public async Task<ProductDetails> CreateProduct(ProductDetails product)
        {
            // Add a new product to the repository
            _unitOfWork.Products.Add(product);
            await _unitOfWork.CompleteAsync();  // Save changes asynchronously
            return product;
        }

        public async Task<ProductDetails> UpdateProduct(int id, ProductDetails product)
        {
            // Update an existing product
            var existingProduct = await _unitOfWork.Products.GetById(id);
            if (existingProduct != null)
            {
                existingProduct.ProductName = product.ProductName;
                existingProduct.ProductDescription = product.ProductDescription;
                existingProduct.ProductPrice = product.ProductPrice;
                existingProduct.ProductStatus = product.ProductStatus;

                _unitOfWork.Products.Update(existingProduct);
                await _unitOfWork.CompleteAsync();
            }
            return existingProduct;
        }

        public async Task PushToApprovalQueue(ProductDetails product, string reason)
        {
            // Add a product change to the approval queue
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
            // Retrieve all entries from the approval queue
            return await _unitOfWork.ApprovalQueue.GetAll();
        }

        public async Task ApproveRequest(int id)
        {
            // Approve a request from the approval queue
            var approvalEntry = await _unitOfWork.ApprovalQueue.GetById(id);
            if (approvalEntry != null)
            {
                _unitOfWork.ApprovalQueue.Remove(approvalEntry);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task RejectRequest(int id)
        {
            // Reject a request from the approval queue
            var approvalEntry = await _unitOfWork.ApprovalQueue.GetById(id);
            if (approvalEntry != null)
            {
                _unitOfWork.ApprovalQueue.Remove(approvalEntry);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}

