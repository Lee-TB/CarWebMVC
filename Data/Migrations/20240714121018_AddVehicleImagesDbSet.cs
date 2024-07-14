using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarWebMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleImagesDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleImage_VehicleModels_VehicleModelId",
                table: "VehicleImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleImage",
                table: "VehicleImage");

            migrationBuilder.RenameTable(
                name: "VehicleImage",
                newName: "VehicleImages");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleImage_VehicleModelId",
                table: "VehicleImages",
                newName: "IX_VehicleImages_VehicleModelId");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleModelId",
                table: "VehicleImages",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleImages",
                table: "VehicleImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleImages_VehicleModels_VehicleModelId",
                table: "VehicleImages",
                column: "VehicleModelId",
                principalTable: "VehicleModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleImages_VehicleModels_VehicleModelId",
                table: "VehicleImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleImages",
                table: "VehicleImages");

            migrationBuilder.RenameTable(
                name: "VehicleImages",
                newName: "VehicleImage");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleImages_VehicleModelId",
                table: "VehicleImage",
                newName: "IX_VehicleImage_VehicleModelId");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleModelId",
                table: "VehicleImage",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleImage",
                table: "VehicleImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleImage_VehicleModels_VehicleModelId",
                table: "VehicleImage",
                column: "VehicleModelId",
                principalTable: "VehicleModels",
                principalColumn: "Id");
        }
    }
}
