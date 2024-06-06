using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dissertation.Migrations
{
    /// <inheritdoc />
    public partial class CheckFeaturesNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceListingFeature_ServiceListings_ServiceListingsId1",
                table: "ServiceListingFeature");

            migrationBuilder.DropIndex(
                name: "IX_ServiceListingFeature_ServiceListingsId1",
                table: "ServiceListingFeature");

            migrationBuilder.DropColumn(
                name: "ServiceListingsId1",
                table: "ServiceListingFeature");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ServiceListingsId1",
                table: "ServiceListingFeature",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceListingFeature_ServiceListingsId1",
                table: "ServiceListingFeature",
                column: "ServiceListingsId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceListingFeature_ServiceListings_ServiceListingsId1",
                table: "ServiceListingFeature",
                column: "ServiceListingsId1",
                principalTable: "ServiceListings",
                principalColumn: "Id");
        }
    }
}
