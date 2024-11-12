using Data.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
    }
}