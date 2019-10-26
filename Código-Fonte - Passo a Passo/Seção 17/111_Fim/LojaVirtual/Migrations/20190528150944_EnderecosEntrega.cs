using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LojaVirtual.Migrations
{
    public partial class EnderecosEntrega : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnderecosEntrega",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(nullable: false),
                    CEP = table.Column<string>(maxLength: 10, nullable: false),
                    Estado = table.Column<string>(nullable: false),
                    Cidade = table.Column<string>(nullable: false),
                    Bairro = table.Column<string>(nullable: false),
                    Endereco = table.Column<string>(nullable: false),
                    Complemento = table.Column<string>(nullable: false),
                    Numero = table.Column<string>(nullable: true),
                    ClienteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnderecosEntrega", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnderecosEntrega_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnderecosEntrega_ClienteId",
                table: "EnderecosEntrega",
                column: "ClienteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnderecosEntrega");
        }
    }
}
