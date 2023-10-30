using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MattRamageTrivia.Migrations
{
    /// <inheritdoc />
    public partial class AvoidQuestionReuse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AskedThisRound",
                table: "Questions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AskedThisRound",
                table: "Questions");
        }
    }
}
