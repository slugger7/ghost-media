using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ghost.Data.Migrations
{
  public partial class VideoDateAdded : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<DateTime>(
          name: "DateAdded",
          table: "Videos",
          type: "Text",
          nullable: false,
          defaultValue: DateTime.UtcNow);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "DateAdded",
          table: "Videos");
    }
  }
}
