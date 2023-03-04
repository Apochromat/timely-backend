using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace timely_backend.Migrations
{
    /// <inheritdoc />
    public partial class crashDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_Groups_GroupId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_GroupId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Lessons");

            migrationBuilder.AddColumn<Guid>(
                name: "LessonId",
                table: "Groups",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_LessonId",
                table: "Groups",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Lessons_LessonId",
                table: "Groups",
                column: "LessonId",
                principalTable: "Lessons",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Lessons_LessonId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_LessonId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "Groups");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Lessons",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_GroupId",
                table: "Lessons",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_Groups_GroupId",
                table: "Lessons",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
