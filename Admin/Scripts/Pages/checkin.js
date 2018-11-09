$('#qrCode').hide();

function setQrCodeImage(response) {
    let result = $.parseJSON(response);
    $("#loading").hide();
    $('#qrCode').attr('src', "data:image/png;base64," + result.qrCode);
    $('#qrCode').show();
}

function getQrCode() {
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