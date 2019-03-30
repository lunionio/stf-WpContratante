
init();
controlarPaineis();
getAreaAtuacao();
aplicarMascaras();

$('#cadastrar').on('click', function () {
    if (validarCampos() == true) {
        carregaModal();
    }
});

$('#btnAgora').on('click', function () {
    
    PublicarAgora();
});

function VagaViewModel() {
    var data = $('#data').val();
    var dataEvento = Date.parse(data);
    var VagaViewModel = {
        id: $('#vagaId').val(),
        nome: $('#nome').val(),
        cep: $('#cep').val(),
        Rua: $('#rua').val(),
        Bairro: $('#bairro').val(),
        Numero: $('#numero').val(),
        Cidade: $('#cidade').val(),
        Date: data,
        Complemento: $('#complemento').val(),
        Referencia: $('#complemento').val(),
        Uf: $('#uf').val(),
        DataEvento: data,
        Hora: $('#txtHoraInicio').val(),
        Qtd: $('#qtd').val(),
        Valor: $('#valor').val(),
        Atuacao: $('#atuacao').val(),
        Profissional: $('#profissional option:selected').val(),
        ProfissionalNome: $('#profissional option:selected').text(),
        //Qtd: $('#qtd').val(),
        Total: $('#total').val(),
        //IdEmpresa: $('#empresas option:selected').val(),
        EnderecoId: $('#endId').val(),
        DataCriacao: $('#vagaData').val(),
        EnderecoDataCriacao: $('#enderecoData').val()
    };

    return VagaViewModel;
}

function PublicarAgora() {

    LoadingInitBase('#modal');
    var vaga = VagaViewModel();
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": "/Vaga/PublicarAgora",
        "method": "POST",
        "data": vaga
    };

    $.ajax(settings).done(function (response) {
        if (response == "ok") {
     
            window.location = "/Vaga";
        }
        else {
            alert(response);
            LoadingStop('#modal');
        }
    });
}

function PublicarDepois() {


}

function getFormData() {
    return {
        nome: $('#nome').val(),
        cep: $('#cep').val(),
        rua: $('#rua').val(),
        bairro: $('#bairro').val(),
        numero: $('#numero').val(),
        cidade: $('#cidade').val(),
        uf: $('#uf').val(),
        data: $('#data').val(),
        hora: $('#txtHoraInicio').val(),
        qtd: $('#qtd').val(),
        valor: $('#valor').val(),
        atuacao: $('#atuacao').val(),
        profissional: $('#profissional option:selected').val(),
        profissionalNome: $('#profissional').val(),
        Qtd: $('#qtd').val(),
        Total: $('#total').val()
    }
}

function aplicarMascaras() {
    $('#data').mask('00/00/0000');
    $('#numero').mask('9999');
    $('#cep').mask('99999-999');
}

function getAreaAtuacao() {

    var Url = "/Profissionais/BuscarServicoTipo/";
    //var Url = "http://localhost:5300/api/seguranca/wpProfissionais/BuscarServicoTipo/1/999";
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": Url,
        "method": "GET"
    }

    $.ajax(settings).done(function (response) {
        for (var i = 0; i < response.length; i++) {
            $('#atuacao').append('<option value="' + response[i].id + '">' + response[i].nome + '</option>');
            $('#atuacao').selectpicker('refresh');
        }
    });
}

function getCep() {
    var cep = $('#cep').val();
    cep = cep.replace("-", "");
    return int.parse(cep);
}

function getProfissionalPorAtuacao(id) {
   
    var Url = "/Profissionais/BuscarServico/";
    //var Url = "http://localhost:5300/api/seguranca/wpProfissionais/BuscarServico/1/999";
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": Url,
        "method": "GET"
    }

    $.ajax(settings).done(function (response) {
        //loadSelect

        $("#profissional").empty();
        $('#profissional').append('<option value="0">Selecione</option>');
        $('#profissional').selectpicker('refresh');
        $('#profissional').focus();

        for (var i = 0; i < response.length; i++) {
            if (response[i].servicoTipoId == id) {

                $('#profissional').append('<option value="' + response[i].id + '">' + response[i].nome + '</option>');
                $('#profissional').selectpicker('refresh');
            }

        }

        LoadingStop('body');
    });
}

function getForm() {
    return {
        nome: $('#nome'),
        cep: $('#cep'),
        rua: $('#rua'),
        bairro: $('#bairro'),
        cidade: $('#cidade'),
        complemento: $('#complemento'),
        uf: $('#uf'),
        data: $('#data'),
        hora: $('#txtHoraInicio'),
        qtd: $('#qtd'),
        valor: $('#valor'),
        atuacao: $('#atuacao'),
        profissional: $('#profissional'),
        referencia: $('#referencia')
    }
}

function LoadingInit(elemento) {
    $(elemento).loading({
        theme: 'dark',
        message: 'Aguarde...',
        onStart: function (loading) {
            loading.overlay.slideDown(400);
        },
        onStop: function (loading) {
            loading.overlay.slideUp(400);
        }
    });
}

function LoadingInitBase(elemento) {
    $(elemento).loading({
        theme: 'dark',
        message: 'Aguarde...'
    });
}

function LoadingStop(elemento) {
    $(elemento).loading('stop');
}

function LoadingBodyStop() {
    $('body').loading('stop');
}

function init() {
    $('.selectpicker').selectpicker();
    $("#miolo").addClass("card-wizard");
    $(".pnProfissional").hide();
    $(".pnValores").hide();
}

function calcularValorContratacao() {
    var valor = $('#valor').val();
    var qtd = $('#qtd').val();
    var total = parseFloat(valor.replace(",", ".")) * qtd;
    return "R$" + total;
}

