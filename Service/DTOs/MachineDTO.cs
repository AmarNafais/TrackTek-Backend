using Data.Entities.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service.DTOs
{
    public class CreateMachineDTO
    {
        public string Name { get; set; }
        public string MachineType { get; set; }

        [DefaultValue("Active | InActive")]
        public string MachineStatus { get; set; }
        public decimal HourlyRate { get; set; }
    }
    public class UpdateMachineDTO : CreateMachineDTO
    {
        public int Id { get; set; }
    }
}
