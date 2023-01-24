namespace BOTY.Models.Entities
{
    public class FullOrder
    {
        //SUPPLIER
        public int supplierId { get; set; }
        //CUSTOMER
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailName { get; set; }
        public string phoneNumber { get; set; }
        //ADDRESS
        public string address { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public int postalCode { get; set; }
    }
}