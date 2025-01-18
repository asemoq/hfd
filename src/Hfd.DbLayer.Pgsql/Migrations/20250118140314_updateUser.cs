using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hfd.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class updateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockedReason",
                table: "user");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "user");

            migrationBuilder.RenameColumn(
                name: "Iin",
                table: "user",
                newName: "UniqueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UniqueId",
                table: "user",
                newName: "Iin");

            migrationBuilder.AddColumn<string>(
                name: "BlockedReason",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "user",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
