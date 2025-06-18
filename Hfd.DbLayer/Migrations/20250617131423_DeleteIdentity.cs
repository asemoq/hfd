using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hfd.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class DeleteIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_nurse_user_UserId",
                table: "nurse");

            migrationBuilder.DropForeignKey(
                name: "FK_patient_user_UserId",
                table: "patient");

            migrationBuilder.DropForeignKey(
                name: "FK_psychologist_user_UserId",
                table: "psychologist");

            migrationBuilder.DropForeignKey(
                name: "FK_social_worker_user_UserId",
                table: "social_worker");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropIndex(
                name: "IX_social_worker_UserId",
                table: "social_worker");

            migrationBuilder.DropIndex(
                name: "IX_psychologist_UserId",
                table: "psychologist");

            migrationBuilder.DropIndex(
                name: "IX_patient_UserId",
                table: "patient");

            migrationBuilder.DropIndex(
                name: "IX_nurse_UserId",
                table: "nurse");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "tshh",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "tshh",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "tshh",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "training",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "training",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "training",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "survey",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "survey",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "survey",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "social_worker",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "social_worker",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "social_worker",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PlotNumber",
                table: "social_worker",
                newName: "plot_number");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "social_worker",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "social_worker",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "social_worker",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "psychologist",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "psychologist",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "psychologist",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PlotNumber",
                table: "psychologist",
                newName: "plot_number");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "psychologist",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "psychologist",
                newName: "full_name");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "psychologist",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "patient",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "patient",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "patient",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PlotNumber",
                table: "patient",
                newName: "plot_number");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "patient",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "patient",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "nurse",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "nurse",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "nurse",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PlotNumber",
                table: "nurse",
                newName: "plot_number");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "nurse",
                newName: "phone_number");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "nurse",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "hospitalization",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "hospitalization",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "hospitalization",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "file",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "file",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "file",
                newName: "created_at");

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "social_worker",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "social_worker",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "social_worker",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "psychologist",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "psychologist",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "full_name",
                table: "psychologist",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "patient",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "patient",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "full_name",
                table: "patient",
                type: "text",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AlterColumn<string>(
                name: "user_id",
                table: "nurse",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "phone_number",
                table: "nurse",
                type: "character varying(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647);

            migrationBuilder.AddColumn<string>(
                name: "full_name",
                table: "nurse",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: string.Empty);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "full_name",
                table: "patient");

            migrationBuilder.DropColumn(
                name: "full_name",
                table: "nurse");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "tshh",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "tshh",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "tshh",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "training",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "training",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "training",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "survey",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "survey",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "survey",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "social_worker",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "social_worker",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "social_worker",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "plot_number",
                table: "social_worker",
                newName: "PlotNumber");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "social_worker",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "social_worker",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "social_worker",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "psychologist",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "psychologist",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "psychologist",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "plot_number",
                table: "psychologist",
                newName: "PlotNumber");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "psychologist",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "full_name",
                table: "psychologist",
                newName: "FullName");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "psychologist",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "patient",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "patient",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "patient",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "plot_number",
                table: "patient",
                newName: "PlotNumber");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "patient",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "patient",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "nurse",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "nurse",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "nurse",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "plot_number",
                table: "nurse",
                newName: "PlotNumber");

            migrationBuilder.RenameColumn(
                name: "phone_number",
                table: "nurse",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "nurse",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "hospitalization",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "hospitalization",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "hospitalization",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "file",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "file",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "file",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "social_worker",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "social_worker",
                type: "text",
                maxLength: 2147483647,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "social_worker",
                type: "text",
                maxLength: 2147483647,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647);

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "psychologist",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "psychologist",
                type: "text",
                maxLength: 2147483647,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "psychologist",
                type: "text",
                maxLength: 2147483647,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647);

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "patient",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "patient",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "nurse",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(36)",
                oldMaxLength: 36);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "nurse",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(15)",
                oldMaxLength: 15);

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Blocked = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FullName = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    Language = table.Column<int>(type: "integer", nullable: false),
                    Password = table.Column<string>(type: "text", maxLength: 2147483647, nullable: false),
                    UniqueId = table.Column<string>(type: "text", maxLength: 2147483647, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Username = table.Column<string>(type: "text", maxLength: 2147483647, nullable: false),
                    Usertype = table.Column<int>(type: "integer", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_social_worker_UserId",
                table: "social_worker",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_psychologist_UserId",
                table: "psychologist",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_patient_UserId",
                table: "patient",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_nurse_UserId",
                table: "nurse",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_nurse_user_UserId",
                table: "nurse",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_patient_user_UserId",
                table: "patient",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_psychologist_user_UserId",
                table: "psychologist",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_social_worker_user_UserId",
                table: "social_worker",
                column: "UserId",
                principalTable: "user",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
