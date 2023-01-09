using BOTY.Models.Tables;
using System.Collections.Generic;
using System.IO;

namespace BOTY.Models.Entities
{
    public class ProductImagesCategories
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string information { get; set; }
        public int manufacturer { get; set; }
        public int material { get; set; }
        public string code { get; set; }
        public int[] categories { get; set; }
        public string[] images { get; set; }
    }
}
