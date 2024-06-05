using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dissertation.Migrations
{
    /// <inheritdoc />
    public partial class FeatureIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Features");

            migrationBuilder.AddColumn<string>(
                name: "IconPath",
                table: "Features",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconPath",
                table: "Features");

            migrationBuilder.AddColumn<byte[]>(
                name: "Icon",
                table: "Features",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
