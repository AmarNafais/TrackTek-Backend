using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Supplier : BaseEntity
    {
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
