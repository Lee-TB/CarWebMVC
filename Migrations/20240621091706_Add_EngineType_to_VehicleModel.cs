using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarWebMVC.Migrations
{
    /// <inheritdoc />
    public partial class Add_EngineType_to_VehicleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EngineTypeId",
                table: "VehicleModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleModels_EngineTypeId",
                table: "VehicleModels",
                column: "EngineTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleModels_EngineTypes_EngineTypeId",
                table: "VehicleModels",
                column: "EngineTypeId",
                principalTable: "EngineTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleModels_EngineTypes_EngineTypeId",
                table: "VehicleModels");

            migrationBuilder.DropIndex(
                name: "IX_VehicleModels_EngineTypeId",
                table: "VehicleModels");

            migrationBuilder.DropColumn(
                name: "EngineTypeId",
                table: "VehicleModels");
        }
    }
}
