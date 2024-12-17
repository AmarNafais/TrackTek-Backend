using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class CreateCustomerDTO
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateCustomerDTO : CreateCustomerDTO
    {
        public int Id { get; set; }
    }
}