function limpaEndereco() {
    $("#cep").val("");
    $('#rua').val("");
    $('#bairro').val("");
    $('#cidade').val("");
    $('#uf').val("");
}

function desbloqueiaEndereco() {
    $("#rua").prop("disabled", false);
    $("#bairro").prop("disabled", false);
    $("#cidade").prop("disabled", false);
    $("#uf").prop("disabled", false);
    $("#numero").focus();

}

function bloqueiaEndereco() {
    $("#rua").prop("disabled", true);
    $("#bairro").prop("disabled", true);
    $("#cidade").prop("disabled", true);
    $("#uf").prop("disabled", true);
    $("#numero").focus();
    $('.card-body').loading('stop');
}

function preencherEndereco(cep) {
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": "/Enderecos/BuscarEnderecoPorCep",
        "method": "POST",
        "headers": {
            "Content-Type": "application/json",
            "Cache-Control": "no-cache",
            "Postman-Token": "fa43c255-9862-4a93-a554-47cb98066396"
        },
        "processData": false,
        "data": "{\"cep\": \"" + cep + "\"}"
    }

    LoadingInit('body');
    $.ajax(settings).done(function (response) {
        if (response.endereco == '' || response.endereco == null) {
            demo.showNotification('top', 'right', 'Por favor digite um CEP válido!');
            limpaEndereco();
            desbloqueiaEndereco();
            $("#cep").focus();
            //LoadingStop('body');
        }
        else {
            $('#rua').val(response.endereco);
            $('#bairro').val(response.bairro);
            $('#cidade').val(response.cidade);
            $('#uf').val(response.uf);

            bloqueiaEndereco();
            //LoadingStop('body');
        }

    }).always(function () {
        LoadingStop('body');
    });


}

function carregaModal() {

    LoadingInit('body');
    var vagaViewModel = VagaViewModel();
    console.log(JSON.stringify(vagaViewModel));

    $.ajax({
        url: "/Vaga/ModalConfirmarVaga",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(vagaViewModel),
        success: function (response) {
            $('#modal').html(response);
            $('#myModal').modal('show');
            LoadingBodyStop();
        },
        error: function (erro) {
            console.log(erro);
        }
    });
  
    return true;
}

function controlarPaineis() {

    getForm().valor.on("change", function () {
        $("#total").val(calcularValorContratacao());
        $("#qtd").focus();
    });

    getForm().complemento.on("change", function () {
        $("#referencia").focus();
    });

    //getForm().referencia.on("change", function () {
    //    $("#data").focus();
    //});
    getForm().qtd.on("change", function () {
        $("#total").val(calcularValorContratacao());
    });

    getForm().atuacao.on("change", function () {
        LoadingInitBase('body');
        getProfissionalPorAtuacao($(this).val());
        $(".pnProfissional").fadeIn();
    });

    getForm().profissional.on("change", function () {
        $(".pnValores").fadeIn();
        $("#valor").focus();

    });

    getForm().nome.on("change", function () {

        $("#cep").focus();
    });

    getForm().cep.on("change", function () {
        preencherEndereco($(this).val());
    });

    getForm().data.on("change", function () {
        valiidarData();
    });

    getForm().hora.on("change", function () {

        $("#atuacao").focus();
    });

}

function getData() {
    var data = getFormData().data;
    return Date.parse(data);
}

function parseDate(input) {
    var parts = input.match(/(\d+)/g);
    // new Date(year, month [, date [, hours[, minutes[, seconds[, ms]]]]])
    return new Date(parts[0], parts[1] - 1, parts[2]); // months are 0-based
}

function valiidarData() {

    var data = parseDate(getFormData().data);

    var date = new Date();
    var hoje = date.getFullYear() + "-" + date.getMonth() + "-" + date.getDay();

    var month = date.getMonth() + 1;
    var day = date.getDate();

    var output = date.getFullYear() + '/' +
        (month < 10 ? '0' : '') + month + '/' +
        (day < 10 ? '0' : '') + day;

    if (Date.parse(data) < Date.parse(output)) {
        demo.showNotification('top', 'right', 'Não é possível cadastrar oportunidades retroativas!');
        $('#data').val(" ");
        $('#data').focus();
        return false;
    }
    else {
        return true;
    }

}

function validarCampos() {
    var form = getFormData();

    if (form.nome == "" || form.nome == null) {
        demo.showNotification('top', 'right', 'Campo nome é obrigatório!');
        $('#nome').focus();
        return false;
    }
    else if (form.cep == "" || form.rua == "" || form.cidade == "" || form.uf == "" || form.bairro == "") {
        demo.showNotification('top', 'right', 'Informe um endereço válido!');
        $('#cep').focus();
        return false;
    }
    //else if (form.numero == "" || form.numero == null) {
    //    demo.showNotification('top', 'right', 'Digite um numero para o endereço!');
    //    $('#numero').focus();

    //    return null;
    //}

    //else if (valiidarData() == false) {
    //    return false;
    //}

    else if (form.valor == "" || form.valor == null) {
        demo.showNotification('top', 'right', 'Informe o valor unitário da contratação!');
        $('#valor').focus();
        return null;
    }
    else if (form.qtd <= 0 || form.qtd == "" || form.qtd == null) {
        demo.showNotification('top', 'right', 'Informe uma quantidade válida!');
        $('#qtd').focus();
        return null;
    }
    else if (form.profissional == "" || form.profissional == 0) {
        demo.showNotification('top', 'right', 'selecione o profissional!');
        $('#profissional').focus();
        return null;
    }
    else if (form.hora == null || form.hora == "")
    {
        demo.showNotification('top', 'right', 'Informe o horário de início!');
        $('#hora').focus();
        return null;
    }
    else
        return true;
}