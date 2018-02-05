ProgramacionApp.controller('InstructorController',
    ['$scope', '$rootScope', '$location', 'InstructorService', '$routeParams', '$sce',
        function ($scope, $rootScope, $location, InstructorService, $routeParams, $sce) {

            var nav = $("#navbar").hasClass("gn-menu-wrapper gn-open-all");
            if (nav == true) {
                $(".Principal").css("width", "80%");

            } else {
                $(".Principal").css("width", "95%");
            }

            $("#atras").hide();
            $("#habilitar").hide();

            //Funciones para la carga del archivo de excel
            $scope.SubirArchivo = function () {
                InstructorService.alerta();
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

            $('#FechaInicio3').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#FechaFin3').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#FechaInicio4').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#FechaFin4').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $scope.Fechas = {

                FechaInicio: "",
                FechaFin: ""
            }

            $scope.datalists = [];

            $scope.curPage = 0;
            $scope.pageSize = 5;


            $scope.Meses = [
                { Id: 1, Mes: "Enero" }, { Id: 2, Mes: "Febrero" }, { Id: 3, Mes: "Marzo" }, { Id: 4, Mes: "Abril" }, { Id: 5, Mes: "Mayo" },
                { Id: 6, Mes: "Junio" }, { Id: 7, Mes: "Julio" }, { Id: 8, Mes: "Agosto" }, { Id: 9, Mes: "Septiembre" }, { Id: 10, Mes: "Octubre" },
                { Id: 11, Mes: "Noviembre" }, { Id: 12, Mes: "Diciembre" }
            ];

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
                        InstructorService.SubirArchivo(dataweb, function (response) {
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
                                InstructorService.ConsultarInstructores(function (response) {
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
            //Funciones para la carga del archivo de excel

            //Función para abrir una modal
            $scope.AbrirModal = function () {
                $("#ModalInstructor").modal("show");
                $scope.VaciarCampos();
            };

            //Declaración del objeto instructor
            $scope.Instructor = {
                IdInstructor: "",
                Nombre: "",
                Apellido: "",
                Cedula: "",
                Email: "",
                Estado: "",
                TipoContrato: "",
                Telefono: "",
                IdArea: "",
                TipoInstructor: ""
            };

            $scope.Contrato = [
                { Tipo: 1, Nombre: "Contratista" }, { Tipo: 2, Nombre: "Planta" }
            ];

            $scope.Estado = [
                { Id: 1, Nombre: "Activo" }, { Id: 0, Nombre: "Inactivo" }
            ];

            //Función para registrar un instructor
            $scope.Guardar = function () {
                $scope.Instructor.Estado = true;
                $.each($scope.Contrato, function (index, value) {
                    if (value.Tipo == $scope.Contrato.Tipo) {
                        $scope.Instructor.TipoContrato = value.Tipo
                    }
                });


                $.each($scope.Areas, function (index, value) {
                    if (value.Parametro1 == $scope.Areas.Parametro1) {
                        $scope.Instructor.IdArea = value.Parametro1;
                    }
                });
                if ($scope.Instructor.TipoInstructor == true) {
                    $scope.Instructor.TipoInstructor = 2
                } else {
                    $scope.Instructor.TipoInstructor = 1
                }
                if ($scope.Instructor.Nombre == "" || $scope.Instructor.IdArea == "" || $scope.Instructor.Apellido == "" || $scope.Instructor.Cedula == null || $scope.Instructor.Email == "" || $scope.Instructor.TipoContrato == null) {
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
                    InstructorService.GuardarInstructor($scope.Instructor, function (response) {
                        if (response.success == true) {
                            $scope.VaciarCampos();

                            $("#ModalInstructor").modal("hide");
                            if ($rootScope.globals.currentUser.tipousuario == 1) {

                                InstructorService.ConsultarInstructores(function (response) {
                                    if (response.success == true) {

                                        $scope.datalists = response.datos;
                                        $scope.ListaCompleta = response.datos;
                                        $scope.Datos = $scope.datalists;
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
                                InstructorService.ConsultarInstructoresxArea($rootScope.globals.currentUser.idpersona, function (response) {
                                    if (response.success == true) {
                                        $scope.datalists = response.datos;
                                        $scope.ListaCompleta = response.datos;
                                        $scope.numberOfPages = function () {
                                            return Math.ceil($scope.datalists.length / $scope.pageSize);
                                        };
                                    }
                                });
                            }
                        } else {
                            bootbox.dialog({
                                title: "Información",
                                message: "La cédula del instructor ya se encuentra registrada",
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



            //Función para consultar todos los instructores activos 
            if ($rootScope.globals.currentUser.tipousuario == 1) {

                InstructorService.ConsultarInstructores(function (response) {
                    if (response.success == true) {
                        $scope.datalists = response.datos;
                        $scope.ListaCompleta = response.datos;
                        $scope.numberOfPages = function () {
                            return Math.ceil($scope.datalists.length / $scope.pageSize);
                        };
                    }
                });
            } else {
                InstructorService.ConsultarInstructoresxArea($rootScope.globals.currentUser.idpersona, function (response) {
                    if (response.success == true) {
                        $scope.datalists = response.datos;
                        $scope.ListaCompleta = response.datos;
                        $scope.numberOfPages = function () {
                            return Math.ceil($scope.datalists.length / $scope.pageSize);
                        };

                        $("#excel").hide();
                        $("#descargar").hide();
                        $("#AreaLista1").prop("disabled", true);
                        $("#cedulaInstructor1").prop("disabled", true);
                        $("#nombreInstructor1").prop("disabled", true);
                        $("#apellidoInstructor1").prop("disabled", true);
                        $("#correoInstructor1").prop("disabled", true);

                    }
                });
            }


            $scope.DescargarInstructores = function () {
                $scope.InstructoresExport = [];
                if ($rootScope.globals.currentUser.tipousuario == 1) {

                    InstructorService.ConsultarInstructores(function (response) {
                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.ListaCompleta = response.datos;
                            $scope.numberOfPages = function () {
                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                            };

                            $.each(response.datos, function (index, value) {

                                $scope.InstructoresExport.push({
                                    NOMBRE: value.Nombre.toUpperCase(), APELLIDO: value.Apellido.toUpperCase(), CEDULA: value.Cedula,
                                    TELEFONO: value.Telefono.toUpperCase(), CORREO: value.Email.toUpperCase(), TIPO_INSTRUCTOR: value.NombreTipoInstructor.toUpperCase(), TIPO_CONTRATO: value.NombreTipoContrato.toUpperCase()
                                });
                            });
                            alasql('SELECT * INTO XLSX("Instructores.xlsx",{headers:true}) FROM ?', [$scope.InstructoresExport]);
                        }
                    });
                } else {
                    InstructorService.ConsultarInstructoresxArea($rootScope.globals.currentUser.idpersona, function (response) {
                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.ListaCompleta = response.datos;
                            $scope.numberOfPages = function () {
                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                            };

                            $.each(response.datos, function (index, value) {

                                $scope.InstructoresExport.push({
                                    NOMBRE: value.Nombre.toUpperCase(), APELLIDO: value.Apellido.toUpperCase(), CEDULA: value.Cedula,
                                    TELEFONO: value.Telefono.toUpperCase(), CORREO: value.Email.toUpperCase(), TIPO_INSTRUCTOR: value.NombreTipoInstructor.toUpperCase(), TIPO_CONTRATO: value.NombreTipoContrato.toUpperCase()
                                });
                            });
                            alasql('SELECT * INTO XLSX("Instructores.xlsx",{headers:true}) FROM ?', [$scope.InstructoresExport]);
                        }
                    });
                }
            }

            //Función para consultar las programas 
            InstructorService.ConsultarProgramas(function (response) {
                if (response.success == true) {
                    $scope.Programa = response.datos;
                }
            });

            //Función para seleccionar todos los instructores de la tabla 
            $scope.SeleccionarTodos = function () {
                $.each($scope.Datos, function (index, value) {
                    value.Seleccionado = $scope.SeleccionTodos;
                });
            };

            //Función para abrir la modal de confirmación para el cambio del estado del instructor de activo a inactivo
            $scope.CambiarEstadoSeleccionados = function () {
                var UsariosBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });

                if (UsariosBorrar.length == 0) {

                    bootbox.dialog({
                        title: "Inhabilitar",
                        message: "Debe por lo menos seleccionar un instructor",
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

            $scope.HabilitarInstructor = function () {
                var InstructorHabilitar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });
                if (InstructorHabilitar.length == 0) {
                    bootbox.dialog({
                        title: "Inhabilitar",
                        message: "Debe por lo menos seleccionar un instructor",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                } else {
                    InstructorService.HabilitarInstructor(InstructorHabilitar, function (response) {
                        if (response.success == true) {
                            //InstructorService.ConsultarInstructores(function (response) {
                            //    if (response.success == true) {
                            //        $scope.datalists = response.datos;
                            //        $("#atras").hide();
                            //        $("#habilitar").hide();
                            //        $("#eliminar").show();
                            //        $("#modificar").show();
                            //        $("#descargar").show();
                            //        $("#excel").show();
                            //        $("#inhabilitados").show();
                            //    }
                            //});
                            if ($rootScope.globals.currentUser.tipousuario == 1) {

                                InstructorService.ConsultarInstructores(function (response) {
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
                            } else {
                                InstructorService.ConsultarInstructoresxArea($rootScope.globals.currentUser.idpersona, function (response) {
                                    if (response.success == true) {
                                        $scope.datalists = response.datos;
                                        $scope.ListaCompleta = response.datos;
                                        $scope.numberOfPages = function () {
                                            return Math.ceil($scope.datalists.length / $scope.pageSize);
                                        };
                                    }
                                });
                            }

                            $("#atras").hide();
                            $("#habilitar").hide();
                            $("#eliminar").show();
                            $("#modificar").show();
                            $("#descargar").show();
                            $("#excel").show();
                            $("#inhabilitados").show()
                        }
                    });
                }
            };

            //Función para para el cambio del estado del instructor de activo a inactivo
            $scope.inhabilitar = function () {

                var InstructorBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });
                InstructorService.CambiarEstado(InstructorBorrar, function (response) {

                    if (response.success == true) {
                        if ($rootScope.globals.currentUser.tipousuario == 1) {

                            InstructorService.ConsultarInstructores(function (response) {
                                if (response.success == true) {


                                    $scope.datalists = response.datos;
                                    $scope.numberOfPages = function () {
                                        return Math.ceil($scope.datalists.length / $scope.pageSize);
                                    };
                                    $scope.Datos = $scope.datalists;

                                }
                            });
                        } else {
                            InstructorService.ConsultarInstructoresxArea($rootScope.globals.currentUser.idpersona, function (response) {
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

                });

            };

            //Función para cargar los campos de la modal con los datos del instructor a modificar  
            $scope.Modificar = function () {

                var InstructorModificar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;

                });


                if (InstructorModificar.length == 1) {

                    InstructorService.ModificarInstructor(InstructorModificar, function (response) {

                        if (response.success == true) {

                            $scope.Instructor.IdInstructor = response.Instructor.IdInstructor;
                            $scope.Instructor.Nombre = response.Instructor.Nombre;
                            $scope.Instructor.Apellido = response.Instructor.Apellido;
                            $scope.Instructor.Cedula = parseInt(response.Instructor.Cedula);
                            $scope.Instructor.Email = response.Instructor.Email;
                            $scope.Instructor.Telefono = parseInt(response.Instructor.Telefono);
                            $scope.Instructor.IdArea = response.Instructor.IdArea;

                            if (response.Instructor.TipoInstructor == 2) {
                                $scope.Instructor.TipoInstructor = true;
                            } else {
                                $scope.Instructor.TipoInstructor = false;
                            }

                            $("#AreaLista1 > option[value='" + response.Instructor.IdArea + "']").attr('selected', 'selected');
                            $("#TipoContrato > option[value='" + response.Instructor.TipoContrato + "']").attr('selected', 'selected');
                            $scope.Instructor.TipoContrato = response.Instructor.TipoContrato;


                            $("#ModalEditar").modal("show");

                        }
                    });
                } else {

                    bootbox.dialog({
                        title: "Editar",
                        message: "Debe seleccionar un instructor",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                }
            }

            //Función para guardar la modificación sobre un registro de un instructor
            $scope.GuardarEdicionInstructor = function () {
                $.each($scope.Contrato, function (index, value) {
                    if (value.Tipo == $scope.Contrato.Tipo) {
                        $scope.Instructor.TipoContrato = value.Tipo
                    }
                });
                $.each($scope.Estado, function (index, value) {
                    if (value.Id == $scope.Estado.Id) {
                        $scope.Instructor.Estado = value.Id
                    }
                });
                $.each($scope.Areas, function (index, value) {
                    if (value.Parametro1 == $scope.Areas.Parametro1) {
                        $scope.Instructor.IdArea = value.Parametro1;
                    }
                });
                if ($scope.Instructor.TipoInstructor == true) {
                    $scope.Instructor.TipoInstructor = 2
                } else {
                    $scope.Instructor.TipoInstructor = 1
                }
                if ($scope.Instructor.Nombre == "" || $scope.Instructor.Apellido == "" || $scope.Instructor.IdArea == "" || $scope.Instructor.Cedula == "" || $scope.Instructor.Email == "" || $scope.Instructor.TipoContrato == "") {
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
                    InstructorService.GuardarModificacionInstructor($scope.Instructor, function (response) {
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
                            if ($rootScope.globals.currentUser.tipousuario == 1) {

                                InstructorService.ConsultarInstructores(function (response) {
                                    if (response.success == true) {

                                        $scope.datalists = response.datos;
                                        $scope.ListaCompleta = response.datos;
                                        $scope.numberOfPages = function () {
                                            return Math.ceil($scope.datalists.length / $scope.pageSize);
                                        };
                                        $scope.Datos = $scope.datalists;

                                    }
                                });
                            } else {
                                InstructorService.ConsultarInstructoresxArea($rootScope.globals.currentUser.idpersona, function (response) {
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
                    });
                }
            }

            $scope.ConsultarInhabilitados = function () {

                if ($rootScope.globals.currentUser.tipousuario == 1) {

                    InstructorService.ConsultarInhabilitados(function (response) {

                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.ListaCompleta = response.datos;
                            $scope.numberOfPages = function () {
                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                            };
                        }
                    });
                } else {
                    InstructorService.ConsultarInhabilitadosxArea($rootScope.globals.currentUser.idpersona, function (response) {

                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.ListaCompleta = response.datos;
                            $scope.numberOfPages = function () {
                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                            };
                        }
                    });
                }


                $("#atras").show();
                $("#habilitar").show();
                $("#eliminar").hide();
                $("#modificar").hide();
                $("#descargar").hide();
                $("#excel").hide();
                $("#inhabilitados").hide();
            };

            $scope.ConsultarHabilitados = function () {
                if ($rootScope.globals.currentUser.tipousuario == 1) {

                    InstructorService.ConsultarInstructores(function (response) {
                        if (response.success == true) {

                            $scope.datalists = response.datos;
                            $scope.ListaCompleta = response.datos;
                            $scope.numberOfPages = function () {
                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                            };
                            $scope.Datos = $scope.datalists;

                        }
                    });
                } else {
                    InstructorService.ConsultarInstructoresxArea($rootScope.globals.currentUser.idpersona, function (response) {
                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.ListaCompleta = response.datos;
                            $scope.numberOfPages = function () {
                                return Math.ceil($scope.datalists.length / $scope.pageSize);
                            };
                        }
                    });
                }

                $("#atras").hide();
                $("#habilitar").hide();
                $("#eliminar").show();
                $("#modificar").show();
                $("#descargar").show();
                $("#excel").show();
                $("#inhabilitados").show();
            };

            //Función para vaciar los campos de las modales
            $scope.VaciarCampos = function () {

                $scope.Instructor.Nombre = "";
                $scope.Instructor.Apellido = "";
                $scope.Instructor.Cedula = "";
                $scope.Instructor.Email = "";
                $scope.Instructor.Telefono = "";
            }

            //Funciones para el filtro sobre la tabla
            //$scope.Filter = function (e) {

            //    var rex = new RegExp($(e.target).val(), 'i');
            //    $('.searchable tr').hide();
            //    $('.searchable tr').filter(function () {
            //        return rex.test($(this).text());
            //    }).show();
            //};

            //$scope.ClearFilter = function () {
            //    $('.searchable tr').show();
            //};

            //Funciòn para consultar las areas
            InstructorService.ConsultarAreas(function (response) {
                if (response.success == true) {
                    $scope.Areas = response.datos;

                }
            });

            //Función para realizar el filtro sobre la tabla
            $scope.Filtrar = function () {
                var Busqueda = $("#Buscar").val();
                var exp = new RegExp(Busqueda);
                if (Busqueda == "") {
                    if ($rootScope.globals.currentUser.tipousuario == 1) {

                        InstructorService.ConsultarInstructores(function (response) {
                            if (response.success == true) {

                                $scope.datalists = response.datos;
                                $scope.ListaCompleta = response.datos;
                                $scope.numberOfPages = function () {
                                    return Math.ceil($scope.datalists.length / $scope.pageSize);
                                };
                                $scope.Datos = $scope.datalists;

                            }
                        });
                    } else {
                        InstructorService.ConsultarInstructoresxArea($rootScope.globals.currentUser.idpersona, function (response) {
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
                var Instructor = [];
                $scope.datalists = $scope.ListaCompleta;
                Instructor = $scope.datalists.filter(function (item) {

                    //if (exp.test(item.Cedula.toLowerCase()) || exp.test(item.Nombre.toLowerCase()) || exp.test(item.Apellido.toLowerCase()) || exp.test(item.Email.toLowerCase()) || exp.test(item.Telefono.toLowerCase())) {

                    //    return item;
                    //}

                    if (exp.test(item.Cedula.toLowerCase()) || exp.test(item.Cedula.toUpperCase())) {

                        return item;
                    }

                    else if (exp.test(item.Nombre.toLowerCase()) || exp.test(item.Nombre.toUpperCase())) {
                        return item;
                    }

                    else if (exp.test(item.Apellido.toLowerCase()) || exp.test(item.Apellido.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.Email.toLowerCase()) || exp.test(item.Email.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.Telefono.toLowerCase()) || exp.test(item.Telefono.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.NombreTipoContrato.toLowerCase()) || exp.test(item.NombreTipoContrato.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.NombreTipoInstructor.toLowerCase()) || exp.test(item.NombreTipoInstructor.toUpperCase())) {
                        return item;
                    }


                });
                $scope.datalists = Instructor;
                //Variable para setear la paginación 
                $scope.curPage = 0;
            };



           

            //Función para descargar un archivo excel con todos los instructores
            $scope.DescargarReporte = function () {

                $scope.ProgramasExport = [];

                $scope.Plantilla = [{
                    Nombres: "", Apellidos: "", Cedula: "", Email: "", Telefono: "", Código_Area: "", Tipo_Contrato: "", Tipo_Instructor: ""
                }];

                $.each($scope.Areas, function (index, value) {

                    $scope.ProgramasExport.push({
                        Codigo_Area: value.Parametro2, Nombre_Area: value.Parametro4
                    });

                });

                alasql('SELECT * INTO XLSX("Codigos Areas.xlsx",{headers:true}) FROM ?', [$scope.ProgramasExport]);
                alasql('SELECT * INTO XLSX("Plantilla Instructor Areas.xlsx",{headers:true}) FROM ?', [$scope.Plantilla]);
            };


            $scope.Resultado = {
                Codigo: "",
                Resultado: "",
                IdCompetencia: ""
            };

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

            $scope.Ambiente = {
                Numero: "",
                IdArea: "",
                Piso: "",
                Pantalla: "",
                NumeroEquipos: "",
            };


            $scope.Ficha = {
                IdFicha: "",
                IdArea: "",
                Ficha1: "",
                NumAprendices: "",
                Estado: "",
                TipoEscolaridad: ""
            };


            //$scope.ReporteHorasInstructor = function () {
            //    $scope.InstructorReporte = $scope.datalists.filter(function (item) {
            //        return item.Seleccionado === true;

            //    });

            //    if ($scope.InstructorModificar.length == 0 || $scope.InstructorModificar.length > 1) {
            //        bootbox.dialog({
            //            title: "Información",
            //            message: "Debe seleccionar un instructor",
            //            buttons: {
            //                success: {
            //                    label: "Cerrar",
            //                    className: "btn-primary",
            //                }
            //            }
            //        });

            //    } else {
            //        $("#ModalReporteHoras").modal("show");
            //    }
            //};
            InstructorService.ConsultarAmbientes(function (response) {
                if (response.success == true) {
                    $scope.Ambiente = response.datos
                }
            });

            InstructorService.ConsultarResultados(function (response) {
                if (response.success == true) {
                    $scope.Resultado = response.datos;
                }
            });

            InstructorService.ConsultarFichas(function (response) {

                if (response.success == true) {
                    $scope.Ficha = response.datos;
                }

            });


            //InstructorService.ConsultarInstructores(function (response) {
            //    if (response.success == true) {
            //        $scope.Instructor = response.datos;
            //    }
            //});

            $scope.ModalHorasInstructor = function () {

                $("#ModalHorasInstructores").modal("show");
            };

            $scope.HorasInstructor = function () {
                
              
                var x = $("#FechaInicio4").val().split('/');
                var y = $("#FechaFin4").val().split('/');

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
              
                InstructorService.RepoorteHorasInstructores($scope.Fechas,$rootScope.globals.currentUser.idpersona, function (response) {
                    if (response.success) {
                        $scope.HorasInstructor = [];

                        $.each(response.ListaIntructores, function (index, value) {

                            $scope.HorasInstructor.push({
                                Area:value.NombreArea, Instructor: value.Nombre, Cedula: value.Cedula, Email: value.Email,
                                Tipo_Instructor: value.NombreTipoInstructor, Tipo_Contrato: value.NombreTipoContrato, Telefono: value.Telefono, Rango_Horas:value.HorasRango, Total_horas: value.Total
                            });
                        });
                        
                        alasql('SELECT * INTO XLSX("ProgramacionInstructor.xlsx",{headers:true}) FROM ?', [$scope.HorasInstructor]);
                    }
                })

            }


            $scope.ModalReporteInstructor = function () {
                $scope.InstructorModificar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;

                });

                if ($scope.InstructorModificar.length == 0 || $scope.InstructorModificar.length > 1) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe seleccionar un instructor",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });

                } else {
                    $("#ModalReporteHoras").modal("show");
                }
            };

            $scope.ModalEnviarProgramacion = function () {
                $scope.InstructorEnviar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;

                });
                if ($scope.InstructorEnviar.length == 0 || $scope.InstructorEnviar.length > 1) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe seleccionar un instructor",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });

                } else {
                    $("#ModalEnviarProgramacion").modal("show");
                }
            };

            $scope.EnviarProgramacion = function () {
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
                var FechaInicio = $("#FechaInicio").val();
                var FechaFin = $("#FechaFin").val();
                InstructorService.EnviarProgramacionInstructor(FechaInicio, FechaFin, $scope.InstructorEnviar, function (response) {
                    if (response.success == true) {
                        $scope.Programacion = response.programacion;
                        $scope.ProgramacionInstructor = [];
                        $.each($scope.Programacion, function (index, value) {

                            $scope.ProgramacionInstructor.push({
                                Ficha: value.IdFicha, Ambiente: value.IdAmbiente, Instructor: value.IdInstructor, Resultado: value.IdResultado, FechaInicio: value.FechaInicio,
                                FechaFin: value.FechaFin, HoraInicio: value.HoraInicio, HoraFin: value.HoraFin
                            });
                        });

                        alasql('SELECT * INTO XLSX("ProgramacionInstructor.xlsx",{headers:true}) FROM ?', [$scope.ProgramacionInstructor]);
                    }
                });
            };

            $scope.ReporteProgramacionInstructor = function () {


                $scope.ProgramacionInstructor = [];

                var x = $("#FechaInicio3").val().split('/');
                var y = $("#FechaFin3").val().split('/');

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

                InstructorService.ReporteDeProgramacionInstructores($scope.InstructorModificar[0].IdInstructor, $scope.Fechas, function (response) {
                    if (response.success == true) {
                        $scope.Programacion = response.datos;
                        $scope.InstructorHoras = response.Horas;

                        InstructorService.ConsultarInstructores(function (response) {
                            if (response.success == true) {
                                $scope.Instructor = response.datos;

                            }
                        });

                        $.each($scope.Programacion, function (index, value1) {

                            value1.FechaInicio = value1.FechaInicio.toString().substring(0, 10);
                            value1.FechaFin = value1.FechaFin.toString().substring(0, 10);
                        });


                        $scope.Total_Horas = 0;
                        $scope.Total_HorasRango = 0;
                        $.each($scope.Programacion, function (index, value) {
                            var Dias = "";

                            if ($scope.Programacion[index].Lunes == true) {

                                Dias += "Lunes-"
                            }

                            if ($scope.Programacion[index].Martes == true) {

                                Dias += "Martes-"
                            }
                            if ($scope.Programacion[index].Miercoles == true) {

                                Dias += "Miércoles-"
                            }
                            if ($scope.Programacion[index].Jueves == true) {

                                Dias += "Jueves-"
                            }
                            if ($scope.Programacion[index].Viernes == true) {

                                Dias += "Viernes-"
                            }

                            if ($scope.Programacion[index].NumeroJornada == 2) {

                                Dias += "Sábado"
                            }

                            if ($scope.Programacion[index].NumeroJornada == 3) {

                                Dias += "Domingos"
                            }


                            $scope.ProgramacionInstructor.push({
                                Ficha: value.Ficha, Ambiente: value.Ambiente, Instructor: value.NombreInstructor, Resultado: value.Resultado, FechaInicio: value.FechaInicio,
                                FechaFin: value.FechaFin, HoraInicio: value.HoraInicio, HoraFin: value.HoraFin, Dias: Dias, Total_Horas_Rango: $scope.InstructorHoras[index].HorasRango,
                                Total_Horas_Programadas: $scope.InstructorHoras[index].Total
                            });


                            $scope.Total_Horas = $scope.Total_Horas + $scope.InstructorHoras[index].Total
                            $scope.Total_HorasRango = $scope.Total_HorasRango + $scope.InstructorHoras[index].HorasRango
                        });

                        $scope.ProgramacionInstructor.push({
                            Ficha: "", Ambiente: "", Instructor: "", Resultado: "", FechaInicio: "",
                            FechaFin: "", HoraInicio: "", HoraFin: "", Total_Horas_Rango: $scope.Total_HorasRango, Total_Horas_Programadas: $scope.Total_Horas
                        });

                        $scope.ProgramacionInstructor.push({
                            Ficha: "Rango Fechas", Ambiente: $scope.Fechas.FechaInicio, Instructor: $scope.Fechas.FechaFin, Resultado: "", FechaInicio: "",
                            FechaFin: "", HoraInicio: "", HoraFin: "", Total_Horas_Rango: "", Total_Horas_Programadas: ""
                        });

                        alasql('SELECT * INTO XLSX("REPORTE INSTRUCTOR ' + $scope.ProgramacionInstructor[0].Instructor + '.xlsx",{headers:true}) FROM ?', [$scope.ProgramacionInstructor]);
                    }

                });
            }

        }]);