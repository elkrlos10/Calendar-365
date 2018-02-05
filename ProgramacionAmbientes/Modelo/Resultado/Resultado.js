ProgramacionApp.controller('ResultadoController',
    ['$scope', '$rootScope', '$location', 'ResultadoService', '$routeParams', '$sce',
        function ($scope, $rootScope, $location, ResultadoService, $routeParams, $sce) {

            var nav = $("#navbar").hasClass("gn-menu-wrapper gn-open-all");
            if (nav == true) {
                $(".Principal").css("width", "80%");

            } else {
                $(".Principal").css("width", "95%");
            }
                   
            $scope.curPage = 0;
            $scope.pageSize = 5;           


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
                        ResultadoService.SubirArchivo(dataweb, function (response) {
                            if (response.success == true) {

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
                                ResultadoService.ConsultarResultados(function (response) {
                                    if (response.success == true) {
                                        $scope.datalists = response.datos;
                                        $scope.ListaCompleta = response.datos;
                                        $scope.numberOfPages = function () {
                                            return Math.ceil($scope.datalists.length / $scope.pageSize);
                                        };
                                    }
                                });

                                $("#fileUploadWeb").replaceWith($("#fileUploadWeb").val('').clone(true));
                                //waitingDialog.hide();
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
                $("#ModalResultado").modal("show");
                $scope.VaciarCampos();
            };

            $scope.Resultado = {
                IdResultado: "",
                Codigo: "",
                Resultado: "",
                IdCompetencia: ""
            };

            ResultadoService.ConsultarCompetencias(function (response) {
                if (response.success == true) {
                    $scope.Competencia = response.datos;
                }
            });

            $scope.Guardar = function () {
                $.each($scope.Competencia, function (index, value) {
                    if (value.IdCompetencia == $scope.Competencia.IdCompetencia) {
                        $scope.Resultado.IdCompetencia = value.IdCompetencia;
                    }
                });
                if ($scope.Resultado.IdCompetencia == "" || $scope.Resultado.Resultado == "" || $scope.Resultado.Codigo == "") {
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
                    ResultadoService.GuardarResultado($scope.Resultado, function (response) {
                        if (response.success == true) {
                            $scope.VaciarCampos();
                            $("#ModalResultado").modal("hide");
                            ResultadoService.ConsultarResultados(function (response) {
                                if (response.success == true) {
                                    $scope.datalists = response.datos;
                                    $scope.ListaCompleta = response.datos;
                                    $scope.numberOfPages = function () {
                                        return Math.ceil($scope.datalists.length / $scope.pageSize);
                                    };
                                    $scope.Datos = $scope.datalists;

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
                                title: "Información",
                                message: "El resultado de aprendizaje ya se encuentra registrado",
                                buttons: {
                                    success: {
                                        label: "Cerrar",
                                        className: "btn-primary",
                                    }
                                }
                            });
                            return;
                        }
                    });
                }
            };

            ResultadoService.ConsultarResultados(function (response) {
                if (response.success == true) {
                    $scope.datalists = response.datos;
                    $scope.ListaCompleta = response.datos;
                    $scope.Datos = $scope.datalists;
                    $scope.numberOfPages = function () {
                        return Math.ceil($scope.datalists.length / $scope.pageSize);
                    };
                }
            });

            $scope.VaciarCampos = function () {
                $scope.Resultado.IdCompetencia = "";
                $scope.Resultado.Resultado = "";
                $scope.Resultado.Codigo = "";
            };

            $scope.Modificar = function () {
                var ResultadosModificar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });

                if (ResultadosModificar.length == 1) {
                    ResultadoService.ModificarResultado(ResultadosModificar, function (response) {
                        if (response.success == true) {
                            $scope.Resultado.IdResultado = response.Resultado.IdResultado;
                            $scope.Resultado.IdCompetencia = response.Resultado.IdCompetencia;
                            $scope.Resultado.Codigo = response.Resultado.Codigo;
                            $scope.Resultado.Resultado = response.Resultado.Resultado;
                            $("#CompetenciaLista > option[value='" + response.Resultado.IdCompetencia + "']").attr('selected', 'selected');
                            $("#ModalEditar").modal("show");
                        }
                    });
                } else {
                    bootbox.dialog({
                        title: "Editar",
                        message: "Seleccione un resultado",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                }
            };


            $scope.GuardarEdicionResultado = function () {
                $.each($scope.Competencia, function (index, value) {
                    if (value.IdCompetencia == $scope.Competencia.IdCompetencia) {
                        $scope.Resultado.IdCompetencia = value.IdCompetencia;
                    }
                });

                if ($scope.Resultado.IdCompetencia == "" || $scope.Resultado.Resultado == "" || $scope.Resultado.Codigo == "") {
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
                    ResultadoService.GuardarModificacionResultado($scope.Resultado, function (response) {
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
                            $scope.VaciarCampos();
                            $("#ModalEditar").modal("hide");
                            ResultadoService.ConsultarResultados(function (response) {
                                if (response.success == true) {
                                    $scope.datalists = response.datos;
                                    $scope.ListaCompleta = response.datos;
                                    $scope.numberOfPages = function () {
                                        return Math.ceil($scope.datalists.length / $scope.pageSize);
                                    };
                                    $scope.Datos = $scope.datalists;
                                }
                            });
                        }
                    });
                }
            };


            $scope.CambiarEstadoSeleccionados = function () {
                var UsariosBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });

                if (UsariosBorrar.length == 0) {

                    bootbox.dialog({
                        title: "Eliminar",
                        message: "Seleccione por lo menos un resultado",
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

                var ResultadosBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });
                ResultadoService.BorrarResultados(ResultadosBorrar, function (response) {
                    if (response.success == true) {
                        ResultadoService.ConsultarResultados(function (response) {
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
                                $scope.Datos = $scope.datalists;
                                $scope.numberOfPages = function () {
                                    return Math.ceil($scope.datalists.length / $scope.pageSize);
                                };
                            }
                        });
                    }

                });

            };


            $scope.DescargarReporte = function () {

                $scope.CompetenciasExport = [];

                $scope.Plantilla = [{
                    Código_Resultado: "", Resultado_Aprendizaje: "", Código_Competencia: ""
                }];

                $.each($scope.Competencia, function (index, value) {

                    $scope.CompetenciasExport.push({
                        Codigo_Competencia: value.Codigo, Nombre_Competencia: value.Nombre
                    });

                });

                alasql('SELECT * INTO XLSX("Codigos Competencias.xlsx",{headers:true}) FROM ?', [$scope.CompetenciasExport]);
                alasql('SELECT * INTO XLSX("Plantilla Resultados.xlsx",{headers:true}) FROM ?', [$scope.Plantilla]);
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
                    ResultadoService.ConsultarResultados(function (response) {
                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.ListaCompleta = response.datos;
                            $scope.Datos = $scope.datalists;
                            $scope.numberOfPages = function () {
                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                            };
                        }
                    });
                }
                var Resultado = [];
                $scope.datalists = $scope.ListaCompleta;
                Resultado = $scope.datalists.filter(function (item) {

                  
                    if (exp.test(item.Parametro2.toLowerCase()) || exp.test(item.Parametro2.toUpperCase())) {

                        return item;
                    }

                    else if (exp.test(item.Parametro3.toLowerCase()) || exp.test(item.Parametro3.toUpperCase())) {
                        return item;
                    }

                    else if (exp.test(item.Parametro4.toLowerCase()) || exp.test(item.Parametro4.toUpperCase())) {
                        return item;
                    }

                });
                $scope.datalists = Resultado;
                //Variable para setear la paginación 
                $scope.curPage = 0;
            };


        }]);