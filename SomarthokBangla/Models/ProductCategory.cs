using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SomarthokBangla.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Product Category")]
        public string CategoryName { get; set; }

        [Required]
        [Display(Name ="Special Tag")]
        public int SpecialTagId { get; set; }

        [ForeignKey("SpecialTagId")]
        public SpecialTag SpecialTag { get; set; }
    }
}
