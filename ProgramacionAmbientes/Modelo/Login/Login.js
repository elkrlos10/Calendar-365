ProgramacionApp.controller('LoginController',
    ['$scope', '$rootScope', '$location', '$cookies', '$cookieStore', 'LoginService', '$routeParams', '$sce',
        function ($scope, $rootScope, $location, $cookies, $cookieStore, LoginService, $routeParams, $sce) {

            $("#BodyPrincipal").css("display", "none");

            $scope.Usuario = {
                NombreUsuario: "",
                Password: ""
            };

            $scope.UsuarioRecuperar = {
                Email: "",
                Cedula: ""
            };



            $scope.AbrirModalRecuperar = function () {
                $("#ModalRecuperar").modal("show");
            };



            $scope.IniciarSesion = function () {
                $(".items-menu-principal").css("display", "block");
                $(".not-coor").css("display", "block");
                $(".not-admin").css("display", "block");
                if ($scope.Usuario.NombreUsuario == Admin && $scope.Usuario.Password == Password) {
                    $rootScope.globals = {
                        currentUser: {
                            id: "0",
                            nombre: "Administrador",
                            apellido: " ",
                            cedula: " ",
                            tipousuario: 1,
                            idpersona: ""
                        }
                    };
                    $cookies.putObject("username", $rootScope.globals);
                    $location.url('/Programacion');
                    $(".not-admin").css("display", "none");
                    return;
                }
                LoginService.IniciarSesion($scope.Usuario, function (response) {
                    if (response.success == true) {
                        if (response.resp == 1) {
                            bootbox.dialog({
                                title: "Información",
                                message: "Usuario y/o contraseña incorrectos",
                                buttons: {
                                    success: {
                                        label: "Cerrar",
                                        className: "btn-primary",
                                    }
                                }
                            });
                            return;
                        }
                        $rootScope.globals = {
                            currentUser: {
                                id: response.usuario.IdUsuario,
                                nombre: response.usuario.Nombre,
                                apellido: response.usuario.Apellido,
                                cedula: response.usuario.Cedula,
                                tipousuario: response.usuario.TipoUsuario,
                                idpersona: response.usuario.IdPersona
                            }
                        };
                        $cookies.putObject("username", $rootScope.globals);
                        var solicitud = $cookieStore.get("solicitud");
                        if (solicitud != undefined) {
                            $location.url("/Solicitud?GUID=" + solicitud);
                            return;
                        }
                        $location.url('/Programacion');
                        $("#BodyPrincipal").css("display", "block");
                        $rootScope.globals = $cookieStore.get('username');
                        $("#username").text($rootScope.globals.currentUser.nombre + " " + $rootScope.globals.currentUser.apellido);
                        if ($rootScope.globals.currentUser.tipousuario == 4) {
                            $(".items-menu-principal").css("display", "none");
                        }
                        if ($rootScope.globals.currentUser.tipousuario == 3) {
                            $(".items-menu-principal").css("display", "none");
                        }
                        if ($rootScope.globals.currentUser.tipousuario == 2) {
                            $(".not-coor").css("display", "none");
                        }
                        if ($rootScope.globals.currentUser.tipousuario == 1) {
                            $(".not-admin").css("display", "none");
                        }
                    }
                });
            };

            $scope.Recuperar = function () {
                LoginService.RecuperarPassword($scope.UsuarioRecuperar, function (response) {
                    if (response.success == true) {
                        $("#ModalRecuperar").modal("hide");
                        bootbox.dialog({
                            title: "Información",
                            message: "Se ha enviado la información al correo " + response.Email,
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
                            message: "Los datos ingresados no coinciden",
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

        }]);