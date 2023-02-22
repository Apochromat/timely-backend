using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace timely_backend.Migrations
{
    /// <inheritdoc />
    public partial class addChainId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChainId",
                table: "Lessons",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChainId",
                table: "Lessons");
        }
    }
}
