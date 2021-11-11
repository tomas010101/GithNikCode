using Microsoft.EntityFrameworkCore.Migrations;

namespace Foromanager.Data.Migrations
{
    public partial class usuarioForo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Foro",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Foro_UsuarioId",
                table: "Foro",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Foro_AspNetUsers_UsuarioId",
                table: "Foro",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foro_AspNetUsers_UsuarioId",
                table: "Foro");

            migrationBuilder.DropIndex(
                name: "IX_Foro_UsuarioId",
                table: "Foro");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Foro");
        }
    }
}
