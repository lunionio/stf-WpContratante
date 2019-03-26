$("#miolo").addClass("card-wizard col-md-10");
preencherTipos();
controlChanges();
$('.sweet-confirm btn btn-info btn-fill').click(function () {
    window.location = "/home/index";
});

$('#enviar').click(function () {


    enviar();

});

function preencherTipos() {
    LoadingStart('body');

    var Url = "GetTipos";
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": Url,
        "method": "GET"
    }

    $.ajax(settings).done(function (response) {
        for (var i = 0; i < response.length; i++) {
            $("#assunto").empty();
            $('#assunto').append('<option value="' + response[i].id + '">' + response[i].nome + '</option>');
            $('#assunto').selectpicker('refresh');
        }
        LoadingStop('body');
    });
}

function controlChanges() {

    $('#assunto').change(function () {
        $('#email').focus();

    })
    $('#email').change(function () {
        $('#mensagem').focus();

    });
    $('#mensagem').change(function () {
        $('#enviar').focus();
    })
}

function getForm() {
    return {
        assunto: $('#assunto'),
        email: $('#email'),
        mensagem: $('#mensagem')
    }
}

function getFormData() {
    return {
        assunto: $('#assunto option:selected').val(),
        assuntoNome: $('#assunto option:selected').text(),
        email: $('#email').val(),
        mensagem: $('#mensagem').val(),
        tipoId: $('#assunto option:selected').prop('id')
    }
}

function LoadingStop(elemento) {
    $(elemento).loading('stop');
}

function LoadingStart(elemento) {
    $(elemento).loading({
        theme: 'dark',
        message: 'Aguarde...'
    });
}


function enviar() {
    LoadingStart('body');

    var data = getFormData();
    var settings = {
        "url": "Enviar",
        "method": "POST",
        "data": data
    };    

    $.ajax(settings).done(function (response) {
        var mensagem = response;
       
        swal(mensagem, "Recebemos sua solicitação, em breve entraremos em contato", "success");
        //LoadingStop('body');
        setTimeout(function () {
            window.location = "/home/index";
        }, 4500);

        LoadingStop('body');
    });
}
