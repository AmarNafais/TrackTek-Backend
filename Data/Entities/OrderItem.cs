using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class OrderItem : BaseEntity
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [ForeignKey("Garment")]
        public int GarmentId { get; set; }
        public Garment Garment { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        }
}
