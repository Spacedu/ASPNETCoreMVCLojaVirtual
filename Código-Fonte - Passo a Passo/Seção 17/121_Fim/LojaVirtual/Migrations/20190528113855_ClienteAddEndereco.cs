using Microsoft.EntityFrameworkCore.Migrations;

namespace LojaVirtual.Migrations
{
    public partial class ClienteAddEndereco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Produtos",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Produtos",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CEP",
                table: "Clientes",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cidade",
                table: "Clientes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Clientes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Endereco",
                table: "Clientes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Clientes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Clientes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CEP",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Cidade",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Endereco",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Clientes");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Produtos",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Descricao",
                table: "Produtos",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
