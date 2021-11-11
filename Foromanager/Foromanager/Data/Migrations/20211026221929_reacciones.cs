using Microsoft.EntityFrameworkCore.Migrations;

namespace Foromanager.Data.Migrations
{
    public partial class reacciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reaccion",
                table: "Publicacion");

            migrationBuilder.CreateTable(
                name: "Reaccion",
                columns: table => new
                {
                    ReaccionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicacionID = table.Column<int>(type: "int", nullable: false),
                    Like = table.Column<bool>(type: "bit", nullable: false),
                    DisLike = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reaccion", x => x.ReaccionID);
                    table.ForeignKey(
                        name: "FK_Reaccion_Publicacion_PublicacionID",
                        column: x => x.PublicacionID,
                        principalTable: "Publicacion",
                        principalColumn: "PublicacionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reaccion_PublicacionID",
                table: "Reaccion",
                column: "PublicacionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reaccion");

            migrationBuilder.AddColumn<int>(
                name: "Reaccion",
                table: "Publicacion",
                type: "int",
                nullable: true);
        }
    }
}
