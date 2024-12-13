using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class GarmentMachine : BaseEntity
    {
        [ForeignKey("Garment")]
        public int GarmentId { get; set; }
        public Garment Garment { get; set; }

        [ForeignKey("Machine")]
        public int MachineId { get; set; }
        public Machine Machine { get; set; }
        public decimal HoursRequired { get; set; }
    }
}
