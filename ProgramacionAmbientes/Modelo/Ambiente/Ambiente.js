ProgramacionApp.controller('AmbienteController',
    ['$scope', '$rootScope', '$location', 'AmbienteService', '$routeParams', '$sce',
        function ($scope, $rootScope, $location, AmbienteService, $routeParams, $sce) {

            var nav = $("#navbar").hasClass("gn-menu-wrapper gn-open-all");
            if (nav == true) {
                $(".Principal").css("width", "80%");

            } else {
                $(".Principal").css("width", "95%");
            }

            $scope.UploadFileWeb = function () {
                $("#fileUploadWeb").trigger('click');
            };

            $("#colegio").show();
            $("#atras").hide();

            if ($rootScope.globals.currentUser.tipousuario == 2) {
                $("#colegio").hide();
            }

            $("#fileUploadWeb").change(function () {
                dataweb = new FormData();

                var files = $("#fileUploadWeb").get(0).files;

                //
                var fileExtension = ['xlsx'];
                if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
                    bootbox.dialog({
                        title: "Importar Archivo",
                        message: "La extencion del archivo no es valida",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    $("#fileUploadWeb").replaceWith($("#fileUploadWeb").val('').clone(true));

                    //waitingDialog.hide();
                    return false;

                }


                // Add the uploaded image content to the form data collection
                if (files.length > 0) {

                    readURL(this, "logoweb");

                    dataweb.append("UploadedImage", files[0]);
                    if (dataweb != null) {
                        AmbienteService.SubirArchivo(dataweb, function (response) {
                            if (response.success) {

                                bootbox.dialog({
                                    title: "Importar Archivo",
                                    message: "La importación del archivo se realizó con éxito ",
                                    buttons: {
                                        success: {
                                            label: "Cerrar",
                                            className: "btn-primary",
                                        }
                                    }
                                });



                                $scope.path = response.path;

                                $("#fileUploadWeb").replaceWith($("#fileUploadWeb").val('').clone(true));

                                if ($rootScope.globals.currentUser.tipousuario == 2) {


                                    AmbienteService.AmbientesxArea($rootScope.globals.currentUser.idpersona, function (response) {

                                        if (response.success == true) {
                                            $scope.datalists = response.datos;
                                            $scope.ListaCompleta = response.datos;
                                            $scope.numberOfPages = function () {
                                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                                            };
                                            if ($rootScope.globals.currentUser.tipousuario == 2) {

                                                $("#boton").hide();
                                                $("#boton1").hide();
                                                $("#excel").hide();
                                                $("#eliminar").hide();
                                                $("#modificar").hide();
                                                $("#descargar").hide();

                                            }
                                        }
                                    });
                                } else {
                                    AmbienteService.ConsultarAmbiente(function (response) {
                                        if (response.success == true) {
                                            $scope.datalists = response.datos;
                                            $scope.ListaCompleta = response.datos;
                                            $scope.numberOfPages = function () {
                                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                                            };
                                        }
                                    });
                                }

                                $("#fileUploadWeb").replaceWith($("#fileUploadWeb").val('').clone(true));
                                //waitingDialog.hide();

                                return;
                            }

                        });
                    }

                }

            });

            function readURL(input, control) {
                if (input.files && input.files[0]) {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#' + control + '').attr('src', e.target.result);
                    }

                    reader.readAsDataURL(input.files[0]);
                }
            };


            $scope.Ambiente = {
                Numero: "",
                IdArea: "",
                Piso: "",
                Pantalla: "",
                NumeroEquipos: "",
                IdSede: "",
                NumeroSillas: "",
                NumeroMesas: ""
            };

            $scope.DiasProgramacion = {
                Lunes: false,
                Martes: false,
                Miercoles: false,
                Jueves: false,
                Viernes: false
            }

            $scope.AbrirModal = function () {
                AmbienteService.ConsultarSedes(function (response) {
                    if (response.success == true) {
                        $scope.Sede = response.datos;
                    }
                });
                $("#ModalAmbiente").modal("show");
                $scope.VaciarCampos();
            };

            $scope.AbrirModal1 = function () {

                AmbienteService.ConsultarSedesColegios(function (response) {
                    if (response.success == true) {
                        $scope.Sede = response.datos;
                        $("#ModalAmbiente").modal("show");
                    }
                });


                $scope.VaciarCampos();
            };


            $scope.Guardar = function () {
                $.each($scope.Area, function (index, value) {

                    if (value.IdArea == $scope.Area.IdArea) {

                        $scope.Ambiente.IdArea = value.IdArea
                    }
                });
                $.each($scope.Sede, function (index, value) {
                    if (value.IdSede == $scope.Sede.IdSede) {
                        $scope.Ambiente.IdSede = value.IdSede;
                    }
                });
                if ($scope.Ambiente.Piso == "" || $scope.Ambiente.Numero == "" || $scope.Ambiente.NumeroEquipos == "" || $scope.Ambiente.IdArea == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe diligenciar todos los campos",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                } else {
                    AmbienteService.GuardarAmbiente($scope.Ambiente, function (response) {
                        if (response.success == true) {

                            $scope.VaciarCampos();
                            $("#ModalAmbiente").modal("hide");
                            AmbienteService.ConsultarAmbiente(function (response) {

                                if (response.success == true) {
                                    $scope.datalists = response.datos;
                                    $scope.ListaCompleta = response.datos;
                                    $scope.numberOfPages = function () {
                                        return Math.ceil($scope.datalists.length / $scope.pageSize);
                                        bootbox.dialog({
                                            title: "Información",
                                            message: "El registro se realizó con éxito",
                                            buttons: {
                                                success: {
                                                    label: "Cerrar",
                                                    className: "btn-primary",
                                                }
                                            }
                                        });
                                    };

                                    if ($rootScope.globals.currentUser.tipousuario == 2) {

                                        $("#boton").hide();
                                        $("#boton1").hide();
                                        $("#excel").hide();
                                        $("#eliminar").hide();
                                        $("#modificar").hide();
                                        $("#descargar").hide();
                                    }
                                }
                            });
                        } else {
                            bootbox.dialog({
                                title: "Información",
                                message: "El ambiente ya se encuentra registrado",
                                buttons: {
                                    success: {
                                        label: "Cerrar",
                                        className: "btn-primary",
                                    }
                                }
                            });
                        }
                    });
                }
            };

            $scope.VaciarCampos = function () {

                $scope.Ambiente.Numero = "";
                $scope.Ambiente.Piso = "";
                $scope.Ambiente.Pantalla = "";
                $scope.Ambiente.NumeroEquipos = "";
                $scope.Ambiente.NumeroMesas = "";
                $scope.Ambiente.NumeroSillas = "";
                $scope.Area.Nombre = "";
                $scope.Area.IdArea = "";


            }
            $scope.curPage = 0;
            $scope.pageSize = 5;


            $scope.atras = function () {

                AmbienteService.ConsultarAmbienteSinColegios(function (response) {
                    if (response.success == true) {
                        $scope.datalists = response.datos;
                        $scope.ListaCompleta = response.datos;
                        $scope.numberOfPages = function () {
                            return Math.ceil($scope.datalists.length / $scope.pageSize);
                        };
                        $("#atras").hide();
                    }

                });
            }

            $scope.ConsultarColegios = function () {

                AmbienteService.ConsultarAmbientesColegios(function (response) {
                    if (response.success == true) {
                        $scope.datalists = response.datos;
                        $scope.ListaCompleta = response.datos;
                        $scope.numberOfPages = function () {
                            return Math.ceil($scope.datalists.length / $scope.pageSize);
                        };
                        $("#atras").show();
                    }
                });

            }

            if ($rootScope.globals.currentUser.tipousuario == 2) {

                AmbienteService.AmbientesxArea($rootScope.globals.currentUser.idpersona, function (response) {

                    if (response.success == true) {
                        $scope.datalists = response.datos;
                        $scope.ListaCompleta = response.datos;
                        $scope.numberOfPages = function () {
                            return Math.ceil($scope.datalists.length / $scope.pageSize);
                        };
                        if ($rootScope.globals.currentUser.tipousuario == 2) {

                            $("#boton").hide();
                            $("#boton1").hide();
                            $("#excel").hide();
                            $("#eliminar").hide();
                            $("#modificar").hide();
                            $("#descargar").hide();
                        }
                    }
                });
            } else {
                AmbienteService.ConsultarAmbienteSinColegios(function (response) {
                    if (response.success == true) {
                        $scope.datalists = response.datos;
                        $scope.ListaCompleta = response.datos;
                        $scope.numberOfPages = function () {
                            return Math.ceil($scope.datalists.length / $scope.pageSize);
                        };
                    }
                });
            };


            $scope.OrganizarPisoAsc = function () {
                AmbienteService.OrganizarPisoAsc(function (response) {
                    if (response.success == true) {
                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.Datos = $scope.datalists;
                            $scope.numberOfPages = function () {
                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                            };
                        }
                    }
                });
            };

            $scope.OrganizarPisoDesc = function () {
                AmbienteService.OrganizarPisoDesc(function (response) {
                    if (response.success == true) {
                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.Datos = $scope.datalists;
                            $scope.numberOfPages = function () {
                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                            };
                        }
                    }
                });
            };

            $scope.OrganizarNumeroAsc = function () {
                AmbienteService.OrganizarNumeroAsc(function (response) {
                    if (response.success == true) {
                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.Datos = $scope.datalists;
                            $scope.numberOfPages = function () {
                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                            };
                        }
                    }
                });
            };

            $scope.OrganizarNumeroDesc = function () {
                AmbienteService.OrganizarNumeroDesc(function (response) {
                    if (response.success == true) {
                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.Datos = $scope.datalists;
                            $scope.numberOfPages = function () {
                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                            };
                        }
                    }
                });
            };

            $scope.SeleccionarTodos = function () {
                $.each($scope.Datos, function (index, value) {
                    value.Seleccionado = $scope.SeleccionTodos;
                });
            };

            $scope.CambiarEstadoSeleccionados = function () {
                var UsariosBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });

                if (UsariosBorrar.length == 0) {

                    bootbox.dialog({
                        title: "Eliminar",
                        message: "Seleccione por lo menos un ambiente",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                } else {

                    $("#modalInhabilitar").modal("show");
                }
            };


            $scope.inhabilitar = function () {

                var AmbientesBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });
                AmbienteService.BorrarAmbientes(AmbientesBorrar, function (response) {

                    if (response.success == true) {
                        AmbienteService.ConsultarAmbienteSinColegios(function (response) {

                            if (response.success == true) {
                                $scope.datalists = response.datos;
                                $scope.ListaCompleta = response.datos;
                                $scope.numberOfPages = function () {
                                    return Math.ceil($scope.datalists.length / $scope.pageSize);
                                };
                                bootbox.dialog({
                                    title: "Información",
                                    message: "La eliminación se realizó con éxito",
                                    buttons: {
                                        success: {
                                            label: "Cerrar",
                                            className: "btn-primary",
                                        }
                                    }
                                });
                                if ($rootScope.globals.currentUser.tipousuario == 2) {

                                    $("#boton").hide();
                                    $("#boton1").hide();
                                    $("#excel").hide();
                                    $("#eliminar").hide();
                                    $("#modificar").hide();
                                    $("#descargar").hide();
                                }
                            }
                        });
                    }

                });

            };

            //Función 1 para editar un registro de la tabla
            $scope.Modificar = function () {

                var AmbienteModificar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;

                });


                if (AmbienteModificar.length == 1) {

                    AmbienteService.ModificarAmbiente(AmbienteModificar, function (response) {

                        if (response.success == true) {
                            $scope.Ambiente.IdAmbiente = response.Ambiente.IdAmbiente;
                            $scope.Ambiente.Numero = parseInt(response.Ambiente.Numero);
                            $scope.Ambiente.Piso = response.Ambiente.Piso;
                            $scope.Ambiente.Pantalla = response.Ambiente.Pantalla;
                            $scope.Ambiente.NumeroEquipos = response.Ambiente.NumeroEquipos;
                            $scope.Ambiente.NumeroMesas = response.Ambiente.NumeroMesas;
                            $scope.Ambiente.NumeroSillas = response.Ambiente.NumeroSillas;
                            $("#SedesLista > option[value='" + response.Ambiente.IdSede + "']").attr('selected', 'selected');
                            $("#AreaLista > option[value='" + response.Ambiente.IdArea + "']").attr('selected', 'selected');
                            $scope.Ambiente.IdSede = response.Ambiente.IdSede;
                            $scope.Ambiente.IdArea = response.Ambiente.IdArea;
                            $("#ModalEditar").modal("show");

                        }
                    });
                } else {

                    bootbox.dialog({
                        title: "Editar",
                        message: "Seleccione un ambiente",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                }
            }



            AmbienteService.ConsultarSedes(function (response) {
                if (response.success == true) {
                    $scope.Sede = response.datos;
                }
            });

            $scope.GuardarEdicionAmbiente = function () {
                $.each($scope.Area, function (index, value) {

                    if (value.IdArea == $scope.Area.IdArea) {

                        $scope.Ambiente.IdArea = value.IdArea
                    }
                });
                $.each($scope.Sede, function (index, value) {
                    if (value.IdSede == $scope.Sede.IdSede) {
                        $scope.Ambiente.IdSede = value.IdSede;
                    }
                });
                if ($scope.Ambiente.Piso == "" || $scope.Ambiente.Numero == "" || $scope.Ambiente.NumeroEquipos == "" || $scope.Ambiente.IdArea == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe diligenciar todos los campos",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                } else {
                    AmbienteService.GuardarModificacionAmbiente($scope.Ambiente, function (response) {
                        if (response.success == true) {
                            $("#ModalEditar").modal("hide");
                            //$scope.VaciarCampos();
                            AmbienteService.ConsultarAmbienteSinColegios(function (response) {
                                if (response.success == true) {
                                    $scope.datalists = response.datos;
                                    $scope.ListaCompleta = response.datos;
                                    $scope.numberOfPages = function () {
                                        return Math.ceil($scope.datalists.length / $scope.pageSize);
                                    };

                                    if ($rootScope.globals.currentUser.tipousuario == 2) {

                                        $("#boton").hide();
                                        $("#boton1").hide();
                                        $("#excel").hide();
                                        $("#eliminar").hide();
                                        $("#modificar").hide();
                                        $("#descargar").hide();
                                    }
                                    bootbox.dialog({
                                        title: "Información",
                                        message: "La modificación se realizó con éxito",
                                        buttons: {
                                            success: {
                                                label: "Cerrar",
                                                className: "btn-primary",
                                            }
                                        }
                                    });
                                }
                            });
                        }
                    });
                }
            };




            $scope.Filter = function (e) {

                //var rex = new RegExp($(e.target).val(), 'i');
                //$('.searchable tr').hide();
                //$('.searchable tr').filter(function () {
                //    return rex.test($(this).text());
                //}).show();

                var Busqueda = $("#Buscar").val();
                var exp = new RegExp(Busqueda);
                if (Busqueda == "") {
                    if ($rootScope.globals.currentUser.tipousuario == 2) {


                        AmbienteService.AmbientesxArea($rootScope.globals.currentUser.idpersona, function (response) {
                            if (response.success == true) {
                                $scope.datalists = response.datos;
                                $scope.ListaCompleta = response.datos;
                                $scope.numberOfPages = function () {
                                    return Math.ceil($scope.datalists.length / $scope.pageSize);
                                };

                                if ($rootScope.globals.currentUser.tipousuario == 2) {

                                    $("#boton").hide();
                                    $("#boton1").hide();
                                    $("#excel").hide();
                                    $("#eliminar").hide();
                                    $("#modificar").hide();
                                    $("#descargar").hide();
                                }
                            }
                        });
                    } else {
                        AmbienteService.ConsultarAmbienteSinColegios(function (response) {
                            if (response.success == true) {
                                $scope.datalists = response.datos;
                                $scope.ListaCompleta = response.datos;
                                $scope.numberOfPages = function () {
                                    return Math.ceil($scope.datalists.length / $scope.pageSize);
                                };
                            }
                        });
                    }
                }
                var Ambiente = [];
                $scope.datalists = $scope.ListaCompleta;
                Ambiente = $scope.datalists.filter(function (item) {

                    if (exp.test(item.Parametro8.toLowerCase()) || exp.test(item.Parametro8.toUpperCase())) {

                        return item;
                    }

                    else if (exp.test(item.Parametro2.toLowerCase()) || exp.test(item.Parametro2.toUpperCase())) {
                        return item;
                    }

                    else if (exp.test(item.Parametro7.toLowerCase()) || exp.test(item.Parametro7.toUpperCase())) {
                        return item;
                    }

                    else if (exp.test(item.Parametro3.toLowerCase()) || exp.test(item.Parametro3.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.Parametro4.toLowerCase()) || exp.test(item.Parametro4.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.Parametro9.toLowerCase()) || exp.test(item.Parametro9.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.Parametro10.toLowerCase()) || exp.test(item.Parametro10.toUpperCase())) {
                        return item;
                    }

                });
                $scope.datalists = Ambiente;
                //Variable para setear la paginación 
                $scope.curPage = 0;

            };

            $scope.ClearFilter = function () {
                $('.searchable tr').show();
            };


            //------------------------------------------------------------- MODAL REPORTE----------------------------------------------------------------------
            //  
            //Función para consultar las areas 
            AmbienteService.ConsultarAreas(function (response) {

                if (response.success == true) {
                    $scope.Area = response.datos;
                }
            });

            $scope.DescargarReporte = function () {

                $scope.sedeExport = [];

                $scope.AmbientesExport = [];

                $scope.Plantilla = [{
                    Codigo_Sede: "", Codigo_Area: "", Piso: "", Número_Ambiente: "", Cantidad_Equipos: "", Cantidad_Mesas: "", Cantidad_Sillas: "", Pantalla: ""
                }];

                $.each($scope.Area, function (index, value) {

                    $scope.AmbientesExport.push({
                        Codigo_Area: value.Codigo, Nombre: value.Nombre
                    });

                });

                $.each($scope.Sede, function (index, value1) {

                    $scope.sedeExport.push({
                        Codigo_Sede: value1.Codigo, Nombre_Sede: value1.Nombre_Sede
                    });

                });

                alasql('SELECT * INTO XLSX("Códigos Area.xlsx",{headers:true}) FROM ?', [$scope.AmbientesExport]);
                alasql('SELECT * INTO XLSX("Códigos Sede.xlsx",{headers:true}) FROM ?', [$scope.sedeExport]);
                alasql('SELECT * INTO XLSX("Plantilla Ambientes.xlsx",{headers:true}) FROM ?', [$scope.Plantilla]);
            };

            $scope.Fechas = {

                FechaInicio: "",
                FechaFin: "",
                HoraInicio: "",
                HoraFin: "",
                opc: ""
            }


            $('#FechaInicio').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#FechaFin').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#HoraInicio').datetimepicker({
                format: 'HH:mm',
            });

            $('#HoraFin').datetimepicker({
                format: 'HH:mm'
            });

            $scope.RangoHoras = [
             { Jornada: 1, Nombre: "06 am - 12 pm" },
             { Jornada: 2, Nombre: "12 pm - 06 pm" },
             { Jornada: 3, Nombre: "06 pm - 10 pm" }
            ];

            $scope.reporteDisoponibilidad = function () {

                $("#ModalReporte").modal("show");

            };


            $('#abrir').change(function () {
                if ($(this).is(":checked")) {
                    $("#horas").css("display", "block");
                    $("#jornada").css("display", "none");
                    $('#jornada1').prop("checked", false)
                } else {
                    $("#horas").css("display", "none");
                }
            });

            $('#jornada1').change(function () {
                if ($(this).is(":checked")) {
                    $("#jornada").css("display", "block");
                    $("#horas").css("display", "none");
                    $('#abrir').prop("checked", false)
                } else {
                    $("#jornada").css("display", "none");
                }
            });


            $scope.Jornada = {
                jornada: ""
            };

            $scope.Rango = {
                rango: ""
            };

            $scope.SeleccionarTodosDias = function () {
                if ($scope.TodosDias == true) {
                    $scope.DiasProgramacion.Lunes = true;
                    $scope.DiasProgramacion.Martes = true;
                    $scope.DiasProgramacion.Miercoles = true;
                    $scope.DiasProgramacion.Jueves = true;
                    $scope.DiasProgramacion.Viernes = true;
                } else {
                    $scope.DiasProgramacion.Lunes = false;
                    $scope.DiasProgramacion.Martes = false;
                    $scope.DiasProgramacion.Miercoles = false;
                    $scope.DiasProgramacion.Jueves = false;
                    $scope.DiasProgramacion.Viernes = false;
                }
            };


            $scope.descargarReporteAmbientes = function () {

                var x = $("#FechaInicio").val().split('/');
                var y = $("#FechaFin").val().split('/');

                var StartDate = x[1] + "-" + x[2] + "-" + x[0];
                var EndDate = y[1] + "-" + y[2] + "-" + y[0];
                if (Date.parse(StartDate) > Date.parse(EndDate)) {
                    bootbox.dialog({
                        title: "Información",
                        message: "La fecha final debe ser mayor a la fecha inicial",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }



                if ($scope.Jornada.jornada == false && $scope.Rango.rango == false) {


                    $scope.Fechas.HoraInicio = "00:00";
                    $scope.Fechas.HoraFin = "01:00";
                    $scope.Fechas.opc = 1;
                    if ($rootScope.globals.currentUser.tipousuario == 2) {
                        $scope.Fechas.opc = 2;
                    }
                  
                    if ($scope.DiasProgramacion.Lunes == false && $scope.DiasProgramacion.Martes == false && $scope.DiasProgramacion.Miercoles == false
                        && $scope.DiasProgramacion.Jueves == false && $scope.DiasProgramacion.Viernes == false) {
                        bootbox.dialog({
                            title: "Información",
                            message: "Debe seleccionar por lo menos un día",
                            buttons: {
                                success: {
                                    label: "Cerrar",
                                    className: "btn-primary",
                                }
                            }
                        });
                        return;
                    }

                    AmbienteService.ambientesDisponibles($scope.Fechas, $rootScope.globals.currentUser.idpersona, $scope.DiasProgramacion, function (response) {

                        if (response.success == true) {

                            $scope.reporte = response.datos;
                            $scope.AmbientesDisponoblesExport = [];

                            $.each($scope.reporte, function (index, value) {
                                $.each($scope.Sede, function (index1, value1) {
                                    if (value.IdSede == value1.IdSede) {
                                        $scope.reporte[index].IdSede = value1.Nombre_Sede;
                                    }
                                });
                            });

                            $.each($scope.reporte, function (index, value) {
                                if (value.Pantalla == 1) {
                                    value.Pantalla = "Si";
                                } else {
                                    value.Pantalla = "No";
                                }
                            });

                            $.each($scope.reporte, function (index, value) {

                                $scope.AmbientesDisponoblesExport.push({
                                    Nombre_Sede: value.IdSede, Piso: value.Piso, Ambiente: value.Numero, Pantalla: value.Pantalla, Numero_Equipos: value.NumeroEquipos, NumeroMesas: value.NumeroMesas, NumeroSillas: value.NumeroSillas
                                });

                            });

                            alasql('SELECT * INTO XLSX("Ambientes Disponibles.xlsx",{headers:true}) FROM ?', [$scope.AmbientesDisponoblesExport]);
                        }
                    });
                }

                if ($scope.Jornada.jornada == true) {


                    if ($scope.RangoHoras.Jornada == 1) {
                        $scope.Fechas.HoraInicio = "06:00";
                        $scope.Fechas.HoraFin = "12:00";

                    }

                    if ($scope.RangoHoras.Jornada == 2) {
                        $scope.Fechas.HoraInicio = "12:00";
                        $scope.Fechas.HoraFin = "18:00";

                    }

                    if ($scope.RangoHoras.Jornada == 3) {
                        $scope.Fechas.HoraInicio = "18:00";
                        $scope.Fechas.HoraFin = "22:00";

                    }

                    $scope.Fechas.opc = 3;
                    if ($rootScope.globals.currentUser.tipousuario == 2) {
                        $scope.Fechas.opc = 4;
                    }
                    AmbienteService.ambientesDisponibles($scope.Fechas, $rootScope.globals.currentUser.idpersona, $scope.DiasProgramacion, function (response) {

                        if (response.success == true) {

                            $scope.reporte = response.datos;
                            $scope.AmbientesDisponoblesExport = [];

                            $.each($scope.reporte, function (index, value) {
                                $.each($scope.Sede, function (index1, value1) {
                                    if (value.IdSede == value1.IdSede) {
                                        $scope.reporte[index].IdSede = value1.Nombre_Sede;
                                    }
                                });
                            });

                            $.each($scope.reporte, function (index, value) {
                                if (value.Pantalla == 1) {
                                    value.Pantalla = "Si";
                                } else {
                                    value.Pantalla = "No";
                                }
                            });

                            $.each($scope.reporte, function (index, value) {

                                $scope.AmbientesDisponoblesExport.push({
                                    Nombre_Sede: value.IdSede, Piso: value.Piso, Ambiente: value.Numero, Pantalla: value.Pantalla, Numero_Equipos: value.NumeroEquipos, NumeroMesas: value.NumeroMesas, NumeroSillas: value.NumeroSillas
                                });

                            });

                            alasql('SELECT * INTO XLSX("Ambientes Disponibles.xlsx",{headers:true}) FROM ?', [$scope.AmbientesDisponoblesExport]);
                        }
                    });

                }


                if ($scope.Rango.rango == true) {
                    $scope.Fechas.HoraInicio = $("#HoraInicio").val()
                    $scope.Fechas.HoraFin = $("#HoraFin").val();

                    if ($scope.Fechas.HoraInicio >= $scope.Fechas.HoraFin) {
                        bootbox.dialog({
                            title: "Información",
                            message: "La hora final debe ser mayor a la hora inicial",
                            buttons: {
                                success: {
                                    label: "Cerrar",
                                    className: "btn-primary",
                                }
                            }
                        });
                        return;
                    }
                    $scope.Fechas.opc = 3;
                    if ($rootScope.globals.currentUser.tipousuario == 2) {
                        $scope.Fechas.opc = 4;
                    }


                    AmbienteService.ambientesDisponibles($scope.Fechas, $rootScope.globals.currentUser.idpersona, $scope.DiasProgramacion, function (response) {

                        if (response.success == true) {

                            $scope.reporte = response.datos;
                            $scope.AmbientesDisponoblesExport = [];


                            $.each($scope.reporte, function (index, value) {
                                $.each($scope.Sede, function (index1, value1) {
                                    if (value.IdSede == value1.IdSede) {
                                        $scope.reporte[index].IdSede = value1.Nombre_Sede;
                                    }
                                });
                            });

                            $.each($scope.reporte, function (index, value) {
                                if (value.Pantalla == 1) {
                                    value.Pantalla = "Si";
                                } else {
                                    value.Pantalla = "No";
                                }
                            });

                            $.each($scope.reporte, function (index, value) {

                                $scope.AmbientesDisponoblesExport.push({
                                    Nombre_Sede: value.IdSede, Piso: value.Piso, Ambiente: value.Numero, Pantalla: value.Pantalla, Numero_Equipos: value.NumeroEquipos, NumeroMesas: value.NumeroMesas, NumeroSillas: value.NumeroSillas
                                });

                            });

                            alasql('SELECT * INTO XLSX("Ambientes Disponibles.xlsx",{headers:true}) FROM ?', [$scope.AmbientesDisponoblesExport]);
                        }
                    });

                }
            }

        }]);