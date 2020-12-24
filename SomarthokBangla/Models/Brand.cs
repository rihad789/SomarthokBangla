using SomarthokBangla.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SomarthokBangla.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "BrandName")]
        public string BrandName { get; set; }

        [Required]
        [Display(Name = "Product Category")]
        public int ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        public ProductCategory ProductCategory { get; set; }
    }
}
