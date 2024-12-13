using Data.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Order : BaseEntity
    {
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalCost { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public Status.OrderStatus OrderStatus { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? CreatedBy { get; set; }
    }
}
