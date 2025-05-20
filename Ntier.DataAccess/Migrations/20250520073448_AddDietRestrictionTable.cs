using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ntier.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDietRestrictionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avoid",
                table: "MedicalConditions");

            migrationBuilder.DropColumn(
                name: "High",
                table: "MedicalConditions");

            migrationBuilder.DropColumn(
                name: "Low",
                table: "MedicalConditions");

            migrationBuilder.CreateTable(
                name: "DietRestrictions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    High = table.Column<string>(type: "text", nullable: true),
                    Low = table.Column<string>(type: "text", nullable: true),
                    Avoid = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DietRestrictions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DietRestrictions");

            migrationBuilder.AddColumn<string>(
                name: "Avoid",
                table: "MedicalConditions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "High",
                table: "MedicalConditions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Low",
                table: "MedicalConditions",
                type: "text",
                nullable: true);
        }
    }
}
