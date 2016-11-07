
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TheWall.Models
{
    public class Product
    {

        public Product()
        {
            Shoppingcarts = new List<Shoppingcart>();
        }
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }

        public List<Shoppingcart> Shoppingcarts { get; set; }
    }
}