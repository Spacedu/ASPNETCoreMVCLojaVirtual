$(document).ready(function () {
    $(".btn-danger").click(function (e) {
        var resultado = confirm("Tem certeza que deseja excluir este registro?");

        if (resultado == false) {
            e.preventDefault();
        }        
    });
});