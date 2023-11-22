using System.ComponentModel.DataAnnotations.Schema;

namespace Pronia.Models
{
    public class Slider
    {
        public int id { get; set; }
        public string Desc { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string? ImageUrl {  get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
