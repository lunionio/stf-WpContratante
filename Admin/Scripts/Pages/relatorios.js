$(document).ready(function () {
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
        $('#tbRelatorio tbody').empty();
        $("#tbRelatorio").removeAttr("hidden");
        var result = response;
        $.each(result, function (index, element, array) {
            $("#tbRelatorio > tbody").append("<tr>" +
                "<td>" + element.Codigo + "</td>" +
                "<td>" + element.Titulo + "</td>" +
                "<td>" + element.CriadoEm + "</td>" +
                "<td>" + element.DataEvento + "</td>" +
                "<td>" + element.Endereco + "</td>" +
                "<td>" + element.Categoria + "</td>" +
                "<td>" + element.Profissional + "</td>" +
                "<td>" + element.Valor + "</td>" +
                "<td>" + element.Quantidade + "</td>" +
                "<td>" + element.Total + "</td>" +
                "<td>" + element.Candidatos + "</td>" +
                "<td>" + element.Aprovados + "</td>" +
                "<td>" + element.Reprovados + "</td>" +
                "</tr>");



            $('body').loading('stop');
        });

        $("#tbRelatorio").DataTable({
            "pagingType": "numbers",
            "columnDefs": [{
                "targets": '_all',
                "orderable": false,
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
                "sSearch": "Pesquisar:",
            },
          
        });

    });
});