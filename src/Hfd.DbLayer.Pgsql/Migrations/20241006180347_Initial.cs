using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Hfd.DbLayer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    scopeId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    userUid = table.Column<string>(type: "text", nullable: false),
                    startedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    stoppedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.scopeId);
                });

            migrationBuilder.CreateTable(
                name: "file",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    FileId = table.Column<string>(type: "text", nullable: true),
                    UniqueId = table.Column<string>(type: "text", nullable: true),
                    Size = table.Column<long>(type: "bigint", nullable: true),
                    Object = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "provider_user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    SubscribedFlags = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_provider_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "training",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    FileId = table.Column<string>(type: "text", nullable: true),
                    FileType = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_training", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Iin = table.Column<string>(type: "text", nullable: true),
                    Usertype = table.Column<int>(type: "integer", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    Blocked = table.Column<bool>(type: "boolean", nullable: false),
                    BlockedReason = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Language = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
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
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                name: "nurse",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PlotNumber = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nurse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_nurse_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "psychologist",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    PlotNumber = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_psychologist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_psychologist_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "social_worker",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    PlotNumber = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_social_worker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_social_worker_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    NewValue = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "patient",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    PatientCardId = table.Column<long>(type: "bigint", nullable: false),
                    NurseId = table.Column<long>(type: "bigint", nullable: true),
                    Scale = table.Column<string>(type: "text", nullable: true),
                    Imt = table.Column<string>(type: "text", nullable: true),
                    Klas = table.Column<int>(type: "integer", nullable: false),
                    PlotNumber = table.Column<int>(type: "integer", nullable: false),
                    Dead = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patient", x => x.Id);
                    table.ForeignKey(
                        name: "FK_patient_nurse_NurseId",
                        column: x => x.NurseId,
                        principalTable: "nurse",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_patient_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hospitalization",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Address = table.Column<string>(type: "text", nullable: true),
                    DocId = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hospitalization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_hospitalization_patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "patient_card",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    Birthday = table.Column<string>(type: "text", nullable: true),
                    Region = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Diagnosis = table.Column<string>(type: "text", nullable: true),
                    Diseases = table.Column<string>(type: "text", nullable: true),
                    Height = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<string>(type: "text", nullable: true),
                    FamilyFullName = table.Column<string>(type: "text", nullable: true),
                    FamilyPhoneNumber = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "survey",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Dyspnea = table.Column<int>(type: "integer", nullable: false),
                    AdValue = table.Column<double>(type: "double precision", nullable: false),
                    WeightValue = table.Column<double>(type: "double precision", nullable: false),
                    Edema = table.Column<int>(type: "integer", nullable: false),
                    HeartRateValue = table.Column<double>(type: "double precision", nullable: false),
                    PositionInBed = table.Column<int>(type: "integer", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_survey", x => x.Id);
                    table.ForeignKey(
                        name: "FK_survey_patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "patient",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tshh",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Klass = table.Column<long>(type: "bigint", nullable: false),
                    Distance = table.Column<long>(type: "bigint", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tshh", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tshh_patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "patient",
                        principalColumn: "Id",
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

            migrationBuilder.CreateIndex(
                name: "IX_hospitalization_PatientId",
                table: "hospitalization",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_nurse_UserId",
                table: "nurse",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_patient_NurseId",
                table: "patient",
                column: "NurseId");

            migrationBuilder.CreateIndex(
                name: "IX_patient_UserId",
                table: "patient",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_patient_card_PatientId",
                table: "patient_card",
                column: "PatientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_psychologist_UserId",
                table: "psychologist",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_social_worker_UserId",
                table: "social_worker",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_survey_PatientId",
                table: "survey",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_tshh_PatientId",
                table: "tshh",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditEntryProperty");

            migrationBuilder.DropTable(
                name: "file");

            migrationBuilder.DropTable(
                name: "hospitalization");

            migrationBuilder.DropTable(
                name: "patient_card");

            migrationBuilder.DropTable(
                name: "provider_user");

            migrationBuilder.DropTable(
                name: "psychologist");

            migrationBuilder.DropTable(
                name: "social_worker");

            migrationBuilder.DropTable(
                name: "survey");

            migrationBuilder.DropTable(
                name: "training");

            migrationBuilder.DropTable(
                name: "tshh");

            migrationBuilder.DropTable(
                name: "AuditEntry");

            migrationBuilder.DropTable(
                name: "patient");

            migrationBuilder.DropTable(
                name: "Audit");

            migrationBuilder.DropTable(
                name: "nurse");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
