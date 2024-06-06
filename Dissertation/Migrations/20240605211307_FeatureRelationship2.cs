using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dissertation.Migrations
{
    /// <inheritdoc />
    public partial class FeatureRelationship2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_ServiceListings_ServiceListingsId",
                table: "Features");

            migrationBuilder.DropIndex(
                name: "IX_Features_ServiceListingsId",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "ServiceListingsId",
                table: "Features");

            migrationBuilder.CreateTable(
                name: "FeaturesServiceListings",
                columns: table => new
                {
                    FeaturesId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceListingsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeaturesServiceListings", x => new { x.FeaturesId, x.ServiceListingsId });
                    table.ForeignKey(
                        name: "FK_FeaturesServiceListings_Features_FeaturesId",
                        column: x => x.FeaturesId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeaturesServiceListings_ServiceListings_ServiceListingsId",
                        column: x => x.ServiceListingsId,
                        principalTable: "ServiceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeaturesServiceListings_ServiceListingsId",
                table: "FeaturesServiceListings",
                column: "ServiceListingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeaturesServiceListings");

            migrationBuilder.AddColumn<int>(
                name: "ServiceListingsId",
                table: "Features",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Features_ServiceListingsId",
                table: "Features",
                column: "ServiceListingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_ServiceListings_ServiceListingsId",
                table: "Features",
                column: "ServiceListingsId",
                principalTable: "ServiceListings",
                principalColumn: "Id");
        }
    }
}
