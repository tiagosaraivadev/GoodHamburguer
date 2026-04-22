using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GoodHamburger.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedCardapio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Itens",
                columns: new[] { "Id", "Nome", "PedidoId", "Preco", "Tipo" },
                values: new object[,]
                {
                    { 1, "X Burger", null, 5.00m, 0 },
                    { 2, "X Egg", null, 4.50m, 0 },
                    { 3, "X Bacon", null, 7.00m, 0 },
                    { 4, "Batata Frita", null, 2.00m, 1 },
                    { 5, "Refrigerante", null, 2.50m, 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Itens",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Itens",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Itens",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Itens",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Itens",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
