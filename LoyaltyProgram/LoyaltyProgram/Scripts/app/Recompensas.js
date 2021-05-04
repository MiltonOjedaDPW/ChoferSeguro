var GETCHOFERES; var x; var GETRECOMPENSA; var totalPuntosCanjeados = 0; var recompensasList = []; var ENTREGARRECOMPENSA; var GETRECOMPENSAS; var RELOADDATA;
var GETLISTADOENTREGARECOMPENSAS;

$(document).ready(function () {

    function agregarFila(id, recompensa, valorPuntos) {
        var htmlTags = ['<tr><td>', recompensa, '</td>',
             '<td class="data-vpoints">', valorPuntos, '</td>',
             '<td><button id=', id, ' type="button" data-punto=', valorPuntos, ' data-toggle="tooltip" data-placement="top" title="Eliminar" class="btn btn-danger btn-eliminarRecom"><i class="fa fa-trash" aria-hidden="true"></i></button></td></tr>'].join("");

        $('#tableRecompensasCanjeadas tbody').append(htmlTags);
    }

    $('#btnRegistrarEntRecompensa').click(function () {
        recompensasList = [];
        $('td button[type="button"]').each(function () {
            let id_recompensa = $(this).attr('id'); console.log(id_recompensa);

            recompensasList.push(id_recompensa);
        });

        let rntt = $("#txtRNTT").val();
        let puntoss = $("#totalPuntos").text();

        var parametros =
          {
              rntt: rntt,
              recompensas: recompensasList,
              puntos_totales: puntoss
          };

        console.log(parametros);
        if (rntt != "" || puntoss != "0") {
            $.post(ENTREGARRECOMPENSA, parametros)
                        .done(function (data) {

                            $('#btnAgregarRecompensa').prop("disabled", true);
                            $.alert({
                                title: '<i class="fa fa-info-circle text-primaty"></i> Información!',
                                content: "Registro Guardado",
                                animation: 'none',
                                type: 'blue',
                                buttons: {
                                    ok: {
                                        text: "OK",
                                        btnClass: "btn-blue",
                                        action: function () {
                                            location.reload();
                                            //$(this).css(csstxtClean);
                                        }
                                    }
                                }
                            });
                            $("#txtNameFull").val("");
                            $("#txtCedula").val("");
                            $("#txtEmpresa").val("");
                            $("#txtPuntosAcumulados").val("");
                            $("#statusC").text("");
                            $("#txtRNTT").val("");
                            $("#body").empty(); $("#totalPuntos").text("0"); totalPuntosCanjeados = 0;
                        })
                        .fail(function (jqXHR, textStatus, errorThrown) {
                            $('#btnAgregarRecompensa').prop("disabled", true);
                            $("#txtRNTT").val("");
                            $("#txtNameFull").val("");
                            $("#txtCedula").val("");
                            $("#txtEmpresa").val("");
                            $("#txtPuntosAcumulados").val("");
                            $("#statusC").text("");
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
        } else {
            $('#btnAgregarRecompensa').prop("disabled", true);
            $.alert({
                title: '<i class="fa fa-info-circle text-warning"></i> Información!',
                content: "Hay campos por completar",
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


        console.log(recompensasList);
    });

    $('#btnAgregarRecompensa').click(function () {
        let puntosRestantes = $('#puntos_restantes').text();
        console.log(puntosRestantes);
        var recompensa = x.data.getSelected().value; //console.log(recompensa);
        var recompensaPuntos = x.data.getSelected().class; console.log(recompensaPuntos);

        if (Number(puntosRestantes) >= Number(recompensaPuntos)) {
            $.get(GETRECOMPENSA, { id_recompensa: recompensa })
                 .done(function (data) {
                     $('#btnAgregarRecompensa').prop("disabled", false);
                     //console.log(data.data[0]);
                     agregarFila(data.data[0].ID_RECOMPENSA, data.data[0].NOMBRE, data.data[0].VALOR_PUNTOS);
                     totalPuntosCanjeados = totalPuntosCanjeados + data.data[0].VALOR_PUNTOS;
                     //console.log(totalPuntosCanjeados);
                     $('#totalPuntos').text(totalPuntosCanjeados); console.log($('#totalPuntos').text(totalPuntosCanjeados));
                     let puntos_disponibles = $('#txtPuntosAcumulados').val();
                     $('#puntos_restantes').text(" " + puntos_disponibles - totalPuntosCanjeados);
                 })
                 .fail(function (jqXHR, textStatus, errorThrown) {
                     $('#btnAgregarRecompensa').prop("disabled", true);
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
        } else {
            $('#btnAgregarRecompensa').prop("disabled", true);
            $.alert({
                title: '<i class="fa fa-info-circle text-warning"></i> Información!',
                content: "Faltan puntos para el valor de la recompensa",
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

    $('#tableRecompensasCanjeadas').on('click', '.btn-eliminarRecom', function (event) {
        event.preventDefault();
        $(this).closest('tr').remove();
        let vpoint = $(this).attr('data-punto'); console.log(vpoint);
        totalPuntosCanjeados = totalPuntosCanjeados - vpoint;
        console.log(totalPuntosCanjeados);
        $('#totalPuntos').text(totalPuntosCanjeados);
        let puntos_disponibles = $('#txtPuntosAcumulados').val();
        $('#puntos_restantes').text(" " + puntos_disponibles - totalPuntosCanjeados);
    });

    //$('#txtRNTT').keyup(function () {
    //    if (this.value.length > 0) {
    //        $('#btnAgregarRecompensa').prop("disabled", false);
    //    } else {
    //        $('#btnAgregarRecompensa').prop("disabled", true);
    //    }

    //});


    $("#btnSearchRnttRecomp").click(function () {
        let rntt = $("#txtRNTT").val(); console.log(rntt);
        if (rntt != "") {
            $.get(GETCHOFERES, { parametro: rntt })
                    .done(function (data) {
                        console.log(data);
                        console.log(data.data[0]);
                        if (data.data[0] != undefined) {
                            $('#btnAgregarRecompensa').prop("disabled", false);
                            $("#txtNameFull").val(data.data[0].CHOFER);
                            $("#txtCedula").val(data.data[0].CEDULA);
                            $("#txtEmpresa").val(data.data[0].EMPRESA);
                            $("#txtPuntosAcumulados").val(data.data[0].PUNTOS_BALANCE_LY); 
                            $('#puntos_restantes').text(data.data[0].PUNTOS_BALANCE_LY);
                            if (data.data[0].ESTATUS_AFILACION == "ACTIVO") { $("#statusC").text(data.data[0].ESTATUS_AFILACION).addClass("text-primary"); }
                            else { $("#statusC").text(data.data[0].ESTATUS_AFILACION).addClass("text-warning"); }
                        } else {
                            $('#btnAgregarRecompensa').prop("disabled", true);
                            $("#txtNameFull").val("");
                            $("#txtCedula").val("");
                            $("#txtEmpresa").val("");
                            $("#txtPuntosAcumulados").val("");
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
                        $('#btnAgregarRecompensa').prop("disabled", true);
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
        } else {
            $("#txtNameFull").val(""); $("#txtCedula").val(""); $("#txtEmpresa").val(""); $("#txtPuntosAcumulados").val(""); $("#statusC").text("");
            $('#btnAgregarRecompensa').prop("disabled", true);
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


    //console.log(GETLISTADOENTREGARECOMPENSAS);
   

     //var tableRegistrosIncidentes = $('#tableRegistroIncidencias').DataTable({
     //           "destroy": true,
     //           "ajax": {
     //               "url": GETREGISTROS,
     //               "type": "GET",
     //               "data": parametros,
     //               "datatype": "json"
     //           },
     //           "columns": [
     //                { "data": "RNTT" },
     //                { "data": "ROTULO" },
     //                { "data": "FULL_NAME" },
     //                { "data": "CEDULA" },
     //                { "data": "EMPRESA" },
     //                { "data": "REGLAS_LIST" },
     //                {
     //                    "data": "REGLAS", render: function (data, type, row) {
     //                        console.log(data);
     //                        if (data != null) {
     //                            return '<i class="fa fa-check" aria-hidden="true"></i>';
     //                        } else {
     //                            return '<i class="fa fa-minus" aria-hidden="true"></i>';
     //                        }
     //                    }
     //                },
     //                {
     //                    "data": "DANOS", render: function (data, type, row) {
     //                        console.log(data);
     //                        if (data != null) {
     //                            return '<i class="fa fa-check" aria-hidden="true"></i>';
     //                        } else {
     //                            return '<i class="fa fa-minus" aria-hidden="true"></i>';
     //                        }
     //                    }
     //                },
     //                {
     //                    "data": "LESIONES", render: function (data, type, row) {
     //                        console.log(data);
     //                        if (data != null) {
     //                            return '<i class="fa fa-check" aria-hidden="true"></i>';
     //                        } else {
     //                            return '<i class="fa fa-minus" aria-hidden="true"></i>';
     //                        }
     //                    }
     //                },
     //                {
     //                    "data": "SEGURIDAD_FISICA", render: function (data, type, row) {
     //                        console.log(data);
     //                        if (data != null) {
     //                            return '<i class="fa fa-check" aria-hidden="true"></i>';
     //                        } else {
     //                            return '<i class="fa fa-minus" aria-hidden="true"></i>';
     //                        }
     //                    }
     //                },
     //                { "data": "STR_FECHA" },
     //                { "data": "STR_HORA" },
     //                { "data": "DESCRIPCION" }
     //           ],
     //           //"scrollX": true
     //           //"rowCallback": function (row, data, index) {
     //           //    switch (data.ACTIVO) {
     //           //        case true: return $('td', row).addClass("activado"); break;
     //           //        case false: return $('td', row).addClass("desacticado"); break;
     //           //    }
     //           //}
     //       });

});