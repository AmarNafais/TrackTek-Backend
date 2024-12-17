using Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class CreateGarmentMachineDTO
    {
        public int GarmentId { get; set; }
        
        public int MachineId { get; set; }
        public decimal HoursRequired { get; set; }
    }
    public class UpdateGarmentMachineDTO : CreateGarmentMachineDTO
    {
        public int Id { get; set; }
    }
}
