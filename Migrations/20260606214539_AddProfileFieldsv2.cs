using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projekt.net_IKS_zofia_stanislawska.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileFieldsv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Profiles",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Profiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Profiles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Profiles",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Profiles");
        }
    }
}
