using Microsoft.AspNetCore.Mvc;
using DotnetCoding.Core.Models;
using DotnetCoding.Services.Interfaces;

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

        [HttpGet]
        public async Task<IActionResult> GetProductList([FromQuery] string productName, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var productDetailsList = await _productService.GetFilteredProducts(productName, minPrice, maxPrice, startDate, endDate);
            if (!productDetailsList.Any())
            {
                return NotFound("No products found with the specified criteria.");
            }
            return Ok(productDetailsList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDetails product)
        {
            try
            {
                var result = await _productService.CreateProduct(product);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDetails product)
        {
            try
            {
                var result = await _productService.UpdateProduct(id, product);
                if (result == null)
                {
                    return NotFound("Product not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProduct(id);
            if (!success)
            {
                return NotFound("Product not found.");
            }
            return Ok("Product deletion processed.");
        }

        [HttpGet("approval-queue")]
        public async Task<IActionResult> GetApprovalQueue()
        {
            var approvalQueue = await _productService.GetApprovalQueue();
            return Ok(approvalQueue);
        }

        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveRequest(int id)
        {
            try
            {
                await _productService.ApproveRequest(id);
                return Ok("Request approved successfully.");
            }
            catch
            {
                return NotFound("Request not found.");
            }
        }

        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectRequest(int id)
        {
            try
            {
                await _productService.RejectRequest(id);
                return Ok("Request rejected successfully.");
            }
            catch
            {
                return NotFound("Request not found.");
            }
        }
    }
}


