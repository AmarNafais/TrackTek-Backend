using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class added_reset_code_email_template : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Name", "Subject", "Body" },
                values: new object[]
                {
                    "PasswordReset",
                    "Password Reset Request",
                    "<p>Hello {Name},</p>" +
                    "<p>Your password reset code is: <strong>{ResetCode}</strong></p>" +
                    "<p>Please use this code to reset your password. The code is valid for a limited time only.</p>" +
                    "<p>Thank you!</p>"
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmailTemplates",
                keyColumn: "Name",
                keyValue: "PasswordReset");
        }
    }
}