using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hfd.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class DeleteColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "birthday",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "gender",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "height",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "imt",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "weight",
                table: "patient");

            migrationBuilder.AlterColumn<string>(
                name: "family_phone_number",
                table: "patient",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "family_full_name",
                table: "patient",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "family_phone_number",
                table: "patient",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "family_full_name",
                table: "patient",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "birthday",
                table: "patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gender",
                table: "patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "height",
                table: "patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "imt",
                table: "patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "weight",
                table: "patient",
                type: "text",
                nullable: true);
        }
    }
}
