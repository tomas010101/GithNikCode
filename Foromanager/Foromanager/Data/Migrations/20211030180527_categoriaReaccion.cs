using Microsoft.EntityFrameworkCore.Migrations;

namespace Foromanager.Data.Migrations
{
    public partial class categoriaReaccion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reaccion_Publicacion_PublicacionID",
                table: "Reaccion");

            migrationBuilder.RenameColumn(
                name: "PublicacionID",
                table: "Reaccion",
                newName: "PublicacionId");

            migrationBuilder.RenameColumn(
                name: "ReaccionID",
                table: "Reaccion",
                newName: "ReaccionId");

            migrationBuilder.RenameIndex(
                name: "IX_Reaccion_PublicacionID",
                table: "Reaccion",
                newName: "IX_Reaccion_PublicacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reaccion_Publicacion_PublicacionId",
                table: "Reaccion",
                column: "PublicacionId",
                principalTable: "Publicacion",
                principalColumn: "PublicacionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reaccion_Publicacion_PublicacionId",
                table: "Reaccion");

            migrationBuilder.RenameColumn(
                name: "PublicacionId",
                table: "Reaccion",
                newName: "PublicacionID");

            migrationBuilder.RenameColumn(
                name: "ReaccionId",
                table: "Reaccion",
                newName: "ReaccionID");

            migrationBuilder.RenameIndex(
                name: "IX_Reaccion_PublicacionId",
                table: "Reaccion",
                newName: "IX_Reaccion_PublicacionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reaccion_Publicacion_PublicacionID",
                table: "Reaccion",
                column: "PublicacionID",
                principalTable: "Publicacion",
                principalColumn: "PublicacionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
