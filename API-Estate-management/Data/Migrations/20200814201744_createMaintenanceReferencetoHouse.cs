using Microsoft.EntityFrameworkCore.Migrations;

namespace API_Estate_management.Data.Migrations
{
    public partial class createMaintenanceReferencetoHouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Maintenances",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(maxLength: 256, nullable: false),
                    ImageMainTen = table.Column<string>(maxLength: 256, nullable: false),
                    StatusMainten = table.Column<bool>(nullable: false),
                    HouseId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maintenances_Houses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_HouseId",
                table: "Maintenances",
                column: "HouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Maintenances");
        }
    }
}
