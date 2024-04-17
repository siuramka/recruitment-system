using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecruitmentSystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class remove_cv_siteuser_relationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cvs_AspNetUsers_SiteUserId",
                table: "Cvs");

            migrationBuilder.DropIndex(
                name: "IX_Cvs_SiteUserId",
                table: "Cvs");

            migrationBuilder.DropColumn(
                name: "SiteUserId",
                table: "Cvs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SiteUserId",
                table: "Cvs",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cvs_SiteUserId",
                table: "Cvs",
                column: "SiteUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cvs_AspNetUsers_SiteUserId",
                table: "Cvs",
                column: "SiteUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
