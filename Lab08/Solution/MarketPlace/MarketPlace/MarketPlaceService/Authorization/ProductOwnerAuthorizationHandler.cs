using IdentityModel;
using MarketPlaceService.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace MarketPlaceService.Authorization {
    public class ProductOwnerAuthorizationHandler : AuthorizationHandler<ProductOwnerRequirement, Product> {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProductOwnerRequirement requirement, Product resource) {
            if (!context.User.HasClaim(c => c.Type == JwtClaimTypes.Name && c.Issuer == "http://localhost:5002"))
                return Task.CompletedTask;

            var userName = context.User.FindFirst(c => c.Type == JwtClaimTypes.Name && c.Issuer == "http://localhost:5002").Value;

            if (userName == resource.UserName)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
