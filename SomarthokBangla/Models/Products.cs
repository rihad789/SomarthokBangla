using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SomarthokBangla.Models
{
    public class Products
    {
        public Products()
        {
            List<Products> productsList = new List<Products>();
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        public string Image { get; set; }

        [Required]
        [Display(Name = "Available")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Model No")]
        public string ModelNo { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }

        [ForeignKey("ProductTypeId")]
        public ProductTypes ProductTypes { get; set; }

        [Required]
        [Display(Name = "Manufacturer")]
        public int ManufacturerId { get; set; }

        [ForeignKey("ManufacturerId")]
        public Manufacturer Manufacturer { get; set; }

        [Display(Name = "Details html")]
        public string DetailsHtmlLink { get; set; }

        [Required]
        [Display(Name="Upload Date")]
        public DateTime UplodaDate { get; set; }
    }
}
