using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace timely_backend.Migrations
{
    /// <inheritdoc />
    public partial class addDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_TimeInterval_TimeIntervalId",
                table: "Lessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeInterval",
                table: "TimeInterval");

            migrationBuilder.RenameTable(
                name: "TimeInterval",
                newName: "TimeIntervals");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeIntervals",
                table: "TimeIntervals",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Domains",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Domains", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_TimeIntervals_TimeIntervalId",
                table: "Lessons",
                column: "TimeIntervalId",
                principalTable: "TimeIntervals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_TimeIntervals_TimeIntervalId",
                table: "Lessons");

            migrationBuilder.DropTable(
                name: "Domains");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeIntervals",
                table: "TimeIntervals");

            migrationBuilder.RenameTable(
                name: "TimeIntervals",
                newName: "TimeInterval");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeInterval",
                table: "TimeInterval",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_TimeInterval_TimeIntervalId",
                table: "Lessons",
                column: "TimeIntervalId",
                principalTable: "TimeInterval",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
