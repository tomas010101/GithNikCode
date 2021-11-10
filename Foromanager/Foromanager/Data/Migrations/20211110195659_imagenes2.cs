using Microsoft.EntityFrameworkCore.Migrations;

namespace Foromanager.Data.Migrations
{
    public partial class imagenes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imagenes_Publicacion_PublicacionID",
                table: "Imagenes");

            migrationBuilder.RenameColumn(
                name: "PublicacionID",
                table: "Imagenes",
                newName: "PublicacionId");

            migrationBuilder.RenameColumn(
                name: "ImagenesID",
                table: "Imagenes",
                newName: "ImagenesId");

            migrationBuilder.RenameIndex(
                name: "IX_Imagenes_PublicacionID",
                table: "Imagenes",
                newName: "IX_Imagenes_PublicacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Imagenes_Publicacion_PublicacionId",
                table: "Imagenes",
                column: "PublicacionId",
                principalTable: "Publicacion",
                principalColumn: "PublicacionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imagenes_Publicacion_PublicacionId",
                table: "Imagenes");

            migrationBuilder.RenameColumn(
                name: "PublicacionId",
                table: "Imagenes",
                newName: "PublicacionID");

            migrationBuilder.RenameColumn(
                name: "ImagenesId",
                table: "Imagenes",
                newName: "ImagenesID");

            migrationBuilder.RenameIndex(
                name: "IX_Imagenes_PublicacionId",
                table: "Imagenes",
                newName: "IX_Imagenes_PublicacionID");

            migrationBuilder.AddForeignKey(
                name: "FK_Imagenes_Publicacion_PublicacionID",
                table: "Imagenes",
                column: "PublicacionID",
                principalTable: "Publicacion",
                principalColumn: "PublicacionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
