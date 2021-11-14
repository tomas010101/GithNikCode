using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Foromanager.Data.Migrations
{
    public partial class dalenene : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "FotodePerfil",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaNombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "Foro",
                columns: table => new
                {
                    ForoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ForoPerfil = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foro", x => x.ForoId);
                    table.ForeignKey(
                        name: "FK_Foro_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reaccion",
                columns: table => new
                {
                    ReaccionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicacionId = table.Column<int>(type: "int", nullable: false),
                    Like = table.Column<bool>(type: "bit", nullable: false),
                    DisLike = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reaccion", x => x.ReaccionId);
                });

            migrationBuilder.CreateTable(
                name: "CategoriaForo",
                columns: table => new
                {
                    CategoriasCategoriaId = table.Column<int>(type: "int", nullable: false),
                    ForosForoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaForo", x => new { x.CategoriasCategoriaId, x.ForosForoId });
                    table.ForeignKey(
                        name: "FK_CategoriaForo_Categoria_CategoriasCategoriaId",
                        column: x => x.CategoriasCategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriaForo_Foro_ForosForoId",
                        column: x => x.ForosForoId,
                        principalTable: "Foro",
                        principalColumn: "ForoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Publicacion",
                columns: table => new
                {
                    PublicacionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForoId = table.Column<int>(type: "int", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacion", x => x.PublicacionId);
                    table.ForeignKey(
                        name: "FK_Publicacion_Foro_ForoId",
                        column: x => x.ForoId,
                        principalTable: "Foro",
                        principalColumn: "ForoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Imagenes",
                columns: table => new
                {
                    ImagenesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagenNombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PublicacionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imagenes", x => x.ImagenesId);
                    table.ForeignKey(
                        name: "FK_Imagenes_Publicacion_PublicacionId",
                        column: x => x.PublicacionId,
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
                name: "IX_CategoriaForo_ForosForoId",
                table: "CategoriaForo",
                column: "ForosForoId");

            migrationBuilder.CreateIndex(
                name: "IX_Foro_UsuarioId",
                table: "Foro",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Imagenes_PublicacionId",
                table: "Imagenes",
                column: "PublicacionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Publicacion_ForoId",
                table: "Publicacion",
                column: "ForoId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicacionReaccion_ReaccionesReaccionId",
                table: "PublicacionReaccion",
                column: "ReaccionesReaccionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriaForo");

            migrationBuilder.DropTable(
                name: "Imagenes");

            migrationBuilder.DropTable(
                name: "PublicacionReaccion");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Publicacion");

            migrationBuilder.DropTable(
                name: "Reaccion");

            migrationBuilder.DropTable(
                name: "Foro");

            migrationBuilder.DropColumn(
                name: "FotodePerfil",
                table: "AspNetUsers");
        }
    }
}
