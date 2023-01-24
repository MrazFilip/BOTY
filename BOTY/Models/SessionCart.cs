using System.Collections.Generic;
using BOTY.Models.Tables;

namespace BOTY.Models
{
    public static class SessionCart
    {
        public static List<Variant> cart = new();
        public static List<int> count = new();
        public static int supplierId = 1;
    }
}