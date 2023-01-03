namespace BOTY.Models.Tables
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int orderId { get; set; }
        public int variantProductId { get; set; }
        public int quantity { get; set; }
        public double Price { get; set; }
    }
}
