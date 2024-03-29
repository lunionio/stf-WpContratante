﻿$(document).ready(function () {
    $(".chosen-select").chosen({ no_results_text: "Nada encontrado!" });
    $(".chosen-select").chosen({ allow_single_deselect: true });
});

$('#download').click(function () {
    var r = $('#relatorios option:selected').val();
    window.location.href = 'Gerar?relatorioId=' + r;
});


$('.botoes').hide();

//comprotamento do select option
$('#relatorios ').change(function () {
    $('.botoes').fadeIn();
});


$('#mostrar').click(function () {
    $('body').loading({
        theme: 'dark',
        message: 'Aguarde...',
        onStart: function (loading) {
            loading.overlay.slideDown(400);
        },
        onStop: function (loading) {
            loading.overlay.slideUp(400);
        }
    });

    var r = $('#relatorios option:selected').val();

    let Url = "/Relatorios/Relatorios?relatorioId=" + r;
    let settings = {
        "async": true,
        "crossDomain": true,
        "url": Url,
        "method": "GET"
    };

    $.ajax(settings).done(function (response) {
        if ($.fn.DataTable.isDataTable("#tbRelatorio")) {
            $('#tbRelatorio').DataTable().clear().destroy();
        }
        $('#tbRelatorio tbody').empty();
        $('#tbRelatorio thead').empty();
        $("#tbRelatorio").removeAttr("hidden");

        if (r == 2) {

            $("#tbRelatorio > thead").append("<tr>" +
                "<th>Codigo</th>" +
                "<th>Titulo</th>" +
                "<th>CriadoEm</th>" +
                "<th>DataEvento</th>" +
                "<th>Endereco</th>" +
                "<th>Categoria</th>" +
                "<th>Profissional</th>" +
                "<th>Valor</th>" +
                "<th>Quantidade</th>" +
                "<th>Total</th>" +
                "<th>Candidatos</th>" +
                "<th>Aprovados</th>" +
                "<th>Reprovados</th>" +
                "</tr>");

            var result1 = response;
            $.each(result1, function (index, element, array) {
                $("#tbRelatorio > tbody").append("<tr>" +
                    "<td>" + element.Codigo + "</td>" +
                    "<td>" + element.Titulo + "</td>" +
                    "<td>" + element.Criado + "</td>" +
                    "<td>" + element.Data + "</td>" +
                    "<td>" + element.Endereco + "</td>" +
                    "<td>" + element.Categoria + "</td>" +
                    "<td>" + element.Profissional + "</td>" +
                    "<td>R$: " + element.Valor + "</td>" +
                    "<td>" + element.Quantidade + "</td>" +
                    "<td>R$: " + element.Total + "</td>" +
                    "<td>" + element.Candidatos + "</td>" +
                    "<td>" + element.Aprovados + "</td>" +
                    "<td>" + element.Reprovados + "</td>" +
                    "</tr>");
            });
        }
        else if (r == 7) {

            $("#tbRelatorio > thead").append("<tr>" +
                "<th>ID</th>" +
                "<th>Nome</th>" +
                "<th>Descricao</th>" +
                "<th>Data Criacao</th>" +
                "<th>Valor</th>" +
                "</tr>");

            var result2 = response;

            $.each(result2, function (index, element, array) {
                $("#tbRelatorio > tbody").append("<tr>" +
                    "<td>" + element.ID + "</td>" +
                    "<td>" + element.Nome + "</td>" +
                    "<td>" + element.Descricao + "</td>" +
                    "<td>" + element.Data + "</td>" +
                    "<td>R$: " + element.Valor + "</td>" +
                    "</tr>");
            });
        }

        $("#tbRelatorio").DataTable({
            "pagingType": "numbers",
            "columnDefs": [{
                "targets": '_all',
                "orderable": true
            }],
            "dom": '<"top"f>rt' + "<'bottom col-sm-12'" +
                "<'row'" +
                "<'col-sm-6'l>" +
                "<'col-sm-6'p>" +
                ">" +
                ">" + '<"clear">',
            "oLanguage": {
                "sLengthMenu": "_MENU_",
                "sZeroRecords": "Nada encontrado",
                "sInfo": "Mostrando oágina _PAGE_ de _PAGES_",
                "sInfoEmpty": "Nenhum dado para mostrar",
                "sInfoFiltered": "(Filtrado de _MAX_ registros)",
                "sSearch": "Pesquisar:"
            }          
        });

        LoadingStop('body');

    });
});

function LoadingStop(elemento) {
    $(elemento).loading('stop');
}