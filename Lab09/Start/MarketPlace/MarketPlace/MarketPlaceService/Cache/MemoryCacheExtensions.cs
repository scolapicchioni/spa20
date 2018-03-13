using MarketPlaceService.Data;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlaceService.Cache
{
    public static class MemoryCacheExtensions
    {
        public static async Task<ImageFile> GetImageFile(this IMemoryCache memoryCache, int id, IProductsRepository repository) {
            string photoImageIdKey = $"photo-image-{id}";
            ImageFile image;
            if (!memoryCache.TryGetValue(photoImageIdKey, out image)) {
                image = await repository.GetImageFile(id);
                if (image != null)
                    memoryCache.Set(photoImageIdKey, image,
                        new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
            }
            return image;
        }
    }
}
