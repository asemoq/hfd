using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hfd.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserIdUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_social_worker_user_id",
                table: "social_worker",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_psychologist_user_id",
                table: "psychologist",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_patient_user_id",
                table: "patient",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_nurse_user_id",
                table: "nurse",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_social_worker_user_id",
                table: "social_worker");

            migrationBuilder.DropIndex(
                name: "IX_psychologist_user_id",
                table: "psychologist");

            migrationBuilder.DropIndex(
                name: "IX_patient_user_id",
                table: "patient");

            migrationBuilder.DropIndex(
                name: "IX_nurse_user_id",
                table: "nurse");
        }
    }
}
