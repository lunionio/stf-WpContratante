
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