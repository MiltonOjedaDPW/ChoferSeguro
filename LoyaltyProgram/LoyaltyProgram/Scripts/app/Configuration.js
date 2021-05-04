var regla; var GETREGLASID; var GETREGLAS; var GETEVENTOS; var GETRECOMPENSAS; var idRecompensa; var idEvento; var idCampania; var idRegla; var root; var img;
var xpuntosPositivos;
$(document).ready(function () {



    $('#btnCreaRegla').click(function () {
        $('#ReglaModalCenter').modal('show');
    });

    $('#btnRecompensa').click(function () {
        $('#RecompensaModalCenter').modal('show');
    });
    $('#btnEvento').click(function () {
        $('#EventoModalCenter').modal('show');
    });
    $('#btnCreaCampaña').click(function () {
        $('#CampanaModalCenter').modal('show');
    });

    $('#pickerFechaCanje').datetimepicker({
        format: 'DD-MMM-YYYY'
    });
    $('#pickerFechaFin').datetimepicker({
        format: 'DD-MMM-YYYY'
    });
    $('#pickerFechaInicio').datetimepicker({
        format: 'DD-MMM-YYYY'
    });
    $('#pickerFechaFinUPD').datetimepicker({
        format: 'DD-MMM-YYYY'
    });
    $('#pickerFechaInicioUPD').datetimepicker({
        format: 'DD-MMM-YYYY'
    });
    $('#pickerFechaCanjeUPD').datetimepicker({
        format: 'DD-MMM-YYYY'
    });

    $('#pickerIncidenciaDateUpdate').datetimepicker({
        format: 'L'
    });

    $('#btnCancelar,#btnCancelar1 ,#btnCancelar2 ,#btnCancelar3,#btnCancelar4,#btnCancelar5,#btnCancelar6,#btnCancelar7').click(function () {
        $('#txtRegla, #txtDescripcion, #txtEvento, #txtPuntaje, #txtArtículo, #txtValorPuntos, #txtStock, #txtDescripcionRecom, #txtCampania, #FechaDeInicio, #FechaDeFin, #FechaDeCanje').val("");
    });


    //$('#CheckHabilitar').change(function () {
    //    if (this.checked) {
    //        $(".data-inputEvento").prop('disabled', false);
    //    } else {
    //        $(".data-inputEvento").prop('disabled', true);
    //    }
    //});


    var tableReglas = $('#tableReglas').DataTable({
        "ajax": {
            "url": GETREGLAS,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
             { "data": "REGLA" },
             //{ "data": "ICON" },
             { "data": "DESCRIPCION" },
             {
                 "data": "ACTIVO", render: function (data, type, row) {
                     switch (data) {
                         case true: return "SI"; break;
                         case false: return "NO"; break;
                     }
                 }
             },
             {
                 "data": null, render: function (data, type, row) {

                     return '<button data-name=\"' + data.REGLA + '"\ id=\"' + data.ID_REGLA + '"\ data-activo=\"' + data.ACTIVO + '"\  data-icon=\"' + data.ICON + '"\ data-descrip=\"' + data.DESCRIPCION + '"\ type="button" style="width:100px" class="btn-updateRegla btn btn-light" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Editar</button>'
                     Console.log(data);
                 }
             },
        ],
        "rowCallback": function (row, data, index) {
            switch (data.ACTIVO) {
                case true: return $('td', row).addClass("activado"); break;
                case false: return $('td', row).addClass("desacticado"); break;
            }
        },
        //"scrollX": true
    });

    var tableEvento = $('#tablaEventos').DataTable({
        "ajax": {
            "url": GETEVENTOS,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
             { "data": "EVENTO" },
             { "data": "PUNTAJE" },
             {
                 "data": "ACTIVO", render: function (data, type, row) {
                     switch (data) {
                         case true: return "SI"; break;
                         case false: return "NO"; break;
                     }
                 }
             },
             {
                 "data": null, render: function (data, type, row) {

                     return '<button id=\"' + data.ID_EVENTO + '"\ data-name=\"' + data.EVENTO + '"\ data-activo=\"' + data.ACTIVO + '"\  data-puntaje=\"' + data.PUNTAJE + '"\ type="button" style="width:100px" class="btn-updateEvento btn btn-light" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Editar</button>'
                     console.log(data);
                 }
             },
        ],
        "rowCallback": function (row, data, index) {
            switch (data.ACTIVO) {
                case true: return $('td', row).addClass("activado"); break;
                case false: return $('td', row).addClass("desacticado"); break;
            }
        },
        //"scrollX": true
    });

    var tableRecompensas = $('#tablaRecompensa').DataTable({
        destroy: true,
        "ajax": {
            "url": GETRECOMPENSAS,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
             { "data": "NOMBRE" },
             {
                 "data": "IMAGEN", render: function (data, type, row) {
                     return '<img src=\"' + root + data + '"\ alt=\"' + data + '"\ class="img-fluid" />'
                 }
             },
             { "data": "DESCRIPCION" },
             { "data": "VALOR_PUNTOS" },
             { "data": "STOCK" },
             { "data": "STR_CREATED_DATE" },
             {
                 "data": "ACTIVO", render: function (data, type, row) {
                     switch (data) {
                         case true: return "SI"; break;
                         case false: return "NO"; break;
                     }
                 }
             },
             {
                 "data": null, render: function (data, type, row) {
                     return '<button id=\"' + data.ID_RECOMPENSA + '"\ data-descrip=\"' + data.DESCRIPCION + '"\ data-activo=\"' + data.ACTIVO + '"\ data-img=\"' + data.IMAGEN + '"\ data-stock=\"' + data.STOCK + '"\ data-valorPuntos=\"' + data.VALOR_PUNTOS + '"\ data-articulo=\"' + data.NOMBRE + '"\ type="button" style="width:100px" class="btn-updateRecompensa btn btn-light" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Editar</button>'
                     console.log(data);
                 }
             },
        ],
        "rowCallback": function (row, data, index) {
            switch (data.ACTIVO) {
                case true: return $('td', row).addClass("activado"); break;
                case false: return $("td", row).addClass("desacticado"); break;
            }
        },
        //"scrollX": true
    });

    var tableCampania = $('#tablaCampanias').DataTable({
        "ajax": {
            "url": GETCAMPANIAS,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
             { "data": "CAMPANIA" },
             { "data": "STR_DATE_START" },
             { "data": "STR_DATE_END" },
             { "data": "STR_FECHA_CANJE" },
             {
                 "data": "ACTIVO", render: function (data, type, row) {
                     switch (data) {
                         case true: return "SI"; break;
                         case false: return "NO"; break;
                     }
                 }
             },
             {
                 "data": null, render: function (data, type, row) {

                     return '<button id=\"' + data.ID_CAMPANIA + '"\ data-name=\"' + data.CAMPANIA + '"\ data-activo=\"' + data.ACTIVO + '"\ data-fechaCanje=\"' + data.STR_FECHA_CANJE + '"\  type="button" style="width:100px" class="btn-updateCampania btn btn-light" ><i class="fa fa-pencil-square-o" aria-hidden="true"></i> Editar</button>'
                     console.log(data);
                 }
             },
        ],
        "rowCallback": function (row, data, index) {
            switch (data.ACTIVO) {
                case true: return $('td', row).addClass("activado"); break;
                case false: return $('td', row).addClass("desacticado"); break;
            }
        },
        //"scrollX": true
    });

    function InsertUpdRegla(id, regla, icon, descrip, activo) {
        var parametersIR =
                            {
                                ID_REGLA: id,
                                REGLA: regla,
                                ICON: icon,
                                DESCRIPCION: descrip,
                                CREATED_BY: "milton",
                                ACTIVO: activo
                            };
        console.log(parametersIR);

        $.confirm({
            title: '<i class="fa fa-check-circle text-success"></i> Confirmación!',
            type: "blue",
            content: '¿Guardar cambios?',
            buttons: {
                confirm: function () {
                    $.post(INSERTREGLA, parametersIR)
                .done(function (data) {

                    $.alert({
                        title: '<i class="fa fa-info-circle text-primary"></i> Información!',
                        content: "Regla Guardada",
                        animation: 'none',
                        type: 'blue',
                        buttons: {
                            ok: {
                                text: "OK",
                                btnClass: "btn-blue",
                                action: function () {
                                    //$(this).css(csstxtClean);
                                }
                            }
                        }
                    });
                    tableReglas.ajax.reload();
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.alert({
                        title: '<i class="fa fa-exclamation-triangle text-danger" aria-hidden="true"></i> Error!',
                        content: "Error: " + errorThrown,
                        animation: 'none',
                        type: 'red',
                        buttons: {
                            ok: {
                                text: "OK",
                                btnClass: "btn-red",
                                action: function () {
                                    //$(this).css(csstxtClean);
                                }
                            }
                        }
                    });
                });
                    $('#ReglaModalModificarCenter').modal('hide');
                    $('#ReglaModalCenter').modal('hide');
                },
                cancel: function () {
                    //cancelado
                }
            }
        });


    }

    function InsertUpdRecompensa(id, articulo, imagen, descrip, valPuntos, stock, activo) {
        var parametersIR =
                            {
                                ID_RECOMPENSA: id,
                                NOMBRE: articulo,
                                IMAGEN: imagen,
                                DESCRIPCION: descrip,
                                VALOR_PUNTOS: valPuntos,
                                STOCK: stock,
                                CREATED_BY: "milton",
                                ACTIVO: activo
                            };
        console.log(parametersIR);
        $.confirm({
            title: '<i class="fa fa-check-circle text-success"></i> Confirmación!',
            type: "blue",
            content: '¿Guardar cambios?',
            buttons: {
                confirm: function () {
                    $.post(INSERTRECOMPENSA, parametersIR)
                .done(function (data) {

                    $.alert({
                        title: '<i class="fa fa-info-circle text-primary"></i> Información!',
                        content: "Recompensa Guardada",
                        animation: 'none',
                        type: 'blue',
                        buttons: {
                            ok: {
                                text: "OK",
                                btnClass: "btn-blue",
                                action: function () {
                                    //$(this).css(csstxtClean);
                                }
                            }
                        }
                    });
                    tableRecompensas.ajax.reload();
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.alert({
                        title: '<i class="fa fa-exclamation-triangle text-danger" aria-hidden="true"></i> Error!',
                        content: "Error: " + errorThrown,
                        animation: 'none',
                        type: 'red',
                        buttons: {
                            ok: {
                                text: "OK",
                                btnClass: "btn-red",
                                action: function () {
                                    //$(this).css(csstxtClean);
                                }
                            }
                        }
                    });
                });
                    $('#RecompensaModalCenter').modal('hide');
                    $('#RecompensaUPDModalCenter').modal('hide');
                },
                cancel: function () {
                    //cancelado
                }
            }
        });
        //$.post(INSERTRECOMPENSA, parametersIR)
        //        .done(function (data) {
        //            alert("Registro Guardado");

        //            tableRecompensas.ajax.reload();
        //        })
        //        .fail(function (jqXHR, textStatus, errorThrown) {
        //            alert("Error al guardar: " + errorThrown);
        //        });
    }

    function InsertUpdEvento(id, evento, puntaje, activo) {

        var parametersIE =
                {
                    ID_EVENTO: id,
                    EVENTO: evento,
                    PUNTAJE: puntaje,
                    CREATED_BY: "milton",
                    ACTIVO: activo
                };

        console.log(parametersIE);

        $.confirm({
            title: '<i class="fa fa-check-circle text-success"></i> Confirmación!',
            type: "blue",
            content: '¿Guardar cambios?',
            buttons: {
                confirm: function () {
                    $.post(INSERTEVENTO, parametersIE)
                .done(function (data) {

                    $.alert({
                        title: '<i class="fa fa-info-circle text-primary"></i> Información!',
                        content: "Evento Guardado",
                        animation: 'none',
                        type: 'blue',
                        buttons: {
                            ok: {
                                text: "OK",
                                btnClass: "btn-blue",
                                action: function () {
                                    //$(this).css(csstxtClean);
                                }
                            }
                        }
                    });
                    tableEvento.ajax.reload();
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.alert({
                        title: '<i class="fa fa-exclamation-triangle text-danger" aria-hidden="true"></i> Error!',
                        content: "Error: " + errorThrown,
                        animation: 'none',
                        type: 'red',
                        buttons: {
                            ok: {
                                text: "OK",
                                btnClass: "btn-red",
                                action: function () {
                                    //$(this).css(csstxtClean);
                                }
                            }
                        }
                    });
                });
                    $('#EventoUPDModalCenter').modal('hide');
                    $('#EventoModalCenter').modal('hide');
                },
                cancel: function () {
                    //cancelado
                }
            }
        });

        //$.post(INSERTEVENTO, parametersIE)
        //        .done(function (data) {
        //            alert("Registro Guardado");
        //            $('#EventoUPDModalCenter').modal('hide');
        //            $('#EventoModalCenter').modal('hide');
        //            tableEvento.ajax.reload();
        //        })
        //        .fail(function (jqXHR, textStatus, errorThrown) {
        //            alert("Error al guardar: " + errorThrown);
        //        });

    }

    function CrearCampania(id, campania, date_start, date_end, fecha_canje, activo) {

        var paramsCampania =
                {
                    ID_CAMPANIA: id,
                    CAMPANIA: campania,
                    DATE_START: date_start,
                    DATE_END: date_end,
                    FECHA_CANJE: fecha_canje,
                    ACTIVO: activo
                };

        console.log(paramsCampania);

        $.confirm({
            title: '<i class="fa fa-check-circle text-success"></i> Confirmación!',
            type: "blue",
            content: '¿Guardar cambios?',
            buttons: {
                confirm: function () {
                    $.post(CREARCAMPANIA, paramsCampania)
                .done(function (data) {

                    $.alert({
                        title: '<i class="fa fa-info-circle text-primary"></i> Información!',
                        content: "Evento Guardado",
                        animation: 'none',
                        type: 'blue',
                        buttons: {
                            ok: {
                                text: "OK",
                                btnClass: "btn-blue",
                                action: function () {
                                    //$(this).css(csstxtClean);
                                }
                            }
                        }
                    });
                    tableCampania.ajax.reload();
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.alert({
                        title: '<i class="fa fa-exclamation-triangle text-danger" aria-hidden="true"></i> Error!',
                        content: "Error: " + errorThrown,
                        animation: 'none',
                        type: 'red',
                        buttons: {
                            ok: {
                                text: "OK",
                                btnClass: "btn-red",
                                action: function () {
                                    //$(this).css(csstxtClean);
                                }
                            }
                        }
                    });
                });
                    $('#CampanaUPDModalCenter').modal('hide');
                    $('#CampanaModalCenter').modal('hide');
                },
                cancel: function () {
                    //cancelado
                }
            }
        });

        //$.post(INSERTEVENTO, parametersIE)
        //        .done(function (data) {
        //            alert("Registro Guardado");
        //            $('#EventoUPDModalCenter').modal('hide');
        //            $('#EventoModalCenter').modal('hide');
        //            tableEvento.ajax.reload();
        //        })
        //        .fail(function (jqXHR, textStatus, errorThrown) {
        //            alert("Error al guardar: " + errorThrown);
        //        });

    }

    function UpdateCampania(id, campania, fecha_canje, activo) {

        var paramsCampania =
                {
                    ID_CAMPANIA: id,
                    CAMPANIA: campania,
                    FECHA_CANJE: fecha_canje,
                    ACTIVO: activo
                };

        console.log(paramsCampania);

        $.confirm({
            title: '<i class="fa fa-check-circle text-success"></i> Confirmación!',
            type: "blue",
            content: '¿Guardar cambios?',
            buttons: {
                confirm: function () {
                    $.post(UPDATECAMPANIA, paramsCampania)
                .done(function (data) {

                    $.alert({
                        title: '<i class="fa fa-info-circle text-primary"></i> Información!',
                        content: "Evento Guardado",
                        animation: 'none',
                        type: 'blue',
                        buttons: {
                            ok: {
                                text: "OK",
                                btnClass: "btn-blue",
                                action: function () {
                                    //$(this).css(csstxtClean);
                                }
                            }
                        }
                    });
                    tableCampania.ajax.reload();
                })
                .fail(function (jqXHR, textStatus, errorThrown) {
                    $.alert({
                        title: '<i class="fa fa-exclamation-triangle text-danger" aria-hidden="true"></i> Error!',
                        content: "Error: " + errorThrown,
                        animation: 'none',
                        type: 'red',
                        buttons: {
                            ok: {
                                text: "OK",
                                btnClass: "btn-red",
                                action: function () {
                                    //$(this).css(csstxtClean);
                                }
                            }
                        }
                    });
                });
                    $('#CampanaUPDModalCenter').modal('hide');
                    $('#CampanaModalCenter').modal('hide');
                },
                cancel: function () {
                    //cancelado
                }
            }
        });

        //$.post(INSERTEVENTO, parametersIE)
        //        .done(function (data) {
        //            alert("Registro Guardado");
        //            $('#EventoUPDModalCenter').modal('hide');
        //            $('#EventoModalCenter').modal('hide');
        //            tableEvento.ajax.reload();
        //        })
        //        .fail(function (jqXHR, textStatus, errorThrown) {
        //            alert("Error al guardar: " + errorThrown);
        //        });

    }

    $("#btnInsertRegla").click(function (evt) {
        let regla = $('#txtRegla').val();
        //let icon = $('#txtIcon').val();
        let descrip = $('#txtDescripcion').val();
        InsertUpdRegla(-1, regla, "fas-test", descrip, 0);
    });

    $("#btnUpdateRegla").click(function () {
        let regla = $('#txtReglaUpd').val();
        let icon = "fas-test" //$('#txtIconUpd').val();
        let descrip = $('#txtDescripcionUpd').val();
        //let activo = $('#txtActivoUpd').val();
        let activo = $('#switchRegla:checked').val();
        switch (activo) {
            case undefined: activo = "false"; break;
            case "on": activo = "true"; break;
        }
        //var id = $(".btn-updateRegla").attr("data-id"); console.log(id);
        InsertUpdRegla(idRegla, regla, icon, descrip, activo);
    });

    $("#btnInsertEvento").click(function (evt) {
        let evento = $('#txtEvento').val(); console.log(evento);
        let puntaje = $('#txtPuntaje').val(); console.log(puntaje);
        InsertUpdEvento(-1, evento, puntaje, 1);
    });

    $("#btnUpdateEvento").click(function () {
        let evento = $('#txtEventoUPD').val();
        let puntaje = $('#txtPuntajeUPD').val();
        let activo = $('#switchEvento:checked').val();
        switch (activo) {
            case undefined: activo = "false"; break;
            case "on": activo = "true"; break;
        }
        //var id = $(".btn-updateEvento").attr("id"); console.log(id);
        InsertUpdEvento(idEvento, evento, puntaje, activo);
    });

    $("#btnInsertRecompensa").click(function () {

        var form = $("#drawingForm");
        var image = document.getElementsByTagName("canvas"); console.log(image);
        image = image[0].toDataURL("image/png");
        image = image.replace('data:image/png;base64,', '');
        $("#imageData").val(image);
        //form.submit();

        let articulo = $('#txtArtículo').val(); console.log(articulo);
        let valorPuntos = $('#txtValorPuntos').val(); console.log(valorPuntos);
        let stock = $('#txtStock').val(); console.log(stock);
        let descripcion = $('#txtDescripcionRecom').val(); console.log(descripcion);
        let img = image;

        //let img = $('.file-preview-image').attr('title'); console.log(img);
        InsertUpdRecompensa(-1, articulo, img, descripcion, valorPuntos, stock, 1);
    });

    $("#btnUpdateRecompensa").click(function () {
        let articulo = $('#txtArtículoUpd').val(); console.log(articulo);
        let valorPuntos = $('#txtValorPuntosUpd').val(); console.log(valorPuntos);
        let stock = $('#txtStockUpd').val(); console.log(stock);
        let descripcion = $('#txtDescripcionRecomUpd').val(); console.log(descripcion);
        let activo = $('#switchRecompensa:checked').val();
        switch (activo) {
            case undefined: activo = "false"; break;
            case "on": activo = "true"; break;
        }
        //var id = $(".btn-updateEvento").attr("id"); console.log(id);
        InsertUpdRecompensa(idRecompensa, articulo, img, descripcion, valorPuntos, stock, activo);
    });//descripcion, valorPuntos, stock, activo

    $("#btnInsertCampaña").click(function () {
        let campania = $('#txtCampania').val(); console.log(campania);
        let date_st = $('#FechaDeInicio').val(); console.log(date_st);
        let date_end = $('#FechaDeFin').val(); console.log(date_end);
        let fe_canje = $('#FechaDeCanje').val(); console.log(fe_canje);
        CrearCampania(-1, campania, date_st, date_end, fe_canje, 1);

    });

    $("#btnUpdateCampaña").click(function () {
        let campania = $('#txtCampaniaUPD').val(); console.log(campania);
        let fe_canje = $('#FechaDeCanjeUPD').val(); console.log(fe_canje);
        let activo = $('#switchCampania:checked').val();

        switch (activo) {
            case undefined: activo = "false"; break;
            case "on": activo = "true"; break;
        }
        UpdateCampania(idCampania, campania, fe_canje, activo);
    });

    $('#tablaRecompensa').on('click', '.btn-updateRecompensa', function () {
        idRecompensa = $(this).attr("id"); console.log(idRecompensa);
        var articulo = $(this).attr("data-articulo"); console.log(articulo);
        var valorPuntos = $(this).attr("data-valorPuntos"); console.log(valorPuntos);
        var stock = $(this).attr("data-stock"); console.log(stock);
        var descripcion = $(this).attr("data-descrip"); console.log(descripcion);
        //var root = $('#img_Update').attr("data-root"); console.log(root);
        img = $(this).attr("data-img"); console.log(img);
        var activo = $(this).attr("data-activo"); console.log(typeof activo);
        console.log(activo);

        $('#txtArtículoUpd').val(articulo);
        $('#txtValorPuntosUpd').val(valorPuntos);
        $('#txtStockUpd').val(stock);
        $('#txtDescripcionRecomUpd').val(descripcion);
        let src = root + img;
        $('#img_Update').attr("src", src);

        switch (activo) {
            case "true": $('#switchRecompensa').prop('checked', true); break;
            case "false": $('#switchRecompensa').prop('checked', false); break;
        }
        //$('#txtActivoRUpd').val(activo);

        $('#RecompensaUPDModalCenter').modal('show');

    });

    $('#tablaEventos').on('click', '.btn-updateEvento', function () {
        idEvento = $(this).attr("id"); console.log(idEvento);
        var evento = $(this).attr("data-name"); console.log(evento);
        var puntaje = $(this).attr("data-puntaje"); console.log(puntaje);
        var activo = $(this).attr("data-activo"); console.log(activo);

        $('#txtEventoUPD').val(evento);
        $('#txtPuntajeUPD').val(puntaje);
        //$('#txtActivoE').val(activo);
        switch (activo) {
            case "true": $('#switchEvento').prop('checked', true); break;
            case "false": $('#switchEvento').prop('checked', false); break;
        }
        $('#EventoUPDModalCenter').modal('show');
    });

    $('#tableReglas').on('click', '.btn-updateRegla', function () {

        idRegla = $(this).attr("id"); console.log(idRegla);
        var regla = $(this).attr("data-name"); console.log(regla);
        //var icon = $(this).attr("data-icon"); console.log(icon);
        var descripcion = $(this).attr("data-descrip"); console.log(descripcion);
        var activo = $(this).attr("data-activo"); console.log(activo);

        $('#txtReglaUpd').val(regla);
        //$('#txtIconUpd').val(icon);
        $('#txtDescripcionUpd').val(descripcion);

        switch (activo) {
            case "true": $('#switchRegla').prop('checked', true); break;
            case "false": $('#switchRegla').prop('checked', false); break;
        }
        //if (activo == "true") {
        //     console.log($('#switchRegla:checked').val());
        //}
        //if (activo == "false") {
        //     console.log($('#switchRegla:checked').val());
        //}
        //$('#txtActivoUpd').val(activo);

        $('#ReglaModalModificarCenter').modal('show');

    });


    $('#tablaCampanias').on('click', '.btn-updateCampania', function () {
        idCampania = $(this).attr("id"); console.log(idCampania);
        var campania = $(this).attr("data-name"); console.log(campania);
        var fecha_canje = $(this).attr("data-fechaCanje"); console.log(campania);
        var activo = $(this).attr("data-activo"); console.log(activo);

        $('#txtCampaniaUPD').val(campania);
        $('#FechaDeCanjeUPD').val(fecha_canje);
        $('#txtActivoE').val(activo);
        switch (activo) {
            case "true": $('#switchCampania').prop('checked', true); break;
            case "false": $('#switchCampania').prop('checked', false); break;
        }
        $('#CampanaUPDModalCenter').modal('show');
    });

    $("#btnSearchRnttManual").click(function () {
        let rntt = $("#txtRNTTmanual").val(); console.log(rntt);
        if (rntt != "") {
            $.get(GETCHOFERES, { parametro: rntt })
                    .done(function (data) {

                        console.log(data);
                        if (data.data[0] != undefined) {
                            $("#txtNameFullManual").val(data.data[0].CHOFER);
                            $("#txtCedulaManual").val(data.data[0].CEDULA);
                            if (data.data[0].ESTATUS_AFILACION == "ACTIVO") { $("#statusC").text(data.data[0].ESTATUS_AFILACION).addClass("text-primary"); console.log(data.data[0].ESTATUS_AFILACION); }
                            else { $("#statusC").text(data.data[0].ESTATUS_AFILACION).addClass("text-warning"); }
                        } else {
                            $("#txtNameFullManual").val("");
                            $("#txtCedulaManual").val("");
                            $("#statusC").text("");
                            $.alert({
                                title: '<i class="fa fa-info-circle text-warning"></i> Información!',
                                content: "Chofer no encontrado",
                                animation: 'none',
                                type: 'orange',
                                buttons: {
                                    ok: {
                                        text: "OK",
                                        btnClass: "btn-orange",
                                        action: function () {
                                            //$(this).css(csstxtClean);
                                        }
                                    }
                                }
                            });
                        }
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        alert("Error: " + errorThrown);
                    });
        } else {
            $("#txtNameFull").val(""); $("#txtCedula").val(""); $("#txtEmpresa").val(""); $("#statusC").text("");
            $.alert({
                title: '<i class="fa fa-info-circle text-warning"></i> Información!',
                content: "Ingrese RNTT",
                animation: 'none',
                type: 'orange',
                buttons: {
                    ok: {
                        text: "OK",
                        btnClass: "btn-orange",
                        action: function () {
                            //$(this).css(csstxtClean);
                        }
                    }
                }
            });
        }
    });


    $('#btnRegistroManual').click(function () {

        let rntt = $("#txtRNTTmanual").val(); console.log(rntt);
        let nombre = $("#txtNameFullManual").val(); console.log(rntt);
        let cedula = $("#txtCedulaManual").val(); console.log(rntt);
        let descrip = $("#commentManual").val(); console.log(rntt);
        var tipoEvento = xpuntosPositivos.data.getSelected().innerHTML; console.log(tipoEvento);
        let puntos = xpuntosPositivos.data.getSelected().value; console.log(puntos);

        var parametros =
                            {
                                RNTT: rntt,
                                NOMBRE: nombre,
                                CEDULA: cedula,
                                DESCRIPCION: descrip,
                                TIPO_EVENTO: tipoEvento,
                                PUNTOS: puntos,
                                CREATED_BY: "milton"
                            };
        console.log(parametros);
        if (rntt != "" || descrip != "") {
            $.confirm({
                title: '<i class="fa fa-check-circle text-success"></i> Confirmación!',
                type: "blue",
                content: '¿Guardar cambios?',
                buttons: {
                    confirm: function () {
                        $.post(CREARREGISTROMANUAL, parametros)
                    .done(function (data) {

                        $.alert({
                            title: '<i class="fa fa-info-circle text-primary"></i> Información!',
                            content: "Registro Manual Guardada",
                            animation: 'none',
                            type: 'blue',
                            buttons: {
                                ok: {
                                    text: "OK",
                                    btnClass: "btn-blue",
                                    action: function () {
                                        $("#txtRNTTmanual").val("");
                                        $("#txtNameFullManual").val("");
                                        $("#txtCedulaManual").val("");
                                        $("#commentManual").val("");
                                        //$(this).css(csstxtClean);
                                    }
                                }
                            }
                        });

                        tableReglas.ajax.reload();
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        $.alert({
                            title: '<i class="fa fa-exclamation-triangle text-danger" aria-hidden="true"></i> Error!',
                            content: "Error: " + errorThrown,
                            animation: 'none',
                            type: 'red',
                            buttons: {
                                ok: {
                                    text: "OK",
                                    btnClass: "btn-red",
                                    action: function () {
                                        //$(this).css(csstxtClean);
                                    }
                                }
                            }
                        });
                    });
                    },
                    cancel: function () {
                        //cancelado
                    }
                }
            });
        } else {
            $.alert({
                title: '<i class="fa fa-info-circle text-warning"></i> Información!',
                content: "Hay campos por llenar",
                animation: 'none',
                type: 'orange',
                buttons: {
                    ok: {
                        text: "OK",
                        btnClass: "btn-orange",
                        action: function () {
                            //$(this).css(csstxtClean);
                        }
                    }
                }
            });
        }

    });


});