using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projekt.net_IKS_zofia_stanislawska.Migrations
{
    /// <inheritdoc />
    public partial class ZmianaWygladuSpecie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreedingSeason",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "Depth",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "WaterFlavour",
                table: "Species");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Species",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Species");

            migrationBuilder.AddColumn<string>(
                name: "BreedingSeason",
                table: "Species",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Depth",
                table: "Species",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaterFlavour",
                table: "Species",
                type: "TEXT",
                nullable: true);
        }
    }
}
