var GETREGLAS; var GETRECOMPENSAS; var x; var REGISTRO_INCIDENCIAS; var GETCHOFERES; var GET_REGISTRO_FOR_ID; var GET_UPDATE_REGISTRO;
var id_registro;
$(document).ready(function () {


    DateNow = moment(new Date());
    $('#pickerIncidenciaDate').datetimepicker({
        defaultDate: DateNow,
        format: 'L'
    });

    $(function () {
        $('#pickerIncidenciaHours').datetimepicker({
            defaultDate: DateNow,
            format: 'LT'
        });
    });

    $('#pickerIncidenciaDateUpdate').datetimepicker({
        format: 'L'
    });

    $(function() {
        $('#pickerIncidenciaHoursUpdate').datetimepicker({
            format: 'LT'
        });
    });


    var infracciones;
    $('[name="checks[]"]').click(function () {
        infracciones = $('[name="checks[]"]:checked').map(function () {
            return this.value;
        }).get();
    });


    $('#tableRegistrarI').on('click', '.btn-updateRegistro', function() {

       


        id_registro = $(this).attr("id"); console.log(id_registro);
        let rotulo = $(this).attr("data-rotulo"); console.log(rotulo);
        let rntt = $(this).attr("data-rntt"); console.log(rntt);
        let fecha = $(this).attr("data-fecha"); console.log(fecha);
        let hora = $(this).attr("data-hora"); console.log(hora);
        let chofer = $(this).attr("data-chofer"); console.log(chofer);
        let cedula = $(this).attr("data-cedula"); console.log(cedula);
        let Reglasinfringidas = $(this).attr("data-infracciones"); console.log(Reglasinfringidas);
        let descripcion = $(this).attr("data-descripcion"); console.log(descripcion);
       

        let reglas = $(this).attr("data-reglasi"); console.log(reglas);
        let dano = $(this).attr("data-dano"); console.log(dano);
        let lesion = $(this).attr("data-lesion"); console.log(lesion);
        let seguridadFisica = $(this).attr("data-seguridadFisica"); console.log(seguridadFisica);

        $('#listReglasInfringidas').empty();
        $('#listInfracciones').empty();

        $('#txtRNTTupdate').val(rntt);
        $('#txtRotuloupdate').val(rotulo);
        $('#FechaIncidenciaUpdate').val(fecha);//fecha
        $('#txtNameFullupdate').val(chofer);
        $('#txtCedulaupdate').val(cedula);
        $('#HoraIncidenciaUpdate').val(hora);//reglas
        $('#commentupdate').val(descripcion);

        let ReglasSTR = Reglasinfringidas.split(",");
        var rpt = "";
        for (var i = 0; i < ReglasSTR.length; i++) {
            rpt = rpt + ReglasSTR[i] + "</br>";
        }
        $('#listReglasInfringidas').append(rpt); console.log(ReglasSTR);
        $('#listInfracciones').append(`${reglas} </br>${dano} </br>${lesion} </br>${seguridadFisica}`);

        $('#RegistroIModalCenterEditar').modal('show');

    });



    $("#btnUpdateRegistro").click(function() {

        let rotulo = $('#txtRotuloupdate').val();
        let fecha = $('#FechaIncidenciaUpdate').val() +" " + $('#HoraIncidenciaUpdate').val();
        let descrip = $('#commentupdate').val();

        var cambios =
        {
            ID_REGISTRO : id_registro,
            ROTULO :  rotulo,
            FECHA_INCIDENTE : fecha,
            DESCRIPCION: descrip
        }

        console.log(cambios);

        $.post(GET_UPDATE_REGISTRO, cambios)
            .done(function(data) {
                $.alert({
                    title: '<i class="fa fa-info-circle text-primaty"></i> Información!',
                    content: "Registro Actualizado",
                    animation: 'none',
                    type: 'blue',
                    buttons: {
                        ok: {
                            text: "OK",
                            btnClass: "btn-blue",
                            action: function() {
                                //$(this).css(csstxtClean);
                                location.reload();
                            }
                        }
                    }
                });
                console.log(data);
                $('#listReglasInfringidas').empty();
                $('#listInfracciones').empty();

                $('#txtRNTTupdate').val("");
                $('#txtRotuloupdate').val("");
                $('#FechaIncidenciaUpdate').val("");//fecha
                $('#txtNameFullupdate').val("");
                $('#txtCedulaupdate').val("");
                $('#HoraIncidenciaUpdate').val("");//reglas
                $('#commentupdate').val("");
                $('#RegistroIModalCenterEditar').modal('hide');
            })
            .fail(function(jqXHR, textStatus, errorThrown) {
                alert("Error: " + errorThrown);
            });

    }); // testing ahora

    $('#RegistroIModalCenter').on('hidden.bs.modal', function() {
        location.reload();
    });

    $("#btnRegistrarIncidente").click(function () {
        var reglas = [];

        for (var i = 0; i < x.data.getSelected().length; i++) {

            reglas[i] = x.data.getSelected()[i].value;
        }

        let descrip = $("#comment").val();
        let rotulo = $("#txtRotulo").val();
        let rntt = $("#txtRNTT").val();

        infracciones = $('[name="checks[]"]:checked').map(function () {
            return this.value;
        }).get();

        var parametros =
        {
            rotulo: rotulo,
            rntt: rntt,
            full_name: $("#txtNameFull").val(),
            cedula: $("#txtCedula").val(),
            empresa: $("#txtEmpresa").val(),
            fecha: $("#FechaIncidencia").val() + " " + $("#HoraIncidencia").val(),
            reglas: reglas,
            infracciones: infracciones,
            descripcion: descrip
        };
        console.log(parametros);

        if (descrip != "" && rntt != "") {
            $.post(REGISTRO_INCIDENCIAS, parametros)
                    .done(function (data) {
                        
                        $('#txtRNTT').val(""); $('#FechaIncidencia').val(DateNow.format("L")); $('#HoraIncidencia').val(DateNow.format("LT")); $('#txtNameFull').val(""); $('#txtCedula').val(""); $('#txtRotulo').val("");
                        $('#txtEmpresa').val(""); $('#comment').val(""); $("input[type=checkbox]").prop('checked', false); x.set([]);
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
                                        //$(this).css(csstxtClean);
                                        //location.reload();
                                    }
                                }
                            }
                        });
                    })
                    .fail(function (jqXHR, textStatus, errorThrown) {
                        $.alert({
                            title: '<i class="fa fa-info-circle text-warning"></i> Información!',
                            content: "Error" + errorThrown,
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
                    });
        }
        else {
            $.alert({
                title: '<i class="fa fa-info-circle text-warning"></i> Información!',
                content: "Hay campos vacíos!",
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

    $("#btnSearchRntt").click(function () {
        let rntt = $("#txtRNTT").val(); console.log(rntt);
        if (rntt != "") {
            $.get(GETCHOFERES, { parametro: rntt, })
                    .done(function (data) {

                        console.log(data.data[0]);
                        if (data.data[0] != undefined) {
                            $("#txtNameFull").val(data.data[0].CHOFER);
                            $("#txtCedula").val(data.data[0].CEDULA);
                            $("#txtEmpresa").val(data.data[0].EMPRESA);
                        } else {
                            $("#txtNameFull").val("");
                            $("#txtCedula").val("");
                            $("#txtEmpresa").val("");
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
            $("#txtNameFull").val(""); $("#txtCedula").val(""); $("#txtEmpresa").val("");
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

    $(".ss-list").click(function () {
        console.log(x.data.getSelected());
    });





});
