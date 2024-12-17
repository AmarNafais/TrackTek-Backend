using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class CreateSupplierDTO
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

    public class UpdateSupplierDTO : CreateSupplierDTO
    {
        public int Id { get; set; }
    }
}
