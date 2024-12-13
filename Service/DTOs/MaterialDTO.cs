using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class CreateMaterialDTO
    {
        public string Name { get; set; }
        public decimal UnitCost { get; set; }
        public int QuantityInStock { get; set; }
        public string Unit { get; set; }
    }
    public class UpdateMaterialDTO : CreateMaterialDTO
    {
        public int Id { get; set; }
    }
}
