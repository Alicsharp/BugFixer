using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugFixer.infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangePropName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EmailSettings",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.RenameColumn(
                name: "UserIP",
                table: "QuestionViews",
                newName: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "QuestionViews",
                newName: "UserIP");

            migrationBuilder.InsertData(
                table: "EmailSettings",
                columns: new[] { "Id", "CreateDate", "DisplayName", "EnableSSL", "From", "IsDefault", "IsDeleted", "Password", "Port", "SMTP" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BugFixer", true, "bugfixer.toplearn@gmail.com", true, false, "strong@password", 587, "smtp.gmail.com" });
        }
    }
}
