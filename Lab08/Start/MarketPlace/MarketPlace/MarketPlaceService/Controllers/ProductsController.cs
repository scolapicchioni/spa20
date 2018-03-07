using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlaceService.Data;
using MarketPlaceService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlaceService.Controllers {
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        //requires using MarketPlaceService.Data;
        private readonly IProductsRepository _ProductsRepository;
        private IAuthorizationService _authorizationService;

        public ProductsController(IProductsRepository ProductsRepository,
            IAuthorizationService authorizationService) {
            _ProductsRepository = ProductsRepository;
            _authorizationService = authorizationService;
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
        [Authorize]
        public async Task<IActionResult> CreateAsync([FromBody] Product product) {
            if (product == null || product.UserName != User.Identity.Name) {
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

            AuthorizationResult authresult = await _authorizationService.AuthorizeAsync(User, original, "ProductOwner");
            if (!authresult.Succeeded) {
                if (User.Identity.IsAuthenticated) {
                    return new ForbidResult();
                } else {
                    return new ChallengeResult();
                }
            }

            original.Name = product.Name;
            original.Description = product.Description;
            original.Price = product.Price;

            await _ProductsRepository.UpdateAsync(original);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id) {
            var product = await _ProductsRepository.FindAsync(id);
            if (product == null) {
                return NotFound();
            }

            AuthorizationResult authresult = await _authorizationService.AuthorizeAsync(User, product, "ProductOwner");
            if (!authresult.Succeeded) {
                if (User.Identity.IsAuthenticated) {
                    return new ForbidResult();
                } else {
                    return new ChallengeResult();
                }
            }

            await _ProductsRepository.RemoveAsync(id);
            return new NoContentResult();
        }
    }
}