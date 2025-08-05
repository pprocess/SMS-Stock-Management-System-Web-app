using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockManagement.Models
{
    public class Product : IEquatable<Product>
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = "/images/placeholder.png";
        public string Description { get; set; }
        public DateTime LastPurchasedDate { get; set; } = DateTime.Now;
        public int PurchaseCount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public bool Equals(Product other)
        {
            if (other == null) return false;
            return this.Id == other.Id;
        }

        public override bool Equals(object obj) => Equals(obj as Product);
        public override int GetHashCode() => Id.GetHashCode();
    }
}