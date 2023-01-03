using System;

namespace BOTY.Models.Tables
{
    public class Order
    {
        public int Id { get; set; }
        public int customerId { get; set; }
        public int supplierId { get; set; }
        public int shippingAddressId { get; set; }
        public DateTime dateOrdered { get; set; }
        public DateTime dateShipped { get; set; }
        public double sale { get; set; }
    }
}
