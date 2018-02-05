ProgramacionApp.controller('CoordinacionController',
    ['$scope', '$rootScope', '$location', 'CoordinacionService', '$routeParams', '$sce',
        function ($scope, $rootScope, $location, CoordinacionService, $routeParams, $sce) {

            var nav = $("#navbar").hasClass("gn-menu-wrapper gn-open-all");
            if (nav == true) {
                $(".Principal").css("width", "80%");

            } else {
                $(".Principal").css("width", "95%");
            }

            $scope.Coordinacion = {
                Nombre_Coordinacion: "",
                Cedula: "",
                Nombre: "",
                Apellido: "",
                Telefono: "",
                Correo: "",
                IdArea: ""
            };

            $scope.AbrirModal = function () {
                $("#ModalCoordinacion").modal("show");
                $scope.VaciarCampos();
            };

            $scope.VaciarCampos = function () {
                $scope.Coordinacion.Nombre_Coordinacion = "";
                $scope.Coordinacion.Cedula = "";
                $scope.Coordinacion.Nombre = "";
                $scope.Coordinacion.Apellido = "";
                $scope.Coordinacion.Telefono = "";
                $scope.Coordinacion.Correo = "";
            };

            //Función para consultar las areas 
            CoordinacionService.ConsultarAreas(function (response) {
                if (response.success == true) {
                    $scope.Area = response.datos;
                }
            });

            $scope.Guardar = function () {
                $.each($scope.Area, function (index, value) {

                    if (value.IdArea == $scope.Area.IdArea) {

                        $scope.Coordinacion.IdArea = value.IdArea
                    }
                });
                if ($scope.Coordinacion.Nombre_Coordinacion == "" || $scope.Coordinacion.Cedula == "" || $scope.Coordinacion.Nombre == "" || $scope.Coordinacion.Apellido == "" || $scope.Coordinacion.Telefono == "" || $scope.Coordinacion.Correo == "" || $scope.Coordinacion.IdArea == "") {
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
                    CoordinacionService.GuardarCoordinacion($scope.Coordinacion, function (response) {
                        if (response.success == true) {
                            if (response.coordinador == 1) {
                                $scope.VaciarCampos();
                                CoordinacionService.ConsultarCoordinacion(function (response) {
                                    if (response.success == true) {
                                        $scope.datalists = response.datos;
                                        $scope.ListaCompleta = response.datos;
                                    }
                                });                                
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
                            } else if (response.coordinador == 2) {
                                bootbox.dialog({
                                    title: "Información",
                                    message: "La cédula del coordinador ya se encuentra registrada",
                                    buttons: {
                                        success: {
                                            label: "Cerrar",
                                            className: "btn-primary",
                                        }
                                    }
                                });
                            } else {
                                bootbox.dialog({
                                    title: "Información",
                                    message: "La coordinación ya tiene un coordinador asociado",
                                    buttons: {
                                        success: {
                                            label: "Cerrar",
                                            className: "btn-primary",
                                        }
                                    }
                                });
                            }

                            $("#ModalCoordinacion").modal("hide");
                            
                        }
                    });
                }
            };

            CoordinacionService.ConsultarCoordinacion(function (response) {
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

            $scope.CambiarEstadoSeleccionados = function () {
                var UsariosBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });

                if (UsariosBorrar.length == 0) {

                    bootbox.dialog({
                        title: "Eliminar",
                        message: "Seleccione por lo menos un coordinador",
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

                var CoordinacionesBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });
                CoordinacionService.BorrarCoordinaciones(CoordinacionesBorrar, function (response) {

                    if (response.success == true) {
                        CoordinacionService.ConsultarCoordinacion(function (response) {
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
                            }
                        });
                    }

                });

            };

            $scope.Modificar = function () {
                var CoordinacionModificar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });
                if (CoordinacionModificar.length == 1) {
                    CoordinacionService.ModificarCoordinacion(CoordinacionModificar, function (response) {
                        if (response.success == true) {
                            $scope.Coordinacion.IdCoordinacion = response.Coordinacion.IdCoordinacion;
                            $scope.Coordinacion.Nombre_Coordinacion = response.Coordinacion.Nombre_Coordinacion;
                            $scope.Coordinacion.Cedula = parseInt(response.Coordinacion.Cedula);
                            $scope.Coordinacion.Nombre = response.Coordinacion.Nombre;
                            $scope.Coordinacion.Apellido = response.Coordinacion.Apellido;
                            $scope.Coordinacion.Telefono = parseInt(response.Coordinacion.Telefono);
                            $scope.Coordinacion.Correo = response.Coordinacion.Correo;
                            $scope.Coordinacion.IdArea = response.Coordinacion.IdArea;
                            $("#AreaLista > option[value='" + response.Coordinacion.IdArea + "']").attr('selected', 'selected');

                            $("#ModalEditar").modal("show");
                        }
                    });
                } else {
                    bootbox.dialog({
                        title: "Editar",
                        message: "Seleccione un coordinador",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                }
            };

            $scope.GuardarEdicionCoordinacion = function () {
                $.each($scope.Area, function (index, value) {

                    if (value.IdArea == $scope.Area.IdArea) {

                        $scope.Coordinacion.IdArea = value.IdArea
                    }
                });
                if ($scope.Coordinacion.Nombre_Coordinacion == "" || $scope.Coordinacion.Cedula == "" || $scope.Coordinacion.Nombre == "" || $scope.Coordinacion.Apellido == "" || $scope.Coordinacion.Telefono == "" || $scope.Coordinacion.Correo == "" || $scope.Coordinacion.IdArea == "") {
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
                    CoordinacionService.GuardarModificacionCoordinacion($scope.Coordinacion, function (response) {
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
                            CoordinacionService.ConsultarCoordinacion(function (response) {
                                if (response.success == true) {
                                    $scope.datalists = response.datos;
                                    $scope.ListaCompleta = response.datos;
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
                    CoordinacionService.ConsultarCoordinacion(function (response) {
                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.ListaCompleta = response.datos;
                        }
                    });
                }
                var Coordinacion = [];
                $scope.datalists = $scope.ListaCompleta;
                Coordinacion = $scope.datalists.filter(function (item) {

                  

                    if (exp.test(item.Cedula.toLowerCase()) || exp.test(item.Cedula.toUpperCase())) {

                        return item;
                    }

                    else if (exp.test(item.Nombre_Coordinacion.toLowerCase()) || exp.test(item.Nombre_Coordinacion.toUpperCase())) {
                        return item;
                    }

                    else if (exp.test(item.Nombre.toLowerCase()) || exp.test(item.Nombre.toUpperCase())) {
                        return item;
                    }

                    else if (exp.test(item.Apellido.toLowerCase()) || exp.test(item.Apellido.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.Correo.toLowerCase()) || exp.test(item.Correo.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.Telefono.toLowerCase()) || exp.test(item.Telefono.toUpperCase())) {
                        return item;
                    }

                });
                $scope.datalists = Coordinacion;
                //Variable para setear la paginación 
                $scope.curPage = 0;
            };

           

        }]);