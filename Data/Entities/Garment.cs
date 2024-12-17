using Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Garment : BaseEntity
    {
        public string Name { get; set; }
        public string Design { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public Category CategoryType { get; set; }
        public string[] Sizes { get; set; }
        public decimal BasePrice { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public Status.GarmentStatus GarmentStatus { get; set; }
        public decimal LaborHoursPerUnit { get; set; }
        public decimal HourlyLaborRate { get; set; }

    }
}
