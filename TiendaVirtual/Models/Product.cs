using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ItemCode { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ImageUrl { get; set; }
    }

    public class Categoria
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}