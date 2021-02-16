using Microsoft.EntityFrameworkCore.Migrations;

namespace CoOwnershipManager.Migrations
{
    public partial class ApartmentHasManyUnhabitants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApartmentId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ApartmentId",
                table: "AspNetUsers",
                column: "ApartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Apartments_ApartmentId",
                table: "AspNetUsers",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Apartments_ApartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ApartmentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApartmentId",
                table: "AspNetUsers");
        }
    }
}
