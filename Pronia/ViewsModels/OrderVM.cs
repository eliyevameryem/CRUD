using Pronia.Models;

namespace Pronia.ViewsModels
{
    public class OrderVM
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public string Address { get; set; }
        public List<BasketItem>? BasketItems { get; set; }
        public List<CheckoutItemVM> CheckoutItems { get; set; } = new List<CheckoutItemVM>();



    }
}
