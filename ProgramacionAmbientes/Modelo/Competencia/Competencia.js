ProgramacionApp.controller('CompetenciaController',
    ['$scope', '$rootScope', '$location', 'CompetenciaService', '$routeParams', '$sce',
        function ($scope, $rootScope, $location, CompetenciaService, $routeParams, $sce) {

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
                        CompetenciaService.SubirArchivo(dataweb, function (response) {
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
                                CompetenciaService.ConsultarCompetencias(function (response) {
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




            $scope.Competencia = {
                IdCompetencia: "",
                Nombre: "",
                Horas: "",
                IdPrograma: "",
                Codigo: ""
            };

            $scope.AbrirModal = function () {
                $("#ModalCompetencia").modal("show");
                $scope.VaciarCampos();
            };



            $scope.Guardar = function () {


                $.each($scope.Programa, function (index, value) {

                    if (value.IdPrograma == $scope.Programa.IdPrograma) {

                        $scope.Competencia.IdPrograma = value.IdPrograma;
                    }
                });
                if ($scope.Competencia.Nombre == "" || $scope.Competencia.Codigo == "" || $scope.Competencia.IdPrograma == "" || $scope.Competencia.Horas == "") {
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
                    CompetenciaService.GuardarCompetencia($scope.Competencia, function (response) {
                        if (response.success == true) {
                            $("#ModalCompetencia").modal("hide");
                            $scope.VaciarCampos();
                            CompetenciaService.ConsultarCompetencias(function (response) {
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

                                }
                            });
                        } else {
                            bootbox.dialog({
                                title: "Información",
                                message: "La competencia ya se encuentra  registrada",
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


            $scope.VaciarCampos = function () {
                $scope.Competencia.Nombre = "";
                $scope.Competencia.Descripcion = "";
                $scope.Competencia.Codigo = "";
            }

            //Función para consultar las competencias
            CompetenciaService.ConsultarCompetencias(function (response) {
                
                if (response.success == true) {

                    
                    $scope.datalists = response.datos;
                    $scope.ListaCompleta = response.datos;
                    $scope.numberOfPages = function () {
                        return Math.ceil($scope.datalists.length / $scope.pageSize);
                    };

                    $scope.Datos = $scope.datalists;
                }
            });

            //Función para seleccionar todos los registros de la tabla
            $scope.SeleccionarTodos = function () {
                $.each($scope.Datos, function (index, value) {
                    value.Seleccionado = $scope.SeleccionTodos;
                });
            };


            $scope.BorrarSeleccionados = function () {
                var CompetenciasBorrar = $scope.Datos.filter(function (item) {
                    return item.Seleccionado === true;
                });
                if (CompetenciasBorrar.length == 0) {
                    alert("Seleccione un área para borrar");
                }
                if (CompetenciasBorrar.length > 0) {
                    r = confirm("¿Desea eliminar las áreas?");
                    if (r == true) {
                        CompetenciaService.BorrarCompetencias(CompetenciasBorrar, function (response) {
                            if (response.success == true) {

                            }
                        })
                    }
                }
            };

            //Función 1 para eliminar registros de la tabla
            $scope.CambiarEstadoSeleccionados = function () {
                var UsariosBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });

                if (UsariosBorrar.length == 0) {

                    bootbox.dialog({
                        title: "Eliminar",
                        message: "Seleccione por lo menos una competencia",
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
            }

            //Función 2 para eliminar registros de la tabla
            $scope.inhabilitar = function () {

                var CompetenciaBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });
                CompetenciaService.BorrarCompetencia(CompetenciaBorrar, function (response) {

                    if (response.success == true) {
                        CompetenciaService.ConsultarCompetencias(function (response) {
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
                        })
                    }

                });

            };

            //Función 1 para editar un registro de la tabla
            $scope.Modificar = function () {

                var CompetenciaModificar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;

                });


                if (CompetenciaModificar.length == 1) {

                    CompetenciaService.ModificarCompetencia(CompetenciaModificar, function (response) {

                        if (response.success == true) {

                            $scope.Competencia.IdCompetencia = response.Competencia.IdCompetencia;
                            $scope.Competencia.Nombre = response.Competencia.Nombre;
                            $scope.Competencia.Descripcion = response.Competencia.Descripcion;
                            $scope.Competencia.Codigo = response.Competencia.Codigo;
                            $scope.Competencia.Horas = response.Competencia.Horas;
                            $scope.Competencia.IdPrograma = response.Competencia.IdPrograma;
                            $("#ProgramaLista > option[value='" + response.Competencia.IdPrograma + "']").attr('selected', 'selected');
                            $("#ModalEditar").modal("show");
                        }
                    });
                } else {

                    bootbox.dialog({
                        title: "Editar",
                        message: "Seleccione una competencia",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                }
            }

            //Función 2 para editar un registro de la tabla
            $scope.GuardarEdicionCompetencia = function () {
                $.each($scope.Programa, function (index, value) {

                    if (value.IdPrograma == $scope.Programa.IdPrograma) {

                        $scope.Competencia.IdPrograma = value.IdPrograma;
                    }
                });
                if ($scope.Competencia.Nombre == "" || $scope.Competencia.Descripcion == "" || $scope.Competencia.Codigo == "" || $scope.Competencia.IdPrograma == "" || $scope.Competencia.Horas == "") {
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
                    CompetenciaService.GuardarModificacionCompetencia($scope.Competencia, function (response) {
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
                            $("#ModalEditar").modal("hide");
                            $scope.VaciarCampos();
                            CompetenciaService.ConsultarCompetencias(function (response) {
                                if (response.success == true) {
                                    $scope.datalists = response.datos;
                                    $scope.ListaCompleta = response.datos;
                                    $scope.numberOfPages = function () {
                                        return Math.ceil($scope.datalists.length / $scope.pageSize);
                                    };
                                }
                            });
                        }
                    });
                }
            };



            //Función para descargar un archivo excel con todos los 
            $scope.DescargarReporte = function () {

                $scope.ProgramasExport = [];

                $scope.Plantilla = [{
                    Código_Competencia: "", Nombre_Competencia: "", Horas: "", Código_Programa: ""
                }];

                $.each($scope.Programa, function (index, value) {

                    $scope.ProgramasExport.push({
                        Codigo_Programa: value.CodigoPrograma, Nombre_Programa: value.NombrePrograma
                    });

                });

                alasql('SELECT * INTO XLSX("Codigos Programas.xlsx",{headers:true}) FROM ?', [$scope.ProgramasExport]);
                alasql('SELECT * INTO XLSX("Plantilla Competencias.xlsx",{headers:true}) FROM ?', [$scope.Plantilla]);
            };


            //Función para filtrar la tabla
            $scope.Filter = function (e) {

                //var rex = new RegExp($(e.target).val(), 'i');
                //$('.searchable tr').hide();
                //$('.searchable tr').filter(function () {
                //    return rex.test($(this).text());
                //}).show();

                var Busqueda = $("#Buscar").val();
                var exp = new RegExp(Busqueda);
                if (Busqueda == "") {
                    CompetenciaService.ConsultarCompetencias(function (response) {
                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.ListaCompleta = response.datos;
                            $scope.numberOfPages = function () {
                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                            };
                        }
                        return;
                    });
                    
                }
                var Competencia = [];
                $scope.datalists = $scope.ListaCompleta;
                Competencia = $scope.datalists.filter(function (item) {
                   

                        if (exp.test(item.Parametro2.toLowerCase()) || exp.test(item.Parametro2.toUpperCase())) {

                            return item;
                        }

                        else if (exp.test(item.Parametro5.toLowerCase()) || exp.test(item.Parametro5.toUpperCase())) {
                            return item;
                        }

                        else if (exp.test(item.Parametro3.toLowerCase()) || exp.test(item.Parametro3.toUpperCase())) {
                            return item;
                        }

                        else if (exp.test(item.Parametro4.toLowerCase()) || exp.test(item.Parametro4.toUpperCase())) {
                            return item;
                        }

                });
                $scope.datalists = Competencia;
                //Variable para setear la paginación 
                $scope.curPage = 0;
            };

            
            //Función para consultar los programas 
            CompetenciaService.ConsultarProgramas(function (response) {
                
                if (response.success == true) {
                    $scope.Programa = response.datos;
                }
            });




        }]);