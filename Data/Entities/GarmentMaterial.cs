using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class GarmentMaterial : BaseEntity
    {
        [ForeignKey("Garment")]
        public int GarmentId { get; set; }
        public Garment Garment { get; set; }

        [ForeignKey("Material")]
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        public decimal RequiredQuantity { get; set; }
        public string UnitOfMeasurement { get; set; }
    }
}
