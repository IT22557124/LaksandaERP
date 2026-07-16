using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laksanda.API.Migrations
{
    /// <inheritdoc />
    public partial class AddGoodsReceivedNoteManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoodsReceivedNotes",
                columns: table => new
                {
                    GoodsReceivedNoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    GRNNumber = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Remarks = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceivedNotes", x => x.GoodsReceivedNoteId);
                    table.ForeignKey(
                        name: "FK_GoodsReceivedNotes_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "PurchaseOrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GoodsReceivedNoteItems",
                columns: table => new
                {
                    GoodsReceivedNoteItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    GoodsReceivedNoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    RawMaterialId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceivedQuantity = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodsReceivedNoteItems", x => x.GoodsReceivedNoteItemId);
                    table.ForeignKey(
                        name: "FK_GoodsReceivedNoteItems_GoodsReceivedNotes_GoodsReceivedNote~",
                        column: x => x.GoodsReceivedNoteId,
                        principalTable: "GoodsReceivedNotes",
                        principalColumn: "GoodsReceivedNoteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodsReceivedNoteItems_RawMaterials_RawMaterialId",
                        column: x => x.RawMaterialId,
                        principalTable: "RawMaterials",
                        principalColumn: "RawMaterialId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceivedNoteItems_GoodsReceivedNoteId",
                table: "GoodsReceivedNoteItems",
                column: "GoodsReceivedNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceivedNoteItems_RawMaterialId",
                table: "GoodsReceivedNoteItems",
                column: "RawMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceivedNotes_GRNNumber",
                table: "GoodsReceivedNotes",
                column: "GRNNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoodsReceivedNotes_PurchaseOrderId",
                table: "GoodsReceivedNotes",
                column: "PurchaseOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoodsReceivedNoteItems");

            migrationBuilder.DropTable(
                name: "GoodsReceivedNotes");
        }
    }
}
