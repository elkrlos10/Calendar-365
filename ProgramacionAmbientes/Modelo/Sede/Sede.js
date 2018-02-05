ProgramacionApp.controller('SedeController',
    ['$scope', '$rootScope', '$location', 'SedeService', '$routeParams', '$sce',
        function ($scope, $rootScope, $location, SedeService, $routeParams, $sce) {

            var nav = $("#navbar").hasClass("gn-menu-wrapper gn-open-all");
            if (nav == true) {
                $(".Principal").css("width", "80%");

            } else {
                $(".Principal").css("width", "95%");
            }
         
            $("#atras").hide();

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
                        SedeService.SubirArchivo(dataweb, function (response) {
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
                                SedeService.ConsultarSedes(function (response) {
                                    if (response.success == true) {
                                        $scope.datalists = response.datos;
                                        $scope.ListaCompleta = response.datos;
                                    }
                                });
                                $("#atras").hide();
                                $("#fileUploadWeb").replaceWith($("#fileUploadWeb").val('').clone(true));
                                document.getElementById("fileUploadWeb").value = "";
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



            $scope.Sede = {
                IdSede: "",
                Nombre_Sede: "",
                Direccion: "",
                IdMunicipio: "",
                Codigo: "",
                TipoSede: ""
            };


            $scope.Departamento = {
                IdDepartamento: "",
                NombrDepartamento: ""
               
            };
     
            $scope.Municipio = {
                IdMunicipo: "",
                NombrDepartamento: "",
                IdDepartamento: ""
            };

      

            $scope.AbrirModal = function () {
               // $scope.VaciarCampos();
                $("#ModalSede").modal("show");
               
                SedeService.CodigoSede(function (response) {
                    if (response.success == true) {
                       
                        $scope.Sede.Codigo = response.datos.Codigo + 1;
                    }
                })
             
            };

            SedeService.ConsultarDepartamentos(function (response) {

                if (response.success == true) {

                    $scope.Departamento = response.datos;
                }
            });

            $scope.ConsultarMunicipiosDepartamento = function (IdDepartamento) {
                
                $.each($scope.Departamento, function (index, value) {
                    if (value.IdDepartamento == IdDepartamento) {

                       
                        SedeService.ConsultarMunicipios(value.IdDepartamento, function (response) {

                            if (response.success == true) {
                                $scope.Municipio = response.datos;
                            }
                        });
                    }

                });

                
             
            };

            $scope.colegio = function () {

                if ($scope.Sede.Colegio == true) {
                    $scope.Sede.Empresa = false;
                }
            }

            $scope.empresa = function () {

                if ($scope.Sede.Empresa == true) {
                    $scope.Sede.Colegio = false;
                }
            }

            //Función para guardar un sede
            $scope.Guardar = function () {
                
                $.each($scope.Municipio, function (index, value) {
                    if (value.IdMunicipio == $scope.Municipio.IdMunicipio) {

                        $scope.Sede.IdMunicipio = value.IdMunicipio;
                      
                    }

                });

              
                if ($scope.Sede.Nombre_Sede != "" || $scope.Sede.Direccion != "" || $scope.Sede.IdMunicipio != "" || $scope.Sede.Codigo != "" || $scope.TipoSede != "") {

                    SedeService.GuardarSede($scope.Sede, function (response) {
                        if (response.success == true) {
                            $("#ModalSede").modal("hide");
                            $scope.VaciarCampos();

                            SedeService.ConsultarSedes(function (response) {
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
                                message: "La sede ya se encuentra resgistrada",
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


            $scope.MostrarColegios = function () {
                SedeService.ConsultarColegios(function (response) {
                    if (response.success == true) {
                        $scope.datalists = response.datos;
                        $scope.ListaCompleta = response.datos;
                        $("#atras").show();

                    }
                });
            }

            $scope.atras = function () {
                SedeService.ConsultarSedes(function (response) {
                    if (response.success == true) {
                        $scope.datalists = response.datos;
                        $scope.ListaCompleta = response.datos;
                        $("#atras").hide();

                    }
                });

            }

            //Función para vaciar campos
            $scope.VaciarCampos = function () {
                $scope.Sede.Nombre_Sede = "";
                $scope.Sede.Direccion = "";
                $scope.Sede.IdMunicipio = "";
                //$scope.departamento.IdDepartamento = "";
                $scope.Municipio.IdMunicipio = "";
                $scope.departamento = "";
              
            }

            //Función para consultar las areas
            SedeService.ConsultarSedes(function (response) {
                
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


      
            //Función 1 para el borrado de las areas
            $scope.CambiarEstadoSeleccionados = function () {
                var UsariosBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });

                if (UsariosBorrar.length == 0) {

                    bootbox.dialog({
                        title: "Eliminar",
                        message: "Seleccione por lo menos una sede",
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
                
                var SedeBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });
                SedeService.BorrarSede(SedeBorrar, function (response) {

                    if (response.success == true && response.respuesta == true) {
                        SedeService.ConsultarSedes(function (response) {
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
                
                var SedeModificar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;

                });


                if (SedeModificar.length == 1) {

                    SedeService.ModificarSede(SedeModificar, function (response) {

                        if (response.success == true) {

                            $scope.Sede.IdSede = response.sede.IdSede;
                            $scope.Sede.Nombre_Sede = response.sede.Nombre_Sede;
                            $scope.Sede.Direccion = response.sede.Direccion;
                            $scope.Sede.IdMunicipio = response.sede.IdMunicipio;
                            $scope.Sede.Codigo = response.sede.Codigo;
                            $scope.Sede.TipoSede = response.sede.TipoSede;
                            $("#TipoSede > option[value='" + $scope.Sede.TipoSede + "']").attr('selected', 'selected');

                            SedeService.consultarDepartamentoxMunicipio($scope.Sede.IdMunicipio, function (response) {

                                if (response.success == true) {
                                    $("#departamento > option[value='" + response.datos.IdDepartamento + "']").attr('selected', 'selected');
                                    
                                    


                                    SedeService.ConsultarMunicipios(response.datos.IdDepartamento, function (response) {

                                        if (response.success == true) {

                                            $scope.Municipio = response.datos;

                                            setTimeout(function () {
                                                $("#municipio > option[value='" + $scope.Sede.IdMunicipio + "']").attr('selected', 'selected');
                                            }, 1000)
                                        }
                                    });
                                }
                            });


                            $("#ModalEditar").modal("show");
                        }
                    });
                } else {

                    bootbox.dialog({
                        title: "Editar",
                        message: "Debe seleccionar una sede",
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
            $scope.GuardarEdicionSede = function () {
                $.each($scope.Municipio, function (index, value) {
                    if (value.IdMunicipio == $scope.Municipio.IdMunicipio) {

                        $scope.Sede.IdMunicipio = value.IdMunicipio;

                    }

                });

                if ($scope.Sede.Nombre_Sede != "" || $scope.Sede.Direccion != "" || $scope.Sede.IdMunicipio != "" || $scope.Sede.TipoSede != "") {
                    SedeService.GuardarModificacionSede($scope.Sede, function (response) {
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
                           // $scope.VaciarCampos();
                            SedeService.ConsultarSedes(function (response) {
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
                    SedeService.ConsultarSedes(function (response) {
                        if (response.success == true) {
                            $scope.datalists = response.datos;
                            $scope.ListaCompleta = response.datos;
                        }
                    });
                }
                var Sede = [];
                $scope.datalists = $scope.ListaCompleta;
                Sede = $scope.datalists.filter(function (item) {

                    //if (exp.test(item.Parametro3.toLowerCase()) || exp.test(item.Parametro4.toLowerCase()) || exp.test(item.Parametro2.toLowerCase())) {

                    //    return item;
                    //}
                    if (exp.test(item.Parametro3.toLowerCase()) || exp.test(item.Parametro3.toUpperCase())) {

                        return item;
                    }

                    else if (exp.test(item.Parametro4.toLowerCase()) || exp.test(item.Parametro4.toUpperCase())) {
                        return item;
                    }

                    else if (exp.test(item.Parametro2.toLowerCase()) || exp.test(item.Parametro2.toUpperCase())) {
                        return item;
                    }

                });
                $scope.datalists = Sede;
                //Variable para setear la paginación 
                $scope.curPage = 0;
            };

        }]);