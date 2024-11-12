using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class added_super_admin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "FirstName", "LastName", "Email", "Password", "Role" , "CreatedTime", "UpdatedTime", "UpdatedById"},
                values: new object[] { "Super", "Admin", "admin@mail.com", "AQAAAAIAAYagAAAAEFtqRiqot29ExL819wCQGcQBEBZSb0u7GIHGl5KoNa/dAwrSTLkmDZ+G/NvIjviVog==", "SuperAdmin" , "2024-10-28 14:44:37.6485967", "2024-10-28 14:44:37.6485967", 1}
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@mail.com"
            );
        }
    }
}