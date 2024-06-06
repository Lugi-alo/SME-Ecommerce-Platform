using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dissertation.Migrations
{
    /// <inheritdoc />
    public partial class CheckFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeaturesServiceListings");

            migrationBuilder.CreateTable(
                name: "ServiceListingFeature",
                columns: table => new
                {
                    ServiceListingsId = table.Column<int>(type: "INTEGER", nullable: false),
                    FeaturesId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiceListingsId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceListingFeature", x => new { x.ServiceListingsId, x.FeaturesId });
                    table.ForeignKey(
                        name: "FK_ServiceListingFeature_Features_FeaturesId",
                        column: x => x.FeaturesId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceListingFeature_ServiceListings_ServiceListingsId",
                        column: x => x.ServiceListingsId,
                        principalTable: "ServiceListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceListingFeature_ServiceListings_ServiceListingsId1",
                        column: x => x.ServiceListingsId1,
                        principalTable: "ServiceListings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceListingFeature_FeaturesId",
                table: "ServiceListingFeature",
                column: "FeaturesId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceListingFeature_ServiceListingsId1",
                table: "ServiceListingFeature",
                column: "ServiceListingsId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceListingFeature");

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
    }
}
