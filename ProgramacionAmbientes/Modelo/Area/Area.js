ProgramacionApp.controller('AreaController',
    ['$scope', '$rootScope', '$location', 'AreaService', '$routeParams', '$sce',
        function ($scope, $rootScope, $location, AreaService, $routeParams, $sce) {

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
                        AreaService.SubirArchivo(dataweb, function (response) {
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
                                AreaService.ConsultarAreas(function (response) {
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




            $scope.Area = {
                IdArea: "",
                Nombre: "",
                Descripcion: "",
                IdCoordinacion: "",
                Codigo: ""
            };

            // pagination
            $scope.curPage = 0;
            $scope.pageSize = 5;




            $scope.AbrirModal = function () {
                $("#ModalArea").modal("show");
                $scope.VaciarCampos();
            };

            //Función para guardar un area
            $scope.Guardar = function () {
                if ($scope.Area.Nombre != "" || $scope.Area.Descripcion != "" || $scope.Area.Codigo != "") {

                    AreaService.GuardarArea($scope.Area, function (response) {
                        if (response.success == true) {
                            $("#ModalArea").modal("hide");
                            $scope.VaciarCampos();
                            AreaService.ConsultarAreas(function (response) {
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
                        } else {
                            bootbox.dialog({
                                title: "Información",
                                message: "El área ya se encuentra registrada",
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

            //Función para vaciar campos
            $scope.VaciarCampos = function () {
                $scope.Area.Nombre = "";
                $scope.Area.Descripcion = "";
                $scope.Area.IdCoordinacion = "";
                $scope.Area.Codigo = "";
                document.getElementById("area").reset();
            }

            //Función para consultar las areas
            AreaService.ConsultarAreas(function (response) {

                if (response.success == true) {

                    $scope.curPage = 0;
                    $scope.pageSize = 5;
                    $scope.datalists = response.datos;
                    $scope.ListaCompleta = response.datos;
                    $scope.numberOfPages = function () {
                        return Math.ceil($scope.datalists.length / $scope.pageSize);
                    };

                    $scope.Datos = $scope.datalists;
                }
            });


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


            $scope.BorrarSeleccionados = function () {
                var AreasBorrar = $scope.Datos.filter(function (item) {
                    return item.Seleccionado === true;
                });
                if (AreasBorrar.length == 0) {
                    alert("Seleccione un área para borrar");
                }
                if (AreasBorrar.length > 0) {
                    r = confirm("¿Desea eliminar las áreas?");
                    if (r == true) {
                        AreaService.BorrarAreas(AreasBorrar, function (response) {
                            if (response.success == true) {
                                alert("Eliminado!!");
                            }
                        })
                    }
                }
            };

            //Función 1 para el borrado de las areas
            $scope.CambiarEstadoSeleccionados = function () {
                var UsariosBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });

                if (UsariosBorrar.length == 0) {

                    bootbox.dialog({
                        title: "Eliminar",
                        message: "Seleccione por lo menos un área",
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

                var AreaBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });
                AreaService.BorrarArea(AreaBorrar, function (response) {

                    if (response.success == true && response.respuesta == true) {
                        AreaService.ConsultarAreas(function (response) {
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
                            title: "Inhabilitar",
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

                var AreaModificar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;

                });


                if (AreaModificar.length == 1) {

                    AreaService.ModificarArea(AreaModificar, function (response) {

                        if (response.success == true) {

                            $scope.Area.IdArea = response.Area.IdArea;
                            $scope.Area.Nombre = response.Area.Nombre;
                            $scope.Area.Descripcion = response.Area.Descripcion;
                            $scope.Area.Codigo = response.Area.Codigo;
                            $("#ModalEditar").modal("show");
                        }
                    });
                } else {

                    bootbox.dialog({
                        title: "Editar",
                        message: "Debe seleccionar un área",
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
            $scope.GuardarEdicionArea = function () {
                if ($scope.Area.Nombre != "" && $scope.Area.Descripcion != "" || $scope.Area.Codigo != "") {
                    AreaService.GuardarModificacionArea($scope.Area, function (response) {
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
                            AreaService.ConsultarAreas(function (response) {
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
                    AreaService.ConsultarAreas(function (response) {
                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.ListaCompleta = response.datos;
                        }
                    });
                }
                var Area = [];
                $scope.datalists = $scope.ListaCompleta;
                Area = $scope.datalists.filter(function (item) {


                    if (exp.test(item.Parametro5.toLowerCase()) || exp.test(item.Parametro5.toUpperCase())) {

                        return item;
                    }

                    else if (exp.test(item.Parametro4.toLowerCase()) || exp.test(item.Parametro4.toUpperCase())) {
                        return item;
                    }

                    else if (exp.test(item.Parametro2.toLowerCase()) || exp.test(item.Parametro2.toUpperCase())) {
                        return item;
                    }
                });
                $scope.datalists = Area;
                //Variable para setear la paginación 
                $scope.curPage = 0;

            };


        }]);