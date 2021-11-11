using Microsoft.EntityFrameworkCore.Migrations;

namespace Foromanager.Data.Migrations
{
    public partial class act_publicaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Publicacion",
                newName: "Usuario");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Usuario",
                table: "Publicacion",
                newName: "UserId");
        }
    }
}
