$('#qrCode').hide();

function setQrCodeImage(response) {
    let result = $.parseJSON(response);
    $("#loading").hide();
    $('#qrCode').attr('src', "data:image/png;base64," + result.qrCode);
    $('#qrCode').show();
}

function getProfissionaisByOpt() {
    var Url = "/CheckIn/GetOpInfo";
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": Url,
        "method": "GET"
    };

    $.ajax(settings).done(function (response) {

        $("#confirmados > tbody").html("");
        $("#aguardando > tbody").html("");

        let result = $.parseJSON(response);
        $.each(result, function (index, item, array) {
            if (item.Status.ID == 1) { //Aprovado
                $('#confirmados').find('tbody').append('<tr><td>' + item.Profissional.Nome + '</td><td>' + item.Profissional.Telefone.Numero + '</td></tr>');
            }
            else if (item.Status.ID == 2) { //Aguardando
                $('#aguardando').find('tbody').append('<tr><td>' + item.Profissional.Nome + '</td><td>' + item.Profissional.Telefone.Numero + '</td></tr>');
            }
        });
    });
}

function getQrCode() {

    getProfissionaisByOpt();

    var Url = "/CheckIn/GetQrCode";
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": Url,
        "method": "GET"
    };

    $.ajax(settings).done(function (response) {
        setQrCodeImage(response);
    });

    setTimeout(function () {
        getQrCode();
    }, 60000);
}

$(document).ready(getQrCode());