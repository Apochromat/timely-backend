using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace timely_backend.Migrations
{
    /// <inheritdoc />
    public partial class deCrashDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "GroupLesson",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LessonsId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupLesson", x => new { x.GroupId, x.LessonsId });
                    table.ForeignKey(
                        name: "FK_GroupLesson_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupLesson_Lessons_LessonsId",
                        column: x => x.LessonsId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GroupLesson_LessonsId",
                table: "GroupLesson",
                column: "LessonsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupLesson");

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
    }
}
