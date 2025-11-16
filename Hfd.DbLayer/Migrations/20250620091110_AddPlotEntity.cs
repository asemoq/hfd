using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hfd.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class AddPlotEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hospitalization_patient_PatientId",
                table: "hospitalization");

            migrationBuilder.DropForeignKey(
                name: "FK_patient_nurse_NurseId",
                table: "patient");

            migrationBuilder.DropForeignKey(
                name: "FK_survey_patient_PatientId",
                table: "survey");

            migrationBuilder.DropForeignKey(
                name: "FK_tshh_patient_PatientId",
                table: "tshh");

            migrationBuilder.DropColumn(
                name: "plot_number",
                table: "patient");

            migrationBuilder.RenameColumn(
                name: "Klass",
                table: "tshh",
                newName: "klass");

            migrationBuilder.RenameColumn(
                name: "Distance",
                table: "tshh",
                newName: "distance");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "tshh",
                newName: "patient_id");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "tshh",
                newName: "creation_date");

            migrationBuilder.RenameIndex(
                name: "IX_tshh_PatientId",
                table: "tshh",
                newName: "IX_tshh_patient_id");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "training",
                newName: "text");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "training",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "FileType",
                table: "training",
                newName: "file_type");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "training",
                newName: "file_id");

            migrationBuilder.RenameColumn(
                name: "Edema",
                table: "survey",
                newName: "edema");

            migrationBuilder.RenameColumn(
                name: "Dyspnea",
                table: "survey",
                newName: "dyspnea");

            migrationBuilder.RenameColumn(
                name: "WeightValue",
                table: "survey",
                newName: "weight_value");

            migrationBuilder.RenameColumn(
                name: "PositionInBed",
                table: "survey",
                newName: "position_in_bed");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "survey",
                newName: "patient_id");

            migrationBuilder.RenameColumn(
                name: "HeartRateValue",
                table: "survey",
                newName: "heart_rate_value");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "survey",
                newName: "date_time");

            migrationBuilder.RenameColumn(
                name: "AdValue",
                table: "survey",
                newName: "ad_value");

            migrationBuilder.RenameIndex(
                name: "IX_survey_PatientId",
                table: "survey",
                newName: "IX_survey_patient_id");

            migrationBuilder.RenameColumn(
                name: "plot_number",
                table: "social_worker",
                newName: "plot_id");

            migrationBuilder.RenameColumn(
                name: "plot_number",
                table: "psychologist",
                newName: "plot_id");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "patient",
                newName: "weight");

            migrationBuilder.RenameColumn(
                name: "Scale",
                table: "patient",
                newName: "scale");

            migrationBuilder.RenameColumn(
                name: "Region",
                table: "patient",
                newName: "region");

            migrationBuilder.RenameColumn(
                name: "Klas",
                table: "patient",
                newName: "klas");

            migrationBuilder.RenameColumn(
                name: "Imt",
                table: "patient",
                newName: "imt");

            migrationBuilder.RenameColumn(
                name: "Height",
                table: "patient",
                newName: "height");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "patient",
                newName: "gender");

            migrationBuilder.RenameColumn(
                name: "Diseases",
                table: "patient",
                newName: "diseases");

            migrationBuilder.RenameColumn(
                name: "Diagnosis",
                table: "patient",
                newName: "diagnosis");

            migrationBuilder.RenameColumn(
                name: "Dead",
                table: "patient",
                newName: "dead");

            migrationBuilder.RenameColumn(
                name: "Birthday",
                table: "patient",
                newName: "birthday");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "patient",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "FamilyPhoneNumber",
                table: "patient",
                newName: "family_phone_number");

            migrationBuilder.RenameColumn(
                name: "FamilyFullName",
                table: "patient",
                newName: "family_full_name");

            migrationBuilder.RenameColumn(
                name: "NurseId",
                table: "patient",
                newName: "plot_id");

            migrationBuilder.RenameIndex(
                name: "IX_patient_NurseId",
                table: "patient",
                newName: "IX_patient_plot_id");

            migrationBuilder.RenameColumn(
                name: "plot_number",
                table: "nurse",
                newName: "plot_id");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "hospitalization",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "hospitalization",
                newName: "patient_id");

            migrationBuilder.RenameColumn(
                name: "DocId",
                table: "hospitalization",
                newName: "doc_id");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "hospitalization",
                newName: "creation_date");

            migrationBuilder.RenameIndex(
                name: "IX_hospitalization_PatientId",
                table: "hospitalization",
                newName: "IX_hospitalization_patient_id");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "file",
                newName: "size");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "file",
                newName: "data");

            migrationBuilder.RenameColumn(
                name: "UniqueId",
                table: "file",
                newName: "unique_id");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "file",
                newName: "message_id");

            migrationBuilder.RenameColumn(
                name: "FileId",
                table: "file",
                newName: "file_id");

            migrationBuilder.CreateTable(
                name: "plot",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    plot_number = table.Column<int>(type: "integer", nullable: false),
                    area = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plot", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_social_worker_plot_id",
                table: "social_worker",
                column: "plot_id");

            migrationBuilder.CreateIndex(
                name: "IX_psychologist_plot_id",
                table: "psychologist",
                column: "plot_id");

            migrationBuilder.CreateIndex(
                name: "IX_nurse_plot_id",
                table: "nurse",
                column: "plot_id");

            migrationBuilder.AddForeignKey(
                name: "FK_hospitalization_patient_patient_id",
                table: "hospitalization",
                column: "patient_id",
                principalTable: "patient",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_nurse_plot_plot_id",
                table: "nurse",
                column: "plot_id",
                principalTable: "plot",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_patient_plot_plot_id",
                table: "patient",
                column: "plot_id",
                principalTable: "plot",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_psychologist_plot_plot_id",
                table: "psychologist",
                column: "plot_id",
                principalTable: "plot",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_social_worker_plot_plot_id",
                table: "social_worker",
                column: "plot_id",
                principalTable: "plot",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_survey_patient_patient_id",
                table: "survey",
                column: "patient_id",
                principalTable: "patient",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tshh_patient_patient_id",
                table: "tshh",
                column: "patient_id",
                principalTable: "patient",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hospitalization_patient_patient_id",
                table: "hospitalization");

            migrationBuilder.DropForeignKey(
                name: "FK_nurse_plot_plot_id",
                table: "nurse");

            migrationBuilder.DropForeignKey(
                name: "FK_patient_plot_plot_id",
                table: "patient");

            migrationBuilder.DropForeignKey(
                name: "FK_psychologist_plot_plot_id",
                table: "psychologist");

            migrationBuilder.DropForeignKey(
                name: "FK_social_worker_plot_plot_id",
                table: "social_worker");

            migrationBuilder.DropForeignKey(
                name: "FK_survey_patient_patient_id",
                table: "survey");

            migrationBuilder.DropForeignKey(
                name: "FK_tshh_patient_patient_id",
                table: "tshh");

            migrationBuilder.DropTable(
                name: "plot");

            migrationBuilder.DropIndex(
                name: "IX_social_worker_plot_id",
                table: "social_worker");

            migrationBuilder.DropIndex(
                name: "IX_psychologist_plot_id",
                table: "psychologist");

            migrationBuilder.DropIndex(
                name: "IX_nurse_plot_id",
                table: "nurse");

            migrationBuilder.RenameColumn(
                name: "klass",
                table: "tshh",
                newName: "Klass");

            migrationBuilder.RenameColumn(
                name: "distance",
                table: "tshh",
                newName: "Distance");

            migrationBuilder.RenameColumn(
                name: "patient_id",
                table: "tshh",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "creation_date",
                table: "tshh",
                newName: "CreationDate");

            migrationBuilder.RenameIndex(
                name: "IX_tshh_patient_id",
                table: "tshh",
                newName: "IX_tshh_PatientId");

            migrationBuilder.RenameColumn(
                name: "text",
                table: "training",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "training",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "file_type",
                table: "training",
                newName: "FileType");

            migrationBuilder.RenameColumn(
                name: "file_id",
                table: "training",
                newName: "FileId");

            migrationBuilder.RenameColumn(
                name: "edema",
                table: "survey",
                newName: "Edema");

            migrationBuilder.RenameColumn(
                name: "dyspnea",
                table: "survey",
                newName: "Dyspnea");

            migrationBuilder.RenameColumn(
                name: "weight_value",
                table: "survey",
                newName: "WeightValue");

            migrationBuilder.RenameColumn(
                name: "position_in_bed",
                table: "survey",
                newName: "PositionInBed");

            migrationBuilder.RenameColumn(
                name: "patient_id",
                table: "survey",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "heart_rate_value",
                table: "survey",
                newName: "HeartRateValue");

            migrationBuilder.RenameColumn(
                name: "date_time",
                table: "survey",
                newName: "DateTime");

            migrationBuilder.RenameColumn(
                name: "ad_value",
                table: "survey",
                newName: "AdValue");

            migrationBuilder.RenameIndex(
                name: "IX_survey_patient_id",
                table: "survey",
                newName: "IX_survey_PatientId");

            migrationBuilder.RenameColumn(
                name: "plot_id",
                table: "social_worker",
                newName: "plot_number");

            migrationBuilder.RenameColumn(
                name: "plot_id",
                table: "psychologist",
                newName: "plot_number");

            migrationBuilder.RenameColumn(
                name: "weight",
                table: "patient",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "scale",
                table: "patient",
                newName: "Scale");

            migrationBuilder.RenameColumn(
                name: "region",
                table: "patient",
                newName: "Region");

            migrationBuilder.RenameColumn(
                name: "klas",
                table: "patient",
                newName: "Klas");

            migrationBuilder.RenameColumn(
                name: "imt",
                table: "patient",
                newName: "Imt");

            migrationBuilder.RenameColumn(
                name: "height",
                table: "patient",
                newName: "Height");

            migrationBuilder.RenameColumn(
                name: "gender",
                table: "patient",
                newName: "Gender");

            migrationBuilder.RenameColumn(
                name: "diseases",
                table: "patient",
                newName: "Diseases");

            migrationBuilder.RenameColumn(
                name: "diagnosis",
                table: "patient",
                newName: "Diagnosis");

            migrationBuilder.RenameColumn(
                name: "dead",
                table: "patient",
                newName: "Dead");

            migrationBuilder.RenameColumn(
                name: "birthday",
                table: "patient",
                newName: "Birthday");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "patient",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "family_phone_number",
                table: "patient",
                newName: "FamilyPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "family_full_name",
                table: "patient",
                newName: "FamilyFullName");

            migrationBuilder.RenameColumn(
                name: "plot_id",
                table: "patient",
                newName: "NurseId");

            migrationBuilder.RenameIndex(
                name: "IX_patient_plot_id",
                table: "patient",
                newName: "IX_patient_NurseId");

            migrationBuilder.RenameColumn(
                name: "plot_id",
                table: "nurse",
                newName: "plot_number");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "hospitalization",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "patient_id",
                table: "hospitalization",
                newName: "PatientId");

            migrationBuilder.RenameColumn(
                name: "doc_id",
                table: "hospitalization",
                newName: "DocId");

            migrationBuilder.RenameColumn(
                name: "creation_date",
                table: "hospitalization",
                newName: "CreationDate");

            migrationBuilder.RenameIndex(
                name: "IX_hospitalization_patient_id",
                table: "hospitalization",
                newName: "IX_hospitalization_PatientId");

            migrationBuilder.RenameColumn(
                name: "size",
                table: "file",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "data",
                table: "file",
                newName: "Data");

            migrationBuilder.RenameColumn(
                name: "unique_id",
                table: "file",
                newName: "UniqueId");

            migrationBuilder.RenameColumn(
                name: "message_id",
                table: "file",
                newName: "MessageId");

            migrationBuilder.RenameColumn(
                name: "file_id",
                table: "file",
                newName: "FileId");

            migrationBuilder.AddColumn<int>(
                name: "plot_number",
                table: "patient",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_hospitalization_patient_PatientId",
                table: "hospitalization",
                column: "PatientId",
                principalTable: "patient",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_patient_nurse_NurseId",
                table: "patient",
                column: "NurseId",
                principalTable: "nurse",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_survey_patient_PatientId",
                table: "survey",
                column: "PatientId",
                principalTable: "patient",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tshh_patient_PatientId",
                table: "tshh",
                column: "PatientId",
                principalTable: "patient",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
