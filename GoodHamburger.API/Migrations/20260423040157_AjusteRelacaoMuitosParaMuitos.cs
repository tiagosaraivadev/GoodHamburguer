using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoodHamburger.API.Migrations
{
    /// <inheritdoc />
    public partial class AjusteRelacaoMuitosParaMuitos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Itens_Pedidos_PedidoId",
                table: "Itens");

            migrationBuilder.DropIndex(
                name: "IX_Itens_PedidoId",
                table: "Itens");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "Itens");

            migrationBuilder.CreateTable(
                name: "PedidoItens",
                columns: table => new
                {
                    ItensId = table.Column<int>(type: "INTEGER", nullable: false),
                    PedidosId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItens", x => new { x.ItensId, x.PedidosId });
                    table.ForeignKey(
                        name: "FK_PedidoItens_Itens_ItensId",
                        column: x => x.ItensId,
                        principalTable: "Itens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoItens_Pedidos_PedidosId",
                        column: x => x.PedidosId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Itens",
                keyColumn: "Id",
                keyValue: 5,
                column: "Nome",
                value: "Coca-Cola 250ml");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItens_PedidosId",
                table: "PedidoItens",
                column: "PedidosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoItens");

            migrationBuilder.AddColumn<int>(
                name: "PedidoId",
                table: "Itens",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Itens",
                keyColumn: "Id",
                keyValue: 1,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Itens",
                keyColumn: "Id",
                keyValue: 2,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Itens",
                keyColumn: "Id",
                keyValue: 3,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Itens",
                keyColumn: "Id",
                keyValue: 4,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Itens",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Nome", "PedidoId" },
                values: new object[] { "Refrigerante", null });

            migrationBuilder.CreateIndex(
                name: "IX_Itens_PedidoId",
                table: "Itens",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Itens_Pedidos_PedidoId",
                table: "Itens",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id");
        }
    }
}
