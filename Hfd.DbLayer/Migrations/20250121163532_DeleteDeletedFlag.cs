using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hfd.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class DeleteDeletedFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "user");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "tshh");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "training");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "survey");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "social_worker");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "psychologist");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "provider_user");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "nurse");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "hospitalization");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "file");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "tshh",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "training",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "survey",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "social_worker",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "psychologist",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "provider_user",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "patient",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "nurse",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "hospitalization",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "file",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
