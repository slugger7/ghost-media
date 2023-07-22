using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ghost.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConvertVideoWithScale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "ConvertJobs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "ConvertJobs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "ConvertJobs");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "ConvertJobs");
        }
    }
}
