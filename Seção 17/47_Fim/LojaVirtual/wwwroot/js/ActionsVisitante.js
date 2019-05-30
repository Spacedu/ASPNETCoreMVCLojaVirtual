$(document).ready(function () {
    MoverScrollOrdenacao();
    MudarOrdenacao();
    MudarImagePrincipalProduto();
    MudarQuantidadeProdutoCarrinho();

    MascaraCEP();
    AcaoCalcularFreteBTN();
    AJAXCalcularFrete(false);
});
function MascaraCEP() {
    $(".cep").mask("00.000-000");
}
function AcaoCalcularFreteBTN() {
    $(".btn-calcular-frete").click(function (e) {
        AJAXCalcularFrete(true);
        e.preventDefault();
    });
}

function AJAXCalcularFrete(callByButton) {
    if (callByButton == false) {
        if ($.cookie('Carrinho.CEP') != undefined) {
            $(".cep").val($.cookie('Carrinho.CEP'));
        }
    }


    var cep = $(".cep").val().replace(".", "").replace("-", "");
    
    if (cep.length == 8) {

        $.cookie('Carrinho.CEP', $(".cep").val());
        $(".container-frete").html("<br /><br /><img src='\\img\\loading.gif' />");

        $.ajax({
            type: "GET",
            url: "/CarrinhoCompra/CalcularFrete?cepDestino=" + cep,
            error: function (data) {
                MostrarMensagemDeErro("Opps! Tivemos um erro ao obter o Frete..." + data.Message);
                console.info(data);
            },
            success: function (data) {
                html = "";

                for (var i = 0; i < data.length; i++) {
                    var tipoFrete = data[i].tipoFrete;
                    var valor = data[i].valor;
                    var prazo = data[i].prazo;

                    html += "<dl class=\"dlist-align\"><dt><input type=\"radio\" name=\"frete\" value=\"" + tipoFrete + "\" /><input type=\"hidden\" name=\"valor\" value=\"" + valor + "\" /></dt><dd>" + tipoFrete + " - " + numberToReal(valor) + " (" + prazo + " dias últeis)</dd></dl>";
                }

                $(".container-frete").html(html);
                $(".container-frete").find("input[type=radio]").change(function () {
                    var valorFrete = parseFloat( $(this).parent().find("input[type=hidden]").val() );
                    $(".frete").text(numberToReal( valorFrete ));
                    
                });
                console.info(data);
            }
        });
    } else {
        if (callByButton == true) {
            MostrarMensagemDeErro("Digite o CEP para calcular o frete!");
        }
    }
}
function numberToReal(numero) {
    console.info(numero);
    var numero = numero.toFixed(2).split('.');
    numero[0] = "R$ " + numero[0].split(/(?=(?:...)*$)/).join('.');
    return numero.join(',');
}
function MudarQuantidadeProdutoCarrinho() {
    $("#order .btn-primary").click(function () {
        if ($(this).hasClass("diminuir")) {
            OrquestradorDeAcoesProduto("diminuir", $(this));
        }
        if ($(this).hasClass("aumentar")) {
            OrquestradorDeAcoesProduto("aumentar", $(this));
        }
    });
}



