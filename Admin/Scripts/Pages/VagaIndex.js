
$("#linhaPadrao").remove();

$('.carousel').carousel();

LoadPanels();


function LoadPanels() {
    Loading('.content');
    getOportunidades();
    getProfissionais()
    getVinculoProfissional();
    LoadingStop('.content');

}
    


function Loading(elemento) {
    $(elemento).loading({
        theme: 'dark',
        message: 'Aguarde...'
    });
}

function LoadingStop(elemento) {
    $(elemento).loading('stop');
}



function getOportunidades() {

    var Url = "vaga/_listarOportunidades";
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": Url,
        "method": "GET"
    }

    $.ajax(settings).done(function (response) {
        $('#oportunidades').hide();
        $('#oportunidades').html(response);
        $('#oportunidades').fadeIn();

        $('#tbOportunidades').dataTable({
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
}

function getProfissionais() {
    var Url = "vaga/_lsitarProfissionais";
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": Url,
        "method": "GET"
    }

    $.ajax(settings).done(function (response) {
        $('#profissionais').hide();
        $('#profissionais').html(response);
        $('#profissionais').fadeIn();
    });
}

function getVinculoProfissional() {
    var Url = "vaga/_vincularPorifissionais";
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": Url,
        "method": "GET"
    }

    $.ajax(settings).done(function (response) {
        $('#vinculoProfissionais').hide();
        $('#vinculoProfissionais').html(response);
        $('#vinculoProfissionais').fadeIn();

    });
}