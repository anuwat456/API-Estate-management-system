using Microsoft.EntityFrameworkCore.Migrations;

namespace API_Estate_management.Data.Migrations
{
    public partial class createHouseReferencetoUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HouseTypes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Type = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Houses",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 256, nullable: false),
                    LaneHouse = table.Column<string>(maxLength: 256, nullable: false),
                    ColorHouse = table.Column<string>(maxLength: 256, nullable: false),
                    AreaHouse = table.Column<string>(maxLength: 256, nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    HouseTypeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Houses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Houses_HouseTypes_HouseTypeId",
                        column: x => x.HouseTypeId,
                        principalTable: "HouseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Houses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Houses_HouseTypeId",
                table: "Houses",
                column: "HouseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Houses_UserId",
                table: "Houses",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Houses");

            migrationBuilder.DropTable(
                name: "HouseTypes");
        }
    }
}
