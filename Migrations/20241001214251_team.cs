using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScoreboardAPI.Migrations
{
    /// <inheritdoc />
    public partial class team : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Teams",
                newName: "Validity");

            migrationBuilder.AddColumn<int>(
                name: "Creativity",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Impact",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Presentation",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Relevance",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalScore",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creativity",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Impact",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Presentation",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Relevance",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TotalScore",
                table: "Teams");

            migrationBuilder.RenameColumn(
                name: "Validity",
                table: "Teams",
                newName: "Score");
        }
    }
}
