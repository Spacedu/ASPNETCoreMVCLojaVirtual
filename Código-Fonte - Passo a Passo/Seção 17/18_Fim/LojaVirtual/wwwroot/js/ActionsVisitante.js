$(document).ready(function () {
    MoverScrollOrdenacao();
    MudarOrdenacao();
    MudarImagePrincipalProduto();
    MudarQuantidadeProdutoCarrinho();
});
function MudarQuantidadeProdutoCarrinho() {
    $("#order .btn-primary").click(function () {
        //var pai = $(this).parent().parent();
        if ($(this).hasClass("diminuir")) {
            LogicaMudarQuantidadeProdutoUnitarioCarrinho("diminuir", $(this));
            //var id = pai.find(".inputProdutoId").val();            
        }
        if ($(this).hasClass("aumentar")) {
            LogicaMudarQuantidadeProdutoUnitarioCarrinho("aumentar", $(this));
        }
    });
}
function LogicaMudarQuantidadeProdutoUnitarioCarrinho(operacao, botao) {
    //TODO - Implementar esta lógica.
    var pai = botao.parent().parent();

    var produtoId = pai.find(".inputProdutoId").val();
    var quantidadeEstoque = pai.find(".inputQuantidadeEstoque").val();
    var valorUnitario = pai.find(".inputValorUnitario").val();

    var campoQuantidadeProdutoCarrinho = pai.find(".inputQuantidadeProdutoCarrinho");
    var quantidadeProdutoCarrinho = parseInt(campoQuantidadeProdutoCarrinho.val());

    var campoValor = botao.parent().parent().parent().parent().parent().find(".price");

    //TODO - Adicionar validações.
    if (operacao == "aumentar") {
        quantidadeProdutoCarrinho++;
        campoQuantidadeProdutoCarrinho.val(quantidadeProdutoCarrinho);

        campoValor.text(valorUnitario * quantidadeProdutoCarrinho);
    } else if (operacao == "diminuir") {
        quantidadeProdutoCarrinho--;
        campoQuantidadeProdutoCarrinho.val(quantidadeProdutoCarrinho);

        campoValor.text(valorUnitario * quantidadeProdutoCarrinho);
    }

    //TODO - Atualizar o subtotal do produto
}
function MudarImagePrincipalProduto() {
    $(".img-small-wrap img").click(function () {
        var Caminho = $(this).attr("src");
        $(".img-big-wrap img").attr("src", Caminho);
        $(".img-big-wrap a").attr("href", Caminho);
    });
}
function MoverScrollOrdenacao() {
    if (window.location.hash.length > 0) {
        var hash = window.location.hash;
        if (hash == "#posicao-produto") {
            window.scrollBy(0, 473);
        }
    }
}
function MudarOrdenacao() {
    $("#ordenacao").change(function () {
        //TODO - Redirecionar para a tela Home/Index passando as QueryString de Ordenação e mantendo a Pagina e a pesquisa.
        var Pagina = 1;
        var Pesquisa = "";
        var Ordenacao = $(this).val();
        var Fragmento = "#posicao-produto";

        var QueryString = new URLSearchParams(window.location.search);
        if (QueryString.has("pagina")) {
            Pagina = QueryString.get("pagina");
        }
        if (QueryString.has("pesquisa")) {
            Pesquisa = QueryString.get("pesquisa");
        }
        if ($("#breadcrumb").length > 0) {
            Fragmento = "";
        }

        var URL = window.location.protocol + "//" + window.location.host + window.location.pathname;

        var URLComParametros = URL + "?pagina=" + Pagina + "&pesquisa=" + Pesquisa + "&ordenacao=" + Ordenacao + Fragmento;
        window.location.href = URLComParametros;

    });
}