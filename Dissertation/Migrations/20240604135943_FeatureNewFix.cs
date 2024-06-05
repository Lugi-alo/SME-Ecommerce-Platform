using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dissertation.Migrations
{
    /// <inheritdoc />
    public partial class FeatureNewFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_ServiceListings_ServiceListingsId",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "ServiceListingId",
                table: "Features");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceListingsId",
                table: "Features",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_ServiceListings_ServiceListingsId",
                table: "Features",
                column: "ServiceListingsId",
                principalTable: "ServiceListings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_ServiceListings_ServiceListingsId",
                table: "Features");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceListingsId",
                table: "Features",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceListingId",
                table: "Features",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Features_ServiceListings_ServiceListingsId",
                table: "Features",
                column: "ServiceListingsId",
                principalTable: "ServiceListings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
