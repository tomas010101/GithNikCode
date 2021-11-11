using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Foromanager.Data.Migrations
{
    public partial class iniciar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categoria_Foro_ForoId",
                table: "Categoria");

            migrationBuilder.DropForeignKey(
                name: "FK_Reaccion_Publicacion_PublicacionId",
                table: "Reaccion");

            migrationBuilder.DropIndex(
                name: "IX_Reaccion_PublicacionId",
                table: "Reaccion");

            migrationBuilder.DropIndex(
                name: "IX_Categoria_ForoId",
                table: "Categoria");

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Publicacion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Publicacion",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FotodePerfil",
                table: "AspNetUsers",
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

            migrationBuilder.CreateTable(
                name: "PublicacionReaccion",
                columns: table => new
                {
                    PublicacionesPublicacionId = table.Column<int>(type: "int", nullable: false),
                    ReaccionesReaccionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicacionReaccion", x => new { x.PublicacionesPublicacionId, x.ReaccionesReaccionId });
                    table.ForeignKey(
                        name: "FK_PublicacionReaccion_Publicacion_PublicacionesPublicacionId",
                        column: x => x.PublicacionesPublicacionId,
                        principalTable: "Publicacion",
                        principalColumn: "PublicacionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicacionReaccion_Reaccion_ReaccionesReaccionId",
                        column: x => x.ReaccionesReaccionId,
                        principalTable: "Reaccion",
                        principalColumn: "ReaccionId",
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

            migrationBuilder.CreateIndex(
                name: "IX_PublicacionReaccion_ReaccionesReaccionId",
                table: "PublicacionReaccion",
                column: "ReaccionesReaccionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForoCategoria");

            migrationBuilder.DropTable(
                name: "Imagenes");

            migrationBuilder.DropTable(
                name: "PublicacionReaccion");

            migrationBuilder.DropColumn(
                name: "FotodePerfil",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Titulo",
                table: "Publicacion",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Publicacion",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Reaccion_PublicacionId",
                table: "Reaccion",
                column: "PublicacionId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Reaccion_Publicacion_PublicacionId",
                table: "Reaccion",
                column: "PublicacionId",
                principalTable: "Publicacion",
                principalColumn: "PublicacionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
