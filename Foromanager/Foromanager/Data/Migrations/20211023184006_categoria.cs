using Microsoft.EntityFrameworkCore.Migrations;

namespace Foromanager.Data.Migrations
{
    public partial class categoria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Foro");

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaNombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ForoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.CategoriaId);
                    table.ForeignKey(
                        name: "FK_Categoria_Foro_ForoId",
                        column: x => x.ForoId,
                        principalTable: "Foro",
                        principalColumn: "ForoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_ForoId",
                table: "Categoria",
                column: "ForoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Foro",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
