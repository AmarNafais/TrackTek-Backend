using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class CreateOrderDTO
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public int GarmentId { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
    }
    public class UpdateOrderDTO : CreateOrderDTO
    {
        public int Id { get; set; }
    }
    public class UpdateOrderStatusDTO
    {
        public int Id { get; set; }

        [DefaultValue("Pending | InProgress | Completed | Canceled")]
        public string OrderStatus { get; set; }
    }
}
