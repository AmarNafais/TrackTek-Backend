using Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Machine : BaseEntity
    {
        public string Name { get; set; }
        public string MachineType { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public Status.MachineStatus MachineStatus { get; set; }
        public decimal HourlyRate { get; set; }
    }
}
