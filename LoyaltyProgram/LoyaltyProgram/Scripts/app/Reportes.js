var GETREGISTROS; var GETCAMPANIAS;
$(document).ready(function () {

    var Year = moment(new Date()).format("YYYY");
    var DateNow = moment(new Date());
    console.log(DateNow);
    $('#pickerDateDE').datetimepicker({
        //defaultDate: "01-01-" + Year,
        //format: 'DD-MM-YYYY'
        format: 'L'
    });

    //$(function () {
    $('#pickerDateHASTA').datetimepicker({
        //defaultDate: "31-12-" + Year,
        //format: 'DD-MM-YYYY'
        format: 'L'
    });
    //});

    $('#Check_AllChoferes').change(function () {
        if (this.checked) {
            $("#txtRNTTChoferFiltro").prop('disabled', true).val("Todos");
        } else {
            $("#txtRNTTChoferFiltro").prop('disabled', false).val("");
        }
    });

    //var change_Regla = function () {
    //    if ($("#Check_AllReglas").checked) {
    //        $('#CbTipoReglas').removeAttr('disabled');
    //    }
    //};

    //$(change_Regla);
    //$("#Check_AllReglas").change(change_Regla);
    //$('#Check_AllReglas').change(function () {
    //    $('#CbTipoReglas').removeAttr('disabled');
    //});

    $('#Check_AllRotulos').change(function () {
        if (this.checked) {
            $("#txtRotuloTruckFiltro").prop('disabled', true).val("Todos");
        } else {
            $("#txtRotuloTruckFiltro").prop('disabled', false).val("");
        }
    });

    var parametros =
        {
            DATE_START: $('#FechaIncidenciaDE').val(),
            DATE_END: $('#FechaIncidenciaHASTA').val(),
            RNTT: $("#txtRotulo").val(),
            ROTULO: $("#txtRNTT").val()
        };



    $('#RefrescarRegistro').click(function () {
        //tableRegistrosIncidentes.destroy();
        let date_start = $('#FechaIncidenciaDE').val(); console.log(date_start);
        let date_end = $('#FechaIncidenciaHASTA').val(); console.log(date_end);

        if (date_start != "" && date_end != "") {


            let rntt = $("#txtRNTTChoferFiltro").val(); console.log(rntt);
            let rotulo = $("#txtRotuloTruckFiltro").val(); console.log(rotulo);


            if (rntt == "Todos") { rntt = null; }
            if (rotulo == "Todos") { rotulo = null; }

            var parametros =
                    {
                        DATE_START: $('#FechaIncidenciaDE').val(),
                        DATE_END: $('#FechaIncidenciaHASTA').val(),
                        RNTT: rntt,
                        ROTULO: rotulo
                    };

            console.log(parametros);

            //$.get(GETREGISTROS, { parametros })
            //     .done(function (data) {
            //         console.log(data);
            //     })
            //     .fail(function (jqXHR, textStatus, errorThrown) {
            //         console.log(errorThrown)
            //     });
            //var dateStart = $('#FechaIncidenciaDE').val(); console.log(dateStart);
            //var dateEnd = $('#FechaIncidenciaHASTA').val(); console.log(dateEnd);
            //var rntt = $('#txtRNTTChoferFiltro').val(); console.log(rntt);
            //var rotulo = $('#txtRotuloTruckFiltro').val(); console.log(rotulo);

            var tableRegistrosIncidentes = $('#tableRegistroIncidencias').DataTable({
                "destroy": true,
                "ajax": {
                    "url": GETREGISTROS,
                    "type": "GET",
                    "data": parametros,
                    "datatype": "json"
                },
                "columns": [
                     { "data": "RNTT" },
                     { "data": "ROTULO" },
                     { "data": "FULL_NAME" },
                     { "data": "CEDULA" },
                     { "data": "EMPRESA" },
                     { "data": "REGLAS_LIST" },
                     {
                         "data": "REGLAS", render: function (data, type, row) {
                             console.log(data);
                             if (data != null) {
                                 return '<i class="fa fa-check" aria-hidden="true"></i>';
                             } else {
                                 return '<i class="fa fa-minus" aria-hidden="true"></i>';
                             }
                         }
                     },
                     {
                         "data": "DANOS", render: function (data, type, row) {
                             console.log(data);
                             if (data != null) {
                                 return '<i class="fa fa-check" aria-hidden="true"></i>';
                             } else {
                                 return '<i class="fa fa-minus" aria-hidden="true"></i>';
                             }
                         }
                     },
                     {
                         "data": "LESIONES", render: function (data, type, row) {
                             console.log(data);
                             if (data != null) {
                                 return '<i class="fa fa-check" aria-hidden="true"></i>';
                             } else {
                                 return '<i class="fa fa-minus" aria-hidden="true"></i>';
                             }
                         }
                     },
                     {
                         "data": "SEGURIDAD_FISICA", render: function (data, type, row) {
                             console.log(data);
                             if (data != null) {
                                 return '<i class="fa fa-check" aria-hidden="true"></i>';
                             } else {
                                 return '<i class="fa fa-minus" aria-hidden="true"></i>';
                             }
                         }
                     },
                     { "data": "STR_FECHA" },
                     { "data": "STR_HORA" },
                     { "data": "DESCRIPCION" }
                ],
                //"scrollX": true
                //"rowCallback": function (row, data, index) {
                //    switch (data.ACTIVO) {
                //        case true: return $('td', row).addClass("activado"); break;
                //        case false: return $('td', row).addClass("desacticado"); break;
                //    }
                //}
            });

            $("#entreRegistros").text("Registros de " + date_start + " hasta " + date_end);

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