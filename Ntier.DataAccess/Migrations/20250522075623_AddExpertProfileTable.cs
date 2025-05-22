using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ntier.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddExpertProfileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpertProfileId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExpertProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CertImage = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpertProfiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExpertProfileId",
                table: "Users",
                column: "ExpertProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ExpertProfiles_ExpertProfileId",
                table: "Users",
                column: "ExpertProfileId",
                principalTable: "ExpertProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_ExpertProfiles_ExpertProfileId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "ExpertProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Users_ExpertProfileId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExpertProfileId",
                table: "Users");
        }
    }
}
