using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class CreateOrderItemDTO
    {
        public int OrderId { get; set; }
        public int GarmentId { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
    }
    public class UpdateOrderItemDTO : CreateOrderItemDTO
    {
        public int Id { get; set; }
    }
}
