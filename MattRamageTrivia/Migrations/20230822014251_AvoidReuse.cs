using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MattRamageTrivia.Migrations
{
    /// <inheritdoc />
    public partial class AvoidReuse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Used",
                table: "Questions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Used",
                table: "Questions");
        }
    }
}
