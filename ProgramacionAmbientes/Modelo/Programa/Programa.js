ProgramacionApp.controller('ProgramaController',
    ['$scope', '$rootScope', '$location', 'ProgramaService', '$routeParams', '$sce',
        function ($scope, $rootScope, $location, ProgramaService, $routeParams, $sce) {

            var nav = $("#navbar").hasClass("gn-menu-wrapper gn-open-all");
            if (nav == true) {
                $(".Principal").css("width", "80%");

            } else {
                $(".Principal").css("width", "95%");
            }

            $scope.UploadFileWeb = function () {
                $("#fileUploadWeb").trigger('click');
            };

            //$(".botones").css("display", "block");

            if ($rootScope.globals.currentUser.tipousuario == 2) {
                $(".botones").css("display", "none");
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
                        ProgramaService.SubirArchivo(dataweb, function (response) {
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
                                ProgramaService.ConsultarProgramas(function (response) {
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

            $scope.curPage = 0;
            $scope.pageSize = 5;



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


            $scope.Area = {
                IdArea: "",
                Codigo: "",
                Nombre: "",
                Descripcion: "",
                IdCoordinacion: ""

            };

            //Función para vaciar campos
            //$scope.VaciarCampos = function () {
            //    $scope.Programa.IdPrograma = "";
            //    $scope.Programa.CodigoPrograma = "";
            //    $scope.Programa.Nivel = "";
            //    $scope.Programa.LineaTecnologica = "";
            //    $scope.Programa.Red_Tecnologica = "";
            //    $scope.Programa.Perfil_Instructor = "";
            //    $scope.Programa.Version_Programa = "";
            //    $scope.Programa.IdArea = "";
            //    $scope.Programa.NombrePrograma = "";
            //}

            $scope.AbrirModal = function () {

                $("#ModalPrograma").modal("show");
                $scope.VaciarCampos();
            };


            ProgramaService.ConsultarAreas(function (response) {
                if (response.success == true) {
                    $scope.Area = response.datos;

                }
            });



            //Función para guardar un area
            $scope.Guardar = function () {
                $.each($scope.Area, function (index, value) {
                    if (value.IdArea == $scope.Area.IdArea) {

                        $scope.Programa.IdArea = value.IdArea;
                    }
                });

                if ($scope.Programa.CodigoPrograma != "" || $scope.Programa.Nivel != "" || $scope.Programa.LineaTecnologica != "" || $scope.Programa.Red_Tecnologica != ""
                   || $scope.Programa.Perfiel_Instructor != "" || $scope.Programa.Version_Programa != "" || $scope.Programa.IdArea != "" || $scope.Programa.NombrePrograma != "") {

                    ProgramaService.GuardarPrograma($scope.Programa, function (response) {
                        if (response.success == true) {
                            $("#ModalPrograma").modal("hide");

                            ProgramaService.ConsultarProgramas(function (response) {
                                if (response.success == true) {
                                    $scope.datalists = response.datos;
                                    $scope.ListaCompleta = response.datos;
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
                        }
                    });
                } else {

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
                }
            };

            

            //Función para consultar las areas
            if ($rootScope.globals.currentUser.tipousuario == 2) {
                ProgramaService.ProgramaxCoordinacion($rootScope.globals.currentUser.idpersona, function (response) {
                    if (response.success == true) {
                        $scope.datalists = response.datos;
                        $scope.Datos = $scope.datalists;
                        $scope.ListaCompleta = response.datos;
                        $scope.numberOfPages = function () {
                            return Math.ceil($scope.datalists.length / $scope.pageSize);
                        };
                    }

                });
            } else {
                ProgramaService.ConsultarProgramas(function (response) {

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



            //Función para Seleccionar todos las registros de la tabla
            $scope.SeleccionarTodos = function () {

                contador = (($scope.curPage + 1) * 3) - 3;
                var item = (($scope.curPage + 1) * 3) - 3;
                var items1 = (($scope.curPage + 1) * 3);
                // $scope.value = $scope.datalists;
                // var i = 0; for ($scope = items; i <= items1; i++) {

                //     $scope.datalists.Seleccionado = $scope.SeleccionTodos;
                //} 

                $.each($scope.datalists, function (index, value) {

                    if (contador < items1) {
                        value[contador];
                        value.Seleccionado = $scope.SeleccionTodos;
                        contador++;

                    }
                });


            };



            //Función 1 para el borrado de las areas
            $scope.CambiarEstadoSeleccionados = function () {
                var UsariosBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });

                if (UsariosBorrar.length == 0) {

                    bootbox.dialog({
                        title: "Eliminar",
                        message: "Debe por lo menos seleccionar una sede",
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

            //Función 2 para el borrado de las areas
            $scope.inhabilitar = function () {

                var ProgramaBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });
                ProgramaService.BorrarPrograma(ProgramaBorrar, function (response) {

                    if (response.success == true && response.respuesta == true) {
                        ProgramaService.ConsultarProgramas(function (response) {
                            if (response.success == true) {
                                $scope.datalists = response.datos;
                                $scope.ListaCompleta = response.datos;
                            }

                        })

                        bootbox.dialog({
                            title: "Eliminar",
                            message: "Se elimaron las siguiente cantidad de registros " + response.contadorTrue,
                            buttons: {
                                success: {
                                    label: "Cerrar",
                                    className: "btn-primary",
                                }
                            }
                        });


                    } else {

                        bootbox.dialog({
                            title: "Eliminar",
                            message: "No se puedieron elimanar la siguiente cantidad de registros " + response.contadorFalse + " porque estan asociados a otras tablas.",
                            buttons: {
                                success: {
                                    label: "Cerrar",
                                    className: "btn-primary",
                                }
                            }
                        });
                    }

                });

            };

            //Función 1 para editar las areas
            $scope.Modificar = function () {

                var ProgramaModificar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;

                });


                if (ProgramaModificar.length == 1) {

                    ProgramaService.ModificarPrograma(ProgramaModificar, function (response) {

                        if (response.success == true) {

                            $scope.Programa.IdPrograma = response.programa.IdPrograma;
                            $scope.Programa.CodigoPrograma = response.programa.CodigoPrograma;
                            $scope.Programa.Nivel = response.programa.Nivel;
                            $scope.Programa.LineaTecnologica = response.programa.LineaTecnologica;
                            $scope.Programa.Red_Tecnologica = response.programa.Red_Tecnologica;
                            $scope.Programa.Perfil_Instructor = response.programa.Perfil_Instructor;
                            $scope.Programa.Version_Programa = response.programa.Version_Programa;
                            $scope.Programa.NombrePrograma = response.programa.NombrePrograma;
                            $scope.Programa.IdArea = response.programa.IdArea;
                            $("#AreaLista > option[value='" + response.programa.IdArea + "']").attr('selected', 'selected');
                            console.log(response.programa.Nivel);
                            $("#NivelLista > option[value='" + response.programa.Nivel + "']").attr('selected', 'selected');
                            $("#ModalEditar").modal("show");


                        }
                    });
                } else {

                    bootbox.dialog({
                        title: "Editar",
                        message: "Debe seleccionar un sede",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                }
            }

            //Función 2 para editar las areas
            $scope.GuardarEdicionPrograma = function () {
                $.each($scope.Area, function (index, value) {
                    if (value.IdArea == $scope.Area.IdArea) {

                        $scope.Programa.IdArea = value.IdArea;

                    }

                });

                if ($scope.Programa.CodigoPrograma != "" || $scope.Programa.Nivel != "" || $scope.Programa.LineaTecnologica != "" || $scope.Programa.Red_Tecnologica != ""
                  || $scope.Programa.Perfiel_Instructor != "" || $scope.Programa.Version_Programa != "" || $scope.Programa.IdArea != 0 || $scope.Programa.NombrePrograma != "") {
                    ProgramaService.GuardarModificacionPrograma($scope.Programa, function (response) {
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

                            ProgramaService.ConsultarProgramas(function (response) {
                                if (response.success == true) {
                                    $scope.datalists = response.datos;
                                    $scope.ListaCompleta = response.datos;
                                }
                            });
                        }
                    });
                } else {
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
                }
            }


            $scope.DescargarReporte = function () {

                $scope.AreasExport = [];

                $scope.Plantilla = [{
                    Codigo: "", Nombre_Programa: "", Nivel_Academico: "", Linea_Tecnologica: "", Red_Tecnologica: "", Perfil_Instructor: "", Version:"",Código_Area:""
                }];
                $.each($scope.Area, function (index, value) {

                    $scope.AreasExport.push({
                        Codigo_Area: value.Codigo, Nombre: value.Nombre
                    });

                });


                alasql('SELECT * INTO XLSX("Códigos Area.xlsx",{headers:true}) FROM ?', [$scope.AreasExport]);
                alasql('SELECT * INTO XLSX("Plantilla Programas.xlsx",{headers:true}) FROM ?', [$scope.Plantilla]);

            };



            $scope.ReporteProgramas = function () {

                var coordinador = 0;
                if ($rootScope.globals.currentUser.tipousuario == 2) {
                    coordinador = $rootScope.globals.currentUser.idpersona;
                } 

                ProgramaService.ReporteProgramas(coordinador,function (response) {
                    if (response.success == true) {
                        alasql('SELECT * INTO XLSX("Reporte de Programas.xlsx",{headers:true}) FROM ?', [response.Datos]);
                    }
                });
            };


            //Función 1 para filtrar la tabla
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
                        ProgramaService.ProgramaxCoordinacion($rootScope.globals.currentUser.idpersona, function (response) {
                            if (response.success == true) {
                                $scope.datalists = response.datos;
                                $scope.Datos = $scope.datalists;
                                $scope.ListaCompleta = response.datos;
                                $scope.numberOfPages = function () {
                                    return Math.ceil($scope.datalists.length / $scope.pageSize);
                                };
                            }

                        });
                    } else {
                        ProgramaService.ConsultarProgramas(function (response) {

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
                }
                var Programa = [];
                $scope.datalists = $scope.ListaCompleta;
                Programa = $scope.datalists.filter(function (item) {

                    //if (exp.test(item.Parametro2.toLowerCase()) || exp.test(item.Parametro9.toLowerCase()) || exp.test(item.Parametro7.toLowerCase()) || exp.test(item.Parametro5.toLowerCase()) || exp.test(item.Parametro3.toLowerCase()) || exp.test(item.Parametro8.toLowerCase())) {

                    //    return item;
                    //}

                    if (exp.test(item.Parametro2.toLowerCase()) || exp.test(item.Parametro2.toUpperCase())) {

                        return item;
                    }
                    else if (exp.test(item.Parametro9.toLowerCase()) || exp.test(item.Parametro9.toUpperCase())) {

                        return item;
                    }
                    else if (exp.test(item.Parametro7.toLowerCase()) || exp.test(item.Parametro7.toUpperCase())) {

                        return item;
                    }
                    else if (exp.test(item.Parametro5.toLowerCase()) || exp.test(item.Parametro5.toUpperCase())) {

                        return item;
                    }
                    else if (exp.test(item.Parametro3.toLowerCase()) || exp.test(item.Parametro3.toUpperCase())) {

                        return item;
                    }
                    else if (exp.test(item.Parametro8.toLowerCase()) || exp.test(item.Parametro8.toUpperCase())) {

                        return item;
                    }

                });
                $scope.datalists = Programa;
                //Variable para setear la paginación 
                $scope.curPage = 0;


            };
        }]);