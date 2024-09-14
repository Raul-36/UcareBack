using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UcareBackApp.Migrations
{
    /// <inheritdoc />
    public partial class deleteCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_AspNetUsers_UserEmail",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_UserEmail",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Cards_UserEmail",
                table: "Cards",
                column: "UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_AspNetUsers_UserEmail",
                table: "Cards",
                column: "UserEmail",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
