using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API_Estate_management.Data.Migrations
{
    public partial class createCommonFeeReferencetoHouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommonFeeTypes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Type = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonFeeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CommonFees",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(6, 2)", nullable: false),
                    Detail = table.Column<string>(type: "text", nullable: false),
                    StatusPay = table.Column<bool>(nullable: false),
                    KeepDate = table.Column<DateTime>(nullable: false),
                    GetDate = table.Column<DateTime>(nullable: false),
                    HouseId = table.Column<string>(nullable: true),
                    CommonFeeTypeId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonFees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommonFees_CommonFeeTypes_CommonFeeTypeId",
                        column: x => x.CommonFeeTypeId,
                        principalTable: "CommonFeeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommonFees_Houses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommonFees_CommonFeeTypeId",
                table: "CommonFees",
                column: "CommonFeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommonFees_HouseId",
                table: "CommonFees",
                column: "HouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommonFees");

            migrationBuilder.DropTable(
                name: "CommonFeeTypes");
        }
    }
}
