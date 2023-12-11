using Pronia.Models.Entity;
using System;

namespace Pronia.Models
{
    public class Order:BaseEntity
    {
        public string AppuserId { get; set; }   
        public AppUser AppUser { get; set; }
        List<BasketItem> BasketItems { get; set; }  
        public bool? Status { get; set; }
        public string Address { get; set; } 
        public DateTime CreateData { get; set; }    
    }
}
