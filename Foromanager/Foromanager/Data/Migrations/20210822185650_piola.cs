using Microsoft.EntityFrameworkCore.Migrations;

namespace Foromanager.Data.Migrations
{
    public partial class piola : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Publicacion",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerID",
                table: "Foro",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Foro",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Publicacion");

            migrationBuilder.DropColumn(
                name: "OwnerID",
                table: "Foro");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Foro");
        }
    }
}
