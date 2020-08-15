using Microsoft.EntityFrameworkCore.Migrations;

namespace API_Estate_management.Data.Migrations
{
    public partial class addRoleColumnIsCoreRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCoreRole",
                table: "AspNetRoles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCoreRole",
                table: "AspNetRoles");
        }
    }
}
