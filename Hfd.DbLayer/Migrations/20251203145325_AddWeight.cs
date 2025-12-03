using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hfd.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddWeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "weight",
                table: "survey",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "weight",
                table: "survey");
        }
    }
}
