namespace MarketPlaceService.Models {
    public class Product {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string UserName { get; set; }
        public byte[] ImageFile { get; set; }
        public string ImageMimeType { get; set; }
    }
}
