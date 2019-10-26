using Microsoft.EntityFrameworkCore.Migrations;

namespace LojaVirtual.Migrations
{
    public partial class ProdutoRenomearQuantidadeEstoque : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("Quantidade", "Produtos", "Estoque");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("Estoque", "Produtos", "Quantidade");
        }
    }
}
