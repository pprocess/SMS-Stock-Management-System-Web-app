using System.Collections.Generic;

namespace StockManagement.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "User" or "Manager"
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}