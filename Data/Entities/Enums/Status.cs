using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities.Enums
{
    public class Status
    {
        public enum GarmentStatus
        {
            Available,
            Discontinued
        }

        public enum MachineStatus
        {
            Active,
            InActive
        }

        public enum OrderStatus
        {
            Pending,
            InProgress,
            Completed,
            Cancelled
        }

    }
}
