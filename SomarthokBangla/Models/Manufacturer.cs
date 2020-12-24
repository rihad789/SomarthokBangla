using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SomarthokBangla.Models
{
    public class Manufacturer
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Manufacturer Name")]
        public string ManufacturerName { get; set; }

    }
}
