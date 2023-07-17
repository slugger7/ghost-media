using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ghost.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConversionOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConstantRateFactor",
                table: "ConvertJobs",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VariableBitrate",
                table: "ConvertJobs",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConstantRateFactor",
                table: "ConvertJobs");

            migrationBuilder.DropColumn(
                name: "VariableBitrate",
                table: "ConvertJobs");
        }
    }
}
