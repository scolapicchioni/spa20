using MarketPlaceService.Data;
using MarketPlaceService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MarketPlaceService.Controllers {
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        private readonly IProductsRepository _ProductsRepository;

        public ProductsController(IProductsRepository ProductsRepository) {
            _ProductsRepository = ProductsRepository;
        }

        [HttpGet]
        public IEnumerable<Product> GetAll() {
            return _ProductsRepository.GetAll();
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public IActionResult GetById(int id) {
            var item = _ProductsRepository.Find(id);
            if (item == null) {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Product product) {
            if (product == null) {
                return BadRequest();
            }

            _ProductsRepository.Add(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product product) {
            if (product == null || product.Id != id) {
                return BadRequest();
            }

            var original = _ProductsRepository.Find(id);
            if (original == null) {
                return NotFound();
            }

            original.Name = product.Name;
            original.Description = product.Description;
            original.Price = product.Price;

            _ProductsRepository.Update(original);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            var product = _ProductsRepository.Find(id);
            if (product == null) {
                return NotFound();
            }

            _ProductsRepository.Remove(id);
            return new NoContentResult();
        }
    }
}