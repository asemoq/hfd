using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hfd.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class DeletePhoneNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "full_name",
                table: "social_worker");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "social_worker");

            migrationBuilder.DropColumn(
                name: "full_name",
                table: "psychologist");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "psychologist");

            migrationBuilder.DropColumn(
                name: "full_name",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "full_name",
                table: "nurse");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "nurse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "full_name",
                table: "social_worker",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "social_worker",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "full_name",
                table: "psychologist",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "psychologist",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "full_name",
                table: "patient",
                type: "text",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "patient",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "full_name",
                table: "nurse",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "nurse",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: string.Empty);
        }
    }
}
