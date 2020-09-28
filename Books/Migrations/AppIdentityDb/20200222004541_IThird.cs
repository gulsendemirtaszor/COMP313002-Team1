using Microsoft.EntityFrameworkCore.Migrations;

namespace Books.Migrations.AppIdentityDb
{
    public partial class IThird : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppUserFK",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserFK",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
