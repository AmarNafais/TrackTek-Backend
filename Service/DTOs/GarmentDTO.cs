using Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class CreateGarmentDTO
    {
        public string Name { get; set; }
        public string Design { get; set; }

        [DefaultValue("Casual | SportsWear | Formal | Accessories")]
        public string CategoryType { get; set; }
        public string[] Sizes { get; set; }
        public decimal BasePrice { get; set; }

        [DefaultValue("Available | Discontinued")]
        public string GarmentStatus { get; set; }
        public decimal LaborHoursPerUnit { get; set; }
        public decimal HourlyLaborRate { get; set; }
    }
    public class UpdateGarmentDTO : CreateGarmentDTO
    {
        public int Id { get; set; }
    }

    public class UpdateGarmentStatusDTO
    {
        public int Id { get; set; }

        [DefaultValue("Available | Discontinued")]
        public string GarmentStatus { get; set; }
    }
    public class UpdateCategoryDTO
    {
        public int Id { get; set; }

        [DefaultValue("Casual | SportsWear | Formal | Accessories")]
        public string CategoryType { get; set; }

    }
}