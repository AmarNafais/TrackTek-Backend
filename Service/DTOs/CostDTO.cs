using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class  CreateCostDTO
    {
        public int OrderId { get; set; }
    }
    public class UpdateCostDTO : CreateCostDTO
    {
        public int Id { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal LaborCost { get; set; }
        public decimal MachineCost { get; set; }
        public decimal TotalCost { get; set; }
    }
}
