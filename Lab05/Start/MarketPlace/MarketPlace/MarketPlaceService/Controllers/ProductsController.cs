using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Data;
using MarketPlaceService.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceService.Controllers {
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        //requires using MarketPlaceService.Data;
        private readonly IProductsRepository _ProductsRepository;

        public ProductsController(IProductsRepository ProductsRepository) {
            _ProductsRepository = ProductsRepository;
        }

        //requires using MarketPlaceService.Models;
        [HttpGet]
        public async Task<IEnumerable<Product>> GetAllAsync() {
            return await _ProductsRepository.GetAllAsync();
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<IActionResult> GetByIdAsync(int id) {
            var item = await _ProductsRepository.FindAsync(id);
            if (item == null) {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Product product) {
            if (product == null) {
                return BadRequest();
            }

            await _ProductsRepository.AddAsync(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Product product) {
            if (product == null || product.Id != id) {
                return BadRequest();
            }

            var original = await _ProductsRepository.FindAsync(id);
            if (original == null) {
                return NotFound();
            }

            original.Name = product.Name;
            original.Description = product.Description;
            original.Price = product.Price;

            await _ProductsRepository.UpdateAsync(original);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var product = await _ProductsRepository.FindAsync(id);
            if (product == null) {
                return NotFound();
            }

            await _ProductsRepository.RemoveAsync(id);
            return new NoContentResult();
        }
    }
}