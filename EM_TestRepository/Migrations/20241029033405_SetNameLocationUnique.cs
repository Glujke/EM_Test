using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EM_TestRepository.Migrations
{
    public partial class SetNameLocationUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name",
                table: "Locations",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locations_Name",
                table: "Locations");
        }
    }
}
