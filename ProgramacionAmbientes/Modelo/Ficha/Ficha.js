ProgramacionApp.controller('FichaController',
    ['$scope', '$rootScope', '$location', 'FichaService', '$routeParams', '$sce',
        function ($scope, $rootScope, $location, FichaService, $routeParams, $sce) {

            var nav = $("#navbar").hasClass("gn-menu-wrapper gn-open-all");
            if (nav == true) {
                $(".Principal").css("width", "80%");

            } else {
                $(".Principal").css("width", "95%");
            }

            $scope.UploadFileWeb = function () {
                $("#fileUploadWeb").trigger('click');
            };

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
                        FichaService.SubirArchivo(dataweb, function (response) {
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
                                FichaService.ConsultarFichas(function (response) {
                                    if (response.success == true) {
                                        $scope.datalists = response.datos;
                                        $scope.ListaCompleta = response.datos;
                                    }
                                });

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



            $scope.AbrirModal = function () {
                $("#ModalFicha").modal("show");
                $scope.VaciarCampos();
            };

            $("#atras").hide();

            $scope.Ficha = {
                IdFicha: "",
                IdPrograma: "",
                Ficha1: "",
                NumAprendices: "",
                Estado: "",
                TipoFormacion: "",
                FechaInicio: "",
                FechaFin: "",
                Jornada: ""
            };

            $scope.Programa = {
                IdPrograma: "",
                CodigoPrograma: "",
                Nivel: "",
                LineaTecnologica: "",
                Red_Tecnologica: "",
                Perfil_Instructor: "",
                Version_Programa: "",
                IdArea: "",
                NombrePrograma: ""
            };

            $('#FechaInicio').datepicker({
                language: 'es',
                autoclose: true,
            });

            $('#FechaFin').datepicker({
                language: 'es',
                autoclose: true
            });

            $('#FechaInicio1').datepicker({
                language: 'es',
                autoclose: true,
            });

            $('#FechaFin1').datepicker({
                language: 'es',
                autoclose: true
            });

            $scope.VaciarCampos = function () {
                $scope.Ficha.IdArea = "";
                $scope.Ficha.Ficha1 = "";
                $scope.Ficha.NumAprendices = "";
            };

            $scope.curPage = 0;
            $scope.pageSize = 5;




            $scope.SeleccionarNivelAcademico = function (programa) {

                $.each($scope.Programa, function (index, value) {

                    if (value.IdPrograma == programa) {

                        $("#Nivel > option[value='" + value.Nivel.toUpperCase() + "']").attr('selected', 'selected');
                        $scope.Ficha.TipoFormacion = value.Nivel.toUpperCase();
                    }
                });
            }


            $scope.Guardar = function () {

                $.each($scope.Programa, function (index, value) {

                    if (value.IdPrograma == $scope.Programa.IdPrograma) {

                        $scope.Ficha.IdPrograma = value.IdPrograma
                    }
                });

                var x = $("#FechaInicio").val().split('/');
                var y = $("#FechaFin").val().split('/');

                var StartDate = x[1] + "-" + x[2] + "-" + x[0];
                var EndDate = y[1] + "-" + y[2] + "-" + y[0];
                if (Date.parse(StartDate) >= Date.parse(EndDate)) {
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


                if ($scope.Ficha.Ficha1 == "" || $scope.Ficha.NumAprendices == null || $scope.Ficha.IdPrograma == "" || $scope.Ficha.TipoFormacion == "" ||
                    $scope.Ficha.FechaInicio == "" || $scope.Ficha.FechaFin == "" || $scope.Ficha.Jornada == "") {
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
                } else {
                    FichaService.GuardarFicha($scope.Ficha, function (response) {
                        if (response.success == true && response.respuesta == true) {
                            $("#ModalFicha").modal("hide");
                            $scope.VaciarCampos();
                            FichaService.ConsultarFichas(function (response) {

                                if (response.success == true) {


                                    if ($rootScope.globals.currentUser.tipousuario == 2) {

                                        FichaService.ConsultarFichasxArea($rootScope.globals.currentUser.idpersona, function (response) {
                                            if (response.success == true) {
                                                $scope.datalists = response.datos;
                                                $scope.numberOfPages = function () {
                                                    return Math.ceil($scope.datalists.length / $scope.pageSize);
                                                };
                                                $.each($scope.datalists, function (index, value) {
                                                    var fechaInicio = value.Parametro6.toString().substring(0, 10);
                                                    var fechaFin = value.Parametro7.toString().substring(0, 10);
                                                    $scope.datalists[index].Parametro6 = fechaInicio;
                                                    $scope.datalists[index].Parametro7 = fechaFin;
                                                });
                                            }
                                        });

                                    } else {

                                        FichaService.ConsultarFichas(function (response) {
                                            if (response.success == true) {
                                                $scope.datalists = response.datos;
                                                $scope.numberOfPages = function () {
                                                    return Math.ceil($scope.datalists.length / $scope.pageSize);
                                                };
                                                $.each($scope.datalists, function (index, value) {
                                                    var fechaInicio = value.Parametro6.toString().substring(0, 10);
                                                    var fechaFin = value.Parametro7.toString().substring(0, 10);
                                                    $scope.datalists[index].Parametro6 = fechaInicio;
                                                    $scope.datalists[index].Parametro7 = fechaFin;
                                                });
                                            }
                                        });
                                    }



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

                                }
                            });
                        } else {

                            bootbox.dialog({
                                title: "Registro",
                                message: "La ficha ya se encuentra registrada",
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



            if ($rootScope.globals.currentUser.tipousuario == 2) {

                //$("#boton").hide();
                $("#excel").hide();
                //$("#eliminar").hide();
                //$("#modificar").hide();
                $("#descargar").hide();
                //$("#DescargarFichas").hide();
                $("#inhabilitadas").hide();

            }

            if ($rootScope.globals.currentUser.tipousuario == 2) {
                FichaService.ConsultarFichasxArea($rootScope.globals.currentUser.idpersona, function (response) {
                    if (response.success == true) {
                        $scope.datalists = response.datos;
                        $scope.ListaCompleta = response.datos;
                        $scope.numberOfPages = function () {
                            return Math.ceil($scope.datalists.length / $scope.pageSize);
                        };
                        $.each($scope.datalists, function (index, value) {
                            var fechaInicio = value.Parametro6.toString().substring(0, 10);
                            var fechaFin = value.Parametro7.toString().substring(0, 10);
                            $scope.datalists[index].Parametro6 = fechaInicio;
                            $scope.datalists[index].Parametro7 = fechaFin;
                        });
                    }
                });

            } else {

                FichaService.ConsultarFichas(function (response) {
                    if (response.success == true) {
                        $scope.datalists = response.datos;
                        $scope.ListaCompleta = response.datos;
                        //$scope.Ficha = response.datos;
                        $scope.numberOfPages = function () {
                            return Math.ceil($scope.datalists.length / $scope.pageSize);
                        };
                        $.each($scope.datalists, function (index, value) {
                            var fechaInicio = value.Parametro6.toString().substring(0, 10);
                            var fechaFin = value.Parametro7.toString().substring(0, 10);
                            $scope.datalists[index].Parametro6 = fechaInicio;
                            $scope.datalists[index].Parametro7 = fechaFin;
                        });
                    }
                });
            }


            $scope.FichasInactivas = function () {
                FichaService.ConsultarFichasInactivas(function (response) {

                    if (response.success == true) {

                        $scope.datalists = response.datos;
                        $scope.ListaCompleta = response.datos;
                        $scope.numberOfPages = function () {
                            return Math.ceil($scope.datalists.length / $scope.pageSize);
                        };
                        $.each($scope.datalists, function (index, value) {
                            var fechaInicio = value.Parametro6.toString().substring(0, 10);
                            var fechaFin = value.Parametro7.toString().substring(0, 10);
                            $scope.datalists[index].Parametro6 = fechaInicio;
                            $scope.datalists[index].Parametro7 = fechaFin;
                        });



                    }
                });

                $("#eliminar").hide();
                //$("#modificar").hide();
                $("#inhabilitadas").hide();
                $("#atras").show();

            }


            $scope.atras = function () {

                $("#eliminar").show();
                $("#modificar").show();
                $("#inhabilitadas").show();
                $("#atras").hide();

                if ($rootScope.globals.currentUser.tipousuario == 2) {

                    FichaService.ConsultarFichasxArea($rootScope.globals.currentUser.idpersona, function (response) {
                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.ListaCompleta = response.datos;
                            //$scope.Ficha = response.datos;
                            $scope.numberOfPages = function () {
                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                            };
                            $.each($scope.datalists, function (index, value) {
                                var fechaInicio = value.Parametro6.toString().substring(0, 10);
                                var fechaFin = value.Parametro7.toString().substring(0, 10);
                                $scope.datalists[index].Parametro6 = fechaInicio;
                                $scope.datalists[index].Parametro7 = fechaFin;
                            });
                        }
                    });

                } else {

                    FichaService.ConsultarFichas(function (response) {
                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.ListaCompleta = response.datos;
                            //$scope.Ficha = response.datos;
                            $scope.numberOfPages = function () {
                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                            };
                            $.each($scope.datalists, function (index, value) {
                                var fechaInicio = value.Parametro6.toString().substring(0, 10);
                                var fechaFin = value.Parametro7.toString().substring(0, 10);
                                $scope.datalists[index].Parametro6 = fechaInicio;
                                $scope.datalists[index].Parametro7 = fechaFin;
                            });
                        }
                    });
                }

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

                if (UsariosBorrar.length == 0 || UsariosBorrar.length > 1) {

                    bootbox.dialog({
                        title: "Inhabilitar",
                        message: "Debe por lo menos seleccionar una ficha",
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

                var FichaBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });
                FichaService.BorrarFicha(FichaBorrar, function (response) {

                    if (response.success == true) {
                        FichaService.ConsultarFichas(function (response) {
                            if (response.success == true) {
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
                                $scope.datalists = response.datos;
                                $scope.ListaCompleta = response.datos;
                                $scope.numberOfPages = function () {
                                    return Math.ceil($scope.datalists.length / $scope.pageSize);
                                };
                            }
                        });
                    }

                });

            };

            $scope.Modificar = function () {

                var FichaModificar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;

                });

                if (FichaModificar.length == 1) {

                    FichaService.ModificarFicha(FichaModificar, function (response) {

                        if (response.success == true) {

                            if ($rootScope.globals.currentUser.tipousuario == 2) {
                                ListaProgramas1
                                $("#listas").prop("disabled", true);
                                $("#ListaProgramas1").prop("disabled", true);
                                $("#ficha13").prop("disabled", true);
                                $("#FechaInicio1").prop("disabled", true);
                            }
                            $scope.Ficha.IdFicha = response.Ficha.IdFicha;
                            //$scope.Ficha.IdArea = response.Ficha.IdArea;
                            $scope.Ficha.Ficha1 = parseInt(response.Ficha.Ficha1);
                            $scope.Ficha.NumAprendices = response.Ficha.NumAprendices;
                            $scope.Ficha.Estado = response.Ficha.Estado;
                            $scope.Ficha.FechaInicio = (response.Ficha.FechaInicio.toString()).substring(0, 10);
                            $scope.Ficha.FechaFin = (response.Ficha.FechaFin.toString()).substring(0, 10);
                            $scope.Ficha.IdPrograma = response.Ficha.IdPrograma;
                            $scope.Ficha.TipoFormacion = response.Ficha.TipoFormacion;
                            $scope.Ficha.Jornada = response.Ficha.Jornada;
                            FichaService.ConsultarAreaxPrograma(response.Ficha.IdPrograma, function (response) {

                                if (response.success == true) {

                                    $("#listas > option[value='" + response.dato.Parametro1 + "']").attr('selected', 'selected');

                                    FichaService.ConsultarProgramaxArea(response.dato.Parametro1, function (response) {

                                        if (response.success == true) {
                                            $scope.Programa = response.programa;
                                            setTimeout(function () {
                                                $("#ListaProgramas1 > option[value='" + $scope.Ficha.IdPrograma + "']").attr('selected', 'selected');
                                            }, 1000);
                                            $("#TipoFormacion > option[value='" + $scope.Ficha.TipoFormacion.toUpperCase() + "']").attr('selected', 'selected');
                                            $("#Jornada > option[value='" + $scope.Ficha.Jornada.toUpperCase() + "']").attr('selected', 'selected');
                                            $("#ModalEditar").modal("show");
                                        }
                                    });

                                }
                            });
                        }
                    });
                } else {

                    bootbox.dialog({
                        title: "Editar",
                        message: "Debe seleccionar una ficha",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                }
            };

            $scope.GuardarEdicionFicha = function () {
                $.each($scope.Programa, function (index, value) {

                    if (value.NombrePrograma == $scope.Programa.NombrePrograma) {

                        $scope.Ficha.IdPrograma = value.IdPrograma
                    }
                });

                if ($scope.Ficha.Ficha1 == "" || $scope.Ficha.NumAprendices == "" || $scope.Ficha.IdPrograma == "" || $scope.Ficha.TipoFormacion == "" ||
                    $scope.Ficha.FechaInicio == "" || $scope.Ficha.FechaFin == "" || $scope.Ficha.Jornada == "") {
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
                } else {
                    FichaService.GuardarModificacionFicha($scope.Ficha, function (response) {
                        if (response.success == true) {
                            $("#ModalEditar").modal("hide");

                            if ($rootScope.globals.currentUser.tipousuario == 2) {

                                FichaService.ConsultarFichasxArea($rootScope.globals.currentUser.idpersona, function (response) {
                                    if (response.success == true) {
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
                                        $scope.datalists = response.datos;
                                        $scope.ListaCompleta = response.datos;
                                        //$scope.Ficha = response.datos;
                                        $scope.numberOfPages = function () {
                                            return Math.ceil($scope.datalists.length / $scope.pageSize);
                                        };
                                        $.each($scope.datalists, function (index, value) {
                                            var fechaInicio = value.Parametro6.toString().substring(0, 10);
                                            var fechaFin = value.Parametro7.toString().substring(0, 10);
                                            $scope.datalists[index].Parametro6 = fechaInicio;
                                            $scope.datalists[index].Parametro7 = fechaFin;
                                        });
                                    }
                                });

                            } else {
                                if ($scope.Ficha.Estado == true) {
                                    FichaService.ConsultarFichas(function (response) {
                                        if (response.success == true) {
                                            $scope.datalists = response.datos;
                                            $scope.ListaCompleta = response.datos;
                                            //$scope.Ficha = response.datos;
                                            $scope.numberOfPages = function () {
                                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                                            };
                                            $.each($scope.datalists, function (index, value) {
                                                var fechaInicio = value.Parametro6.toString().substring(0, 10);
                                                var fechaFin = value.Parametro7.toString().substring(0, 10);
                                                $scope.datalists[index].Parametro6 = fechaInicio;
                                                $scope.datalists[index].Parametro7 = fechaFin;
                                            });
                                        }
                                    });
                                } else {

                                    $scope.FichasInactivas();
                                }

                            }



                        }
                    });
                }
            };

            //if ($rootScope.globals.currentUser.tipousuario == 2) {

            //    $("#excel").hide();
            //    $("#eliminar").hide();
            //    $("#modificar").hide();
            //    $("#descargar").hide();
            //}



            $scope.DescargarReporte = function () {

                $scope.ProgramasExport = [];
                $scope.Plantilla = [{
                    Código_Programa: "", Ficha: "", Cantiad_Aprendices: "", Tipo_De_Formacion: "", Fecha_Inicio: "", Fecha_Fin_Lectiva: "", Jornada: ""
                }];

                FichaService.ConsultarProgramas(function (response) {

                    if (response.success == true) {
                        $scope.Programa = response.datos;

                        $.each($scope.Programa, function (index, value) {

                            $scope.ProgramasExport.push({
                                Codigo_Programa: value.CodigoPrograma, Nombre_Programa: value.NombrePrograma, Nivel: value.Nivel
                            });

                        });
                        alasql('SELECT * INTO XLSX("Codigos Programas.xlsx",{headers:true}) FROM ?', [$scope.ProgramasExport]);
                    }
                });
                alasql('SELECT * INTO XLSX("Plantilla Ficha.xlsx",{headers:true}) FROM ?', [$scope.Plantilla]);
            };

            $scope.DescargarFichas = function () {

                $scope.FichasExport = [];
                if ($rootScope.globals.currentUser.tipousuario == 2) {
                    FichaService.ConsultarFichasxArea($rootScope.globals.currentUser.idpersona, function (response) {
                        if (response.success == true) {

                            $.each(response.datos, function (index, value) {

                                $scope.FichasExport.push({
                                    NOMBRE_PROGRAMA: value.Parametro8.toUpperCase(), FICHA: value.Parametro2.toUpperCase(), NUMERO_APRENDICES: value.Parametro3,
                                    NIVEL: value.Parametro4.toUpperCase(), FECHA_INICIO: value.Parametro6.toUpperCase(), FECHA_FIN: value.Parametro7.toUpperCase(), JORNADA: value.Parametro9.toUpperCase()
                                });
                            });
                            alasql('SELECT * INTO XLSX("Fichas.xlsx",{headers:true}) FROM ?', [$scope.FichasExport]);

                        }
                    });
                } else {
                    FichaService.ConsultarFichas(function (response) {
                        if (response.success == true) {
                            $.each(response.datos, function (index, value) {
                                $scope.FichasExport.push({
                                    NOMBRE_PROGRAMA: value.Parametro8.toUpperCase(), FICHA: value.Parametro2.toUpperCase(), NUMERO_APRENDICES: value.Parametro3,
                                    NIVEL: value.Parametro4.toUpperCase(), FECHA_INICIO: value.Parametro6.toUpperCase(), FECHA_FIN: value.Parametro7.toUpperCase(), JORNADA: value.Parametro9.toUpperCase()
                                });

                            });
                            alasql('SELECT * INTO XLSX("Fichas.xlsx",{headers:true}) FROM ?', [$scope.FichasExport]);
                        }
                    });
                }
            };



            $scope.Filter = function (e) {

                //    var rex = new RegExp($(e.target).val(), 'i');
                //    $('.searchable tr').hide();
                //    $('.searchable tr').filter(function () {
                //        return rex.test($(this).text());
                //    }).show();
                //};

                //$scope.ClearFilter = function () {
                //    $('.searchable tr').show();
                var Busqueda = $("#Buscar").val();
                var exp = new RegExp(Busqueda);
                if (Busqueda == "") {
                    if ($rootScope.globals.currentUser.tipousuario == 2) {

                        FichaService.ConsultarFichasxArea($rootScope.globals.currentUser.idpersona, function (response) {
                            if (response.success == true) {
                                $scope.datalists = response.datos;
                                $scope.ListaCompleta = response.datos;
                                //$scope.Ficha = response.datos;
                                $.each($scope.datalists, function (index, value) {
                                    var fechaInicio = value.Parametro6.toString().substring(0, 10);
                                    var fechaFin = value.Parametro7.toString().substring(0, 10);
                                    $scope.datalists[index].Parametro6 = fechaInicio;
                                    $scope.datalists[index].Parametro7 = fechaFin;
                                });
                                $scope.numberOfPages = function () {
                                    return Math.ceil($scope.datalists.length / $scope.pageSize);
                                };
                            }
                        });

                    } else {
                        var display = $("#atras").css("display");

                        if (display == "none") {
                            FichaService.ConsultarFichas(function (response) {
                                if (response.success == true) {
                                    $scope.datalists = response.datos;
                                    $scope.ListaCompleta = response.datos;
                                    //$scope.Ficha = response.datos;

                                    $.each($scope.datalists, function (index, value) {
                                        var fechaInicio = value.Parametro6.toString().substring(0, 10);
                                        var fechaFin = value.Parametro7.toString().substring(0, 10);
                                        $scope.datalists[index].Parametro6 = fechaInicio;
                                        $scope.datalists[index].Parametro7 = fechaFin;
                                    });
                                    $scope.numberOfPages = function () {
                                        return Math.ceil($scope.datalists.length / $scope.pageSize);
                                    };
                                }
                            });
                        } else {
                            $scope.FichasInactivas();
                        }


                    }
                }
                var Ficha = [];
                $scope.datalists = $scope.ListaCompleta;
                Ficha = $scope.datalists.filter(function (item) {

                    if (exp.test(item.Parametro8.toLowerCase()) || exp.test(item.Parametro8.toUpperCase())) {

                        return item;
                    }

                    else if (exp.test(item.Parametro2.toLowerCase()) || exp.test(item.Parametro2.toUpperCase())) {
                        return item;
                    }

                    else if (exp.test(item.Parametro3.toLowerCase()) || exp.test(item.Parametro3.toUpperCase())) {
                        return item;
                    }

                    else if (exp.test(item.Parametro4.toLowerCase()) || exp.test(item.Parametro4.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.Parametro6.toLowerCase()) || exp.test(item.Parametro6.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.Parametro7.toLowerCase()) || exp.test(item.Parametro7.toUpperCase())) {
                        return item;
                    }

                });
                $scope.datalists = Ficha;
                //Variable para setear la paginación 
                $scope.curPage = 0;
            };

            //Función para consultar las areas 
            FichaService.ConsultarAreas(function (response) {

                if (response.success == true) {
                    $scope.Area = response.datos;
                }
            });



            $scope.ConsultarProgramaxArea = function (IdArea) {
                FichaService.ConsultarProgramaxArea(IdArea, function (response) {

                    if (response.success == true) {
                        $scope.Programa = response.programa;
                    }
                });

            }



            ///-------------------------------------------------------Reporte de progamación por ficha------------------------------------------


            $scope.Fechas = {

                FechaInicio: "",
                FechaFin: ""
            }

            $('#FechaInicio2').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#FechaFin2').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $scope.Programacion = {
                IdFicha: '',
                IdResultado: '',
                IdAmbiente: '',
                IdInstructor: '',
                FechaInicio: '',
                FechaFin: '',
                HoraInicio: '',
                HoraFin: '',
                Jornada: "",
                DiaSemana: ""
            };


            FichaService.ConsultarResultados(function (response) {
                if (response.success == true) {
                    $scope.Resultado = response.datos;

                }
            });


            FichaService.ConsultarInstructores(function (response) {
                if (response.success == true) {
                    $scope.Instructor = response.datos;

                }
            });

            $scope.ModalReporteFicha = function () {

                $scope.FichaSeleccionada = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;

                });



                if ($scope.FichaSeleccionada.length == 0 || $scope.FichaSeleccionada.length > 1) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe seleccionar una ficha",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });

                } else {
                    $("#ModalReporte").modal("show");
                }
            };

            $scope.GenerarReporte = function () {


                var x = $("#FechaInicio2").val().split('/');
                var y = $("#FechaFin2").val().split('/');

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

                FichaService.ReporteProgramacion($scope.Fechas, $scope.FichaSeleccionada[0].Parametro1, function (response) {

                    if (response.success == true) {

                        $scope.Programacion = response.datos;
                        $scope.ProgramacionFichaExport = [];


                        $.each($scope.Programacion, function (index, value) {

                            var fechaIini = value.FechaInicio.split('T');
                            var fechafin = value.FechaFin.split('T');


                            $.each($scope.FichaSeleccionada, function (index, value1) {
                                if (value1.Parametro1 == value.IdFicha) {
                                    value.IdFicha = value1.Parametro2
                                }
                            });

                            $.each($scope.Resultado, function (index, value1) {
                                if (value1.Parametro1 == value.IdResultado) {
                                    value.IdResultado = value1.Parametro4
                                }
                            });


                            $.each($scope.Instructor, function (index, value1) {
                                if (value1.IdInstructor == value.IdInstructor) {
                                    value.IdInstructor = value1.Nombre + " " + value1.Apellido

                                }
                            });

                            $scope.ProgramacionFichaExport.push({
                                Ficha: value.IdFicha, Fecha_Inicio: fechaIini[0], Fecha_Fin: fechafin[0],
                                Hora_Inicio: value.HoraInicio, Hora_Fin: value.HoraFin,
                                Resultado: value.IdResultado, Instructor: value.IdInstructor
                            });

                        });

                        alasql('SELECT * INTO XLSX("Reporte Programación.xlsx",{headers:true}) FROM ?', [$scope.ProgramacionFichaExport]);

                    }
                });
            }


        }]);