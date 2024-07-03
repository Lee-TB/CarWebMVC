using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarWebMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddTransmissionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransmissionId",
                table: "VehicleModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Transmission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transmission", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleModels_TransmissionId",
                table: "VehicleModels",
                column: "TransmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleModels_Transmission_TransmissionId",
                table: "VehicleModels",
                column: "TransmissionId",
                principalTable: "Transmission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleModels_Transmission_TransmissionId",
                table: "VehicleModels");

            migrationBuilder.DropTable(
                name: "Transmission");

            migrationBuilder.DropIndex(
                name: "IX_VehicleModels_TransmissionId",
                table: "VehicleModels");

            migrationBuilder.DropColumn(
                name: "TransmissionId",
                table: "VehicleModels");
        }
    }
}
