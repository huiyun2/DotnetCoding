using Microsoft.AspNetCore.Mvc;
using DotnetCoding.Core.Models;
using DotnetCoding.Services.Interfaces;
using System.Linq; 

namespace DotnetCoding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // Requirement 1 and 2: Get list of active products, allow filtering by name, price, and date range.
        [HttpGet]
        public async Task<IActionResult> GetProductList([FromQuery] string productName, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var productDetailsList = await _productService.GetAllProducts();

            // Filter and sort logic (assuming ProductService handles only basic retrieval)
            if (!string.IsNullOrEmpty(productName))
            {
                productDetailsList = productDetailsList.Where(p => p.ProductName.Contains(productName));
            }
            if (minPrice.HasValue)
            {
                productDetailsList = productDetailsList.Where(p => p.ProductPrice >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                productDetailsList = productDetailsList.Where(p => p.ProductPrice <= maxPrice.Value);
            }
            if (startDate.HasValue)
            {
                productDetailsList = productDetailsList.Where(p => p.PostedDate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                productDetailsList = productDetailsList.Where(p => p.PostedDate <= endDate.Value);
            }

            productDetailsList = productDetailsList.OrderByDescending(p => p.PostedDate);

            if (productDetailsList == null || !productDetailsList.Any())
            {
                return NotFound("No products found with the specified criteria.");
            }
            return Ok(productDetailsList);
        }

        // Requirement 3 and 4: Create a product with price validation and possible approval queue addition.
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDetails product)
        {
            if (product.ProductPrice > 10000)
            {
                return BadRequest("Product price cannot exceed $10,000.");
            }

            if (product.ProductPrice > 5000)
            {
                await _productService.PushToApprovalQueue(product, "Price exceeds $5000.");
            }

            var result = await _productService.CreateProduct(product);
            return Ok(result);
        }

        // Requirement 4 and 5: Update a product with validation on price changes.
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDetails product)
        {
            var existingProduct = await _productService.GetProductById(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            if (product.ProductPrice > 5000)
            {
                await _productService.PushToApprovalQueue(product, "Price exceeds $5000 on update.");
            }

            if (product.ProductPrice > (existingProduct.ProductPrice * 1.5m))
            {
                await _productService.PushToApprovalQueue(product, "Price increased by more than 50%.");
            }

            var result = await _productService.UpdateProduct(id, product);
            return Ok(result);
        }

        // Requirement 6: Push delete request to approval queue.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _productService.GetProductById(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            await _productService.PushToApprovalQueue(existingProduct, "Product deletion request.");
            return Ok("Product deletion request pushed to approval queue.");
        }

        // Requirement 7: View all items in the approval queue, sorted by request date.
        [HttpGet("approval-queue")]
        public async Task<IActionResult> GetApprovalQueue()
        {
            var approvalQueue = await _productService.GetApprovalQueue();
            return Ok(approvalQueue.OrderByDescending(a => a.RequestDate));
        }

        // Requirement 8: Approve a request and update product state.
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            await _productService.ApproveRequest(id);
            return Ok("Request approved successfully.");
        }

        // Requirement 9: Reject a request and maintain the original product state.
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectRequest(int id)
        {
            await _productService.RejectRequest(id);
            return Ok("Request rejected successfully.");
        }
    }
}

