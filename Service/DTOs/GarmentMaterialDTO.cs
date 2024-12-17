using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class CreateGarmentMaterialDTO
    {
        public int GarmentId { get; set; }
        public int MaterialId { get; set; }
        public decimal RequiredQuantity { get; set; }
        public string UnitOfMeasurement { get; set; }
    }
    public class UpdateGarmentMaterialDTO : CreateGarmentMaterialDTO
    {
        public int Id { get; set; }
    }
}
