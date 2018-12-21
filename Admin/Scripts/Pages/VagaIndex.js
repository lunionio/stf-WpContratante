let avaliacoes = [];

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
    Loading('body');

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
        LoadingStop('body');
    });
}

function getVinculoProfissional() {
    Loading('body');

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
        LoadingStop('body');
    });
}

function getModalMatch(idOpt) {
    Loading('body');

    var Url = "vaga/ModalMatch?optId=" + idOpt;
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": Url,
        "method": "GET"
    };

    $.ajax(settings).done(function (response) {
        LoadingStop('body');

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
                "sZeroRecords": "Nenhuma candidatura registrada.",
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
                "sZeroRecords": "Nenhum profissional contratado.",
                "sInfo": "Mostrando oágina _PAGE_ de _PAGES_",
                "sInfoEmpty": "Nenhum dado para mostrar",
                "sInfoFiltered": "(Filtrado de _MAX_ registros)",
                "sSearch": "Pesquisar:",
            },
        });

        //$('select').each(function () {
        //    var select = $(this);
        //    console.log(select.attr('id'));
        //    var avaliacao = select.attr('id');
        //    select.barrating({
        //        theme: 'fontawesome-stars',
        //        initialRating: 5,
        //        readonly: true
        //    });
        //});
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
        try {

            var p = $.parseJSON(response);

            if (p.Id == undefined) {
                swal(response, "", "ERROR");
            }
            else {
                let table = $('#tbContratar').DataTable();
                table.row("#" + userId).remove().draw();
                var contratados = $('#tbContratados').DataTable();
                var row = contratados.row.add([
                    p.IdUsuario,
                    p.Nome,
                    p.Especialidade,
                    p.Endereco.Local,
                    p.Valor,
                    '<select id="' + p.Avaliacao + '" disabled>' +
                        '<option value = "' + Math.floor(p.Avaliacao) + '" >' + p.Avaliacao + '</option > ' +
                    '</select>'
                ]).draw(false);
            }
        }
        catch (e) {
            alert(response);
        }
    });
}

function reprovarProfissional(userXOpt, optId, userId) {
    try {
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

        Loading('body');

        $.ajax(settings).done(function (response) {
            var p = $.parseJSON(response);

            if (typeof p == 'object') {
                if (p.Id == undefined) {
                    swal(response, "", "ERROR");
                }
                else {
                    let table = $('#tbContratar').DataTable();
                    table.row("#" + userId).remove().draw();
                }
            }
            else {
                alert(response);
            }
        
            LoadingStop('body');
            });
        }
    catch (e) {
        alert(response);
    }
}

function getProfissional(id) {
    Loading('body');

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
        LoadingStop('body');
    });
}

function guardaCache(profissionalId, optId) {
    console.log(profissionalId);
    avaliacoes.forEach(function (item, index, array) {
        if (item.UsuarioAvaliadoId === profissionalId) {
            array.splice(index, 1);
        }
    });

    let avaliacao = {
        UsuarioAvaliadoId: profissionalId,
        OportunidadeId: optId,
        Estrelas: $('#' + profissionalId + ' option:selected').text()
    };

    avaliacoes.push(avaliacao);
}

function avaliarProfissionais() {
    var settings = {
        "async": true,
        "crossDomain": true,
        "url": "/Vaga/AvaliarProfissional",
        "method": "POST",
        "data": { viewModels: avaliacoes }
    };

    $.ajax(settings).done(function (response) {
        avaliacoes.forEach(function (item, index, array) {
            $('#' + item.UsuarioAvaliadoId).prop('disabled', true);
        });

        alert(response);
    });
}