namespace BOTY.Models.Tables
{
    public class Variant
    {
        public int Id { get; set; }
        public int productId { get; set; }
        public int colorId { get; set; }
        public int sizeId { get; set; }
        public double price { get; set; }
        public double sale { get; set; }
        public double vat { get; set; }
        public int stock { get; set; }
    }
}
