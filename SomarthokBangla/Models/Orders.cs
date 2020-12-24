  using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SomarthokBangla.Models
{
    public class Orders
    {
        public Orders()
        {
            OrderDetails = new List<OrderDetails>();
        }

        public int Id { get; set; }

        public string OrderNo { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name ="Phone No")]
        public string PhoneNo { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name ="Delivery Address")]
        public string Address { get; set; }

        public DateTime OrderDate { get; set; }

        public bool OrderConfirmed { get; set; }

        public List<OrderDetails> OrderDetails { get; set; }


    }
}
