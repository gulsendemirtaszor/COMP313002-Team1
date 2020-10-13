using Microsoft.EntityFrameworkCore.Migrations;

namespace Books.Migrations
{
    public partial class Eighth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Advertisements",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Advertisements",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
