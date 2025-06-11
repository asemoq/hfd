using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hfd.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "patient_card");

            migrationBuilder.DropColumn(
                name: "PatientCardId",
                table: "patient");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Birthday",
                table: "patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Diagnosis",
                table: "patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Diseases",
                table: "patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FamilyFullName",
                table: "patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FamilyPhoneNumber",
                table: "patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Height",
                table: "patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "patient",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Weight",
                table: "patient",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "Diagnosis",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "Diseases",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "FamilyFullName",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "FamilyPhoneNumber",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "patient");

            migrationBuilder.AddColumn<long>(
                name: "PatientCardId",
                table: "patient",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "patient_card",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Birthday = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    Diagnosis = table.Column<string>(type: "text", nullable: true),
                    Diseases = table.Column<string>(type: "text", nullable: true),
                    FamilyFullName = table.Column<string>(type: "text", nullable: true),
                    FamilyPhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    Height = table.Column<string>(type: "text", nullable: true),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Region = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Weight = table.Column<string>(type: "text", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patient_card", x => x.Id);
                    table.ForeignKey(
                        name: "FK_patient_card_patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_patient_card_PatientId",
                table: "patient_card",
                column: "PatientId",
                unique: true);
        }
    }
}
