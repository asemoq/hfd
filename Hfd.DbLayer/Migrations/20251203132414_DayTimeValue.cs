using System.Data.Common;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;

#nullable disable

namespace Hfd.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class DayTimeValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "day_time_value",
                table: "survey",
                type: "text",
                nullable: false,
                defaultValue: "");
            migrationBuilder.Sql(@"UPDATE survey SET day_time_value = to_char(date_time,'AM');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "day_time_value",
                table: "survey");
        }
    }
}
