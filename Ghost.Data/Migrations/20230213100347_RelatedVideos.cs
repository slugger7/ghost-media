using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ghost.Data.Migrations
{
    public partial class RelatedVideos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RelatedVideos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VideoId = table.Column<int>(type: "INTEGER", nullable: false),
                    RelatedToId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatedVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RelatedVideos_Videos_RelatedToId",
                        column: x => x.RelatedToId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RelatedVideos_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RelatedVideos_RelatedToId",
                table: "RelatedVideos",
                column: "RelatedToId");

            migrationBuilder.CreateIndex(
                name: "IX_RelatedVideos_VideoId",
                table: "RelatedVideos",
                column: "VideoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RelatedVideos");
        }
    }
}
