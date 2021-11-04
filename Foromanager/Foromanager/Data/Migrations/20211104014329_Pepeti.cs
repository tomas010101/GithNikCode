using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Foromanager.Data.Migrations
{
    public partial class Pepeti : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categoria_Foro_ForoId",
                table: "Categoria");

            migrationBuilder.DropIndex(
                name: "IX_Categoria_ForoId",
                table: "Categoria");

            migrationBuilder.AddColumn<byte[]>(
                name: "ForoPerfil",
                table: "Foro",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ForoCategoria",
                columns: table => new
                {
                    CategoriasCategoriaId = table.Column<int>(type: "int", nullable: false),
                    ForosForoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForoCategoria", x => new { x.CategoriasCategoriaId, x.ForosForoId });
                    table.ForeignKey(
                        name: "FK_ForoCategoria_Categoria_CategoriasCategoriaId",
                        column: x => x.CategoriasCategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForoCategoria_Foro_ForosForoId",
                        column: x => x.ForosForoId,
                        principalTable: "Foro",
                        principalColumn: "ForoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Imagenes",
                columns: table => new
                {
                    ImagenesID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagenNombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PublicacionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagenes", x => x.ImagenesID);
                    table.ForeignKey(
                        name: "FK_Imagenes_Publicacion_PublicacionID",
                        column: x => x.PublicacionID,
                        principalTable: "Publicacion",
                        principalColumn: "PublicacionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForoCategoria_ForosForoId",
                table: "ForoCategoria",
                column: "ForosForoId");

            migrationBuilder.CreateIndex(
                name: "IX_Imagenes_PublicacionID",
                table: "Imagenes",
                column: "PublicacionID",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForoCategoria");

            migrationBuilder.DropTable(
                name: "Imagenes");

            migrationBuilder.DropColumn(
                name: "ForoPerfil",
                table: "Foro");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_ForoId",
                table: "Categoria",
                column: "ForoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categoria_Foro_ForoId",
                table: "Categoria",
                column: "ForoId",
                principalTable: "Foro",
                principalColumn: "ForoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
