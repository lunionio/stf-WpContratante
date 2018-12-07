let pagamentosParaLiberar = [];

$("#linhaPadrao").remove();

$('.carousel').carousel();

LoadPanels();

function LoadPanels() {
    getOportunidades();
    //getProfissionais();
    //getVinculoProfissional();
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
    Loading('body');
    var Url = "/Vaga/_listarOportunidades";
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

        LoadingStop('body');
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

function getModalMatch(idOpt) {

    var Url = "vaga/ModalMatch?optId=" + idOpt;
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": Url,
        "method": "GET"
    };

    $.ajax(settings).done(function (response) {

        $('#modal').html(response);
        $('#myModal').modal('show');

        $('#tbContratar').dataTable({
            "pagingType": "numbers",
            "columnDefs": [{
                "targets": "_all",
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

        $('#tbContratados').dataTable({
            "pagingType": "numbers",
            "columnDefs": [{
                "targets": "_all",
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

function aprovarProfissional(userXOpt, optId, userId) {
    var obj = {
        ID: userXOpt,
        UserId: userId,
        OportunidadeId: optId,
        StatusId: 1 //Aprovado
    };

    var settings = {
        "async": true,
        "crossDomain": true,
        "url": "/Vaga/Match",
        "method": "POST",
        "data": obj
    };

    $.ajax(settings).done(function (response) {
            var p = JSON.parse(response);

            if (p.Id == undefined) {
                alert(response);
            }
            else {
                let table = $('#tbContratar').DataTable();
                table.row("#" + userId).remove().draw();
                var contratados = $('#tbContratados').DataTable();
                var row = contratados.row.add([
                    p.Id,
                    p.Nome,
                    p.Especialidade,
                    p.Endereco.Local,
                    p.Valor,
                    'Avaliação'
                ]).draw(false);
            }
    });
}

function reprovarProfissional(userXOpt, optId, userId) {
    var obj = {
        ID: userXOpt,
        UserId: userId,
        OportunidadeId: optId,
        StatusId: 3 //Reprovado
    };

    var settings = {
        "async": true,
        "crossDomain": true,
        "url": "/Vaga/Match",
        "method": "POST",
        "data": obj
    };

    $.ajax(settings).done(function (response) {
            var p = JSON.parse(response);

            if (p.Id == undefined) {
                alert(response);
            }
            else {
                let table = $('#tbContratar').DataTable();
                table.row("#" + userId).remove().draw();
            }
    });
}

function getProfissional(id) {
    var Url = "/Vaga/ModalProfissional?pId=" + id;
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": Url,
        "method": "GET"
    };

    $.ajax(settings).done(function (response) {
        $('#modalProfissional').html(response);
        $('#profissionalModal').modal('show');
    });
}

function guardaCache(optId, profissionalId) {
    pagamentosParaLiberar.forEach(function (item, index, array) {
        if (item.Id === profissionalId) {
            array.splice(index, 1);
        }
    });

    let pagamento = {
        Id: profissionalId,
        OportunidadeId: optId,
        Status: $('#' + optId + ' option:selected').text(),
        StatusPagamento: $('#' + optId + ' option:selected').val()
    };

    pagamentosParaLiberar.push(pagamento);
}

function liberarPagamentos() {
    let settings = {
        "async": true,
        "crossDomain": true,
        "url": "/Vaga/LiberarPagamentos",
        "method": "POST",
        "data": { models: pagamentosParaLiberar }
    };

    $.ajax(settings).done(function (response) {
        alert(response);
    });
}

function getModalCheckIn(profissionalId) {
    var Url = "/Vaga/ModalCheckIn?pId=" + profissionalId;
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": Url,
        "method": "GET"
    };

    $.ajax(settings).done(function (response) {
        $('#modalCheckIn').html(response);
        $('#checkinModal').modal('show');
    });
}