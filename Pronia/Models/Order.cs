using Pronia.Models.Entity;
using Pronia.Utilities;
using System;

namespace Pronia.Models
{
    public class Order:BaseEntity
    {
        public string AppuserId { get; set; }   
        public AppUser? AppUser { get; set; }
        public List<BasketItem> BasketItems { get; set; }  
        public OrderStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
        public double TotalPrice { get; set; }
        public string Address { get; set; } 
        public string Name { get; set; }
        public string Surname { get; set; } 
        public string Email { get; set; }
       
    }
}
