using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LojaVirtual.Migrations
{
    public partial class PedidoPedidoSituacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClienteId = table.Column<int>(nullable: true),
                    TransactionId = table.Column<string>(nullable: true),
                    FreteEmpresa = table.Column<string>(nullable: true),
                    FreteCodRastreamento = table.Column<string>(nullable: true),
                    FormaPagamento = table.Column<string>(nullable: true),
                    ValorTotal = table.Column<decimal>(nullable: false),
                    DadosTransaction = table.Column<string>(nullable: true),
                    DadosProdutos = table.Column<string>(nullable: true),
                    DataRegistro = table.Column<DateTime>(nullable: false),
                    Situacao = table.Column<string>(nullable: true),
                    NFE = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pedidos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PedidoSituacoes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PedidoId = table.Column<int>(nullable: true),
                    Data = table.Column<DateTime>(nullable: false),
                    Situacao = table.Column<string>(nullable: true),
                    Dados = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoSituacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoSituacoes_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_ClienteId",
                table: "Pedidos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoSituacoes_PedidoId",
                table: "PedidoSituacoes",
                column: "PedidoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PedidoSituacoes");

            migrationBuilder.DropTable(
                name: "Pedidos");
        }
    }
}
