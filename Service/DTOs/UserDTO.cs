using Data.Entities.Enums;
using System.ComponentModel;

namespace Service.DTOs
{
    public class CreateUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        [DefaultValue("SuperAdmin | Manager | Inventory Manager | Staff")]
        public string Role { get; set; }

        public bool IsAdmin { get; set; }
    }

    public class UpdateUserDTO : CreateUserDTO
    {
        public int Id { get; set; }
    }
}