function OrquestradorDeAcoesProduto(operacao, botao) {
    OcultarMensagemDeErro();
    /*
     * Carregamento dos valores
     */
    var pai = botao.parent().parent();

    var produtoId = pai.find(".inputProdutoId").val();
    var quantidadeEstoque = parseInt(pai.find(".inputQuantidadeEstoque").val());
    var valorUnitario = parseFloat(pai.find(".inputValorUnitario").val().replace(",", "."));

    var campoQuantidadeProdutoCarrinho = pai.find(".inputQuantidadeProdutoCarrinho");
    var quantidadeProdutoCarrinhoAntiga = parseInt(campoQuantidadeProdutoCarrinho.val());

    var campoValor = botao.parent().parent().parent().parent().parent().find(".price");

    var produto = new ProdutoQuantidadeEValor(produtoId, quantidadeEstoque, valorUnitario, quantidadeProdutoCarrinhoAntiga, 0, campoQuantidadeProdutoCarrinho, campoValor);

    /*
     * Chamada de Métodos
     */
    AlteracoesVisuaisProdutoCarrinho(produto, operacao);

    //TODO - Adicionar validações.

    //TODO - Atualizar o subtotal do produto
}
function AlteracoesVisuaisProdutoCarrinho(produto, operacao) {
    if (operacao == "aumentar") {
        /*if (produto.quantidadeProdutoCarrinhoAntiga == produto.quantidadeEstoque) {
            alert("Opps! Não possuimos estoque suficiente para a quantidade que você deseja comprar!");
        } else*/ {
            produto.quantidadeProdutoCarrinhoNova = produto.quantidadeProdutoCarrinhoAntiga + 1;

            AtualizarQuantidadeEValor(produto);

            AJAXComunicarAlteracaoQuantidadeProduto(produto);

        }
    } else if (operacao == "diminuir") {
        /*if (produto.quantidadeProdutoCarrinhoAntiga == 1) {
            alert("Opps! Caso não deseje este produto clique no botão Remover");
        } else */ {
            produto.quantidadeProdutoCarrinhoNova = produto.quantidadeProdutoCarrinhoAntiga - 1;

            AtualizarQuantidadeEValor(produto);

            AJAXComunicarAlteracaoQuantidadeProduto(produto);
        }
    }
}
function AJAXComunicarAlteracaoQuantidadeProduto(produto) {
    $.ajax({
        type: "GET",
        url: "/CarrinhoCompra/AlterarQuantidade?id=" + produto.produtoId + "&quantidade=" + produto.quantidadeProdutoCarrinhoNova,
        error: function (data) {
            MostrarMensagemDeErro(data.responseJSON.mensagem);

            //Rollback
            produto.quantidadeProdutoCarrinhoNova = produto.quantidadeProdutoCarrinhoAntiga;
            AtualizarQuantidadeEValor(produto);
        },
        success: function () {
            AJAXCalcularFrete();
        }
    });
}
function MostrarMensagemDeErro(mensagem) {
    $(".alert-danger").css("display", "block");
    $(".alert-danger").text(mensagem);
}
function OcultarMensagemDeErro() {
    $(".alert-danger").css("display", "none");
}

function AtualizarQuantidadeEValor(produto) {
    produto.campoQuantidadeProdutoCarrinho.val(produto.quantidadeProdutoCarrinhoNova);

    var resultado = produto.valorUnitario * produto.quantidadeProdutoCarrinhoNova;
    produto.campoValor.text(numberToReal(resultado));

    AtualizarSubtotal();
}
function AtualizarSubtotal() {
    var Subtotal = 0;

    var TagsComPrice = $(".price");

    TagsComPrice.each(function () {
        var ValorReais = parseFloat($(this).text().replace("R$", "").replace(".", "").replace(" ", "").replace(",", "."));

        Subtotal += ValorReais;
    })
    $(".subtotal").text(numberToReal(Subtotal));


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


/*
 * ------------------ Classes --------------------
 */
class ProdutoQuantidadeEValor {
    constructor(produtoId, quantidadeEstoque, valorUnitario, quantidadeProdutoCarrinhoAntiga, quantidadeProdutoCarrinhoNova, campoQuantidadeProdutoCarrinho, campoValor) {
        this.produtoId = produtoId;
        this.quantidadeEstoque = quantidadeEstoque;
        this.valorUnitario = valorUnitario;

        this.quantidadeProdutoCarrinhoAntiga = quantidadeProdutoCarrinhoAntiga;
        this.quantidadeProdutoCarrinhoNova = quantidadeProdutoCarrinhoNova;

        this.campoQuantidadeProdutoCarrinho = campoQuantidadeProdutoCarrinho;
        this.campoValor = campoValor;
    }
}