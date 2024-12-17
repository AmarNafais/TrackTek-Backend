using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs
{
    public class CreateNotificationDTO
    {
        public string Message { get; set; }
        public DateTime SentDate { get; set; }
    }

    public class UpdateNotificationDTO : CreateNotificationDTO
    {
        public int Id { get; set; }
    }
}
