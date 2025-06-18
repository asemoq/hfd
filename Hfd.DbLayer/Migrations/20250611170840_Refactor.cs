using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hfd.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditEntryProperty");

            migrationBuilder.DropTable(
                name: "provider_user");

            migrationBuilder.DropTable(
                name: "AuditEntry");

            migrationBuilder.DropTable(
                name: "Audit");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "tshh",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "hospitalization",
                newName: "CreationDate");

            migrationBuilder.RenameColumn(
                name: "Object",
                table: "file",
                newName: "Data");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "user",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "user",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "training",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "training",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileId",
                table: "training",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "nurse",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DocId",
                table: "hospitalization",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "hospitalization",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UniqueId",
                table: "file",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FileId",
                table: "file",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "tshh",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "hospitalization",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "file",
                newName: "Object");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "user",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "user",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647);

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "training",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "training",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647);

            migrationBuilder.AlterColumn<string>(
                name: "FileId",
                table: "training",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "nurse",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647);

            migrationBuilder.AlterColumn<string>(
                name: "DocId",
                table: "hospitalization",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "hospitalization",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647);

            migrationBuilder.AlterColumn<string>(
                name: "UniqueId",
                table: "file",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647);

            migrationBuilder.AlterColumn<string>(
                name: "FileId",
                table: "file",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647);

            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    scopeId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    startedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    stoppedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    userUid = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.scopeId);
                });

            migrationBuilder.CreateTable(
                name: "provider_user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    SubscribedFlags = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditEntry",
                columns: table => new
                {
                    AuditEntryID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntitySetName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    EntityTypeName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    State = table.Column<int>(type: "integer", nullable: false),
                    StateName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedBy = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntry", x => x.AuditEntryID);
                    table.ForeignKey(
                        name: "FK_AuditEntry_Audit_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Audit",
                        principalColumn: "scopeId");
                });

            migrationBuilder.CreateTable(
                name: "AuditEntryProperty",
                columns: table => new
                {
                    AuditEntryPropertyID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AuditEntryID = table.Column<int>(type: "integer", nullable: false),
                    RelationName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PropertyName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    OldValue = table.Column<string>(type: "text", nullable: true),
                    NewValue = table.Column<string>(type: "text", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntryProperty", x => x.AuditEntryPropertyID);
                    table.ForeignKey(
                        name: "FK_AuditEntryProperty_AuditEntry_AuditEntryID",
                        column: x => x.AuditEntryID,
                        principalTable: "AuditEntry",
                        principalColumn: "AuditEntryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntry_CreatedBy",
                table: "AuditEntry",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntryProperty_AuditEntryID",
                table: "AuditEntryProperty",
                column: "AuditEntryID");
        }
    }
}
