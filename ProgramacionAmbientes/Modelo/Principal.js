// script.js

// create the module and name it scotchApp
var ProgramacionApp = angular.module('ProgramacionApp', ['ngRoute', 'ngCookies', 'ngNotify']);



// configure our routes
ProgramacionApp.config(function ($routeProvider) {
    $routeProvider

    // route for the home page

          .when('/Sede', {
              templateUrl: 'Views/SedeView.html',
              controller: 'SedeController'
          })

        .when('/Area', {
            templateUrl: 'Views/AreaView.html',
            controller: 'AreaController'
        })
        .when('/Instructor', {
            templateUrl: 'Views/InstructorView.html',
            controller: 'InstructorController'
        })
        .when('/Ficha', {
            templateUrl: 'Views/FichaView.html',
            controller: 'FichaController'
        })
        .when('/Competencia', {
            templateUrl: 'Views/CompetenciaView.html',
            controller: 'CompetenciaController'
        })
        .when('/Ambiente', {
            templateUrl: 'Views/AmbienteView.html',
            controller: 'AmbienteController'
        })
        .when('/Programacion', {
            templateUrl: 'Views/ProgramacionView.html',
            controller: 'ProgramacionController'
        })
        .when('/Coordinacion', {
            templateUrl: 'Views/CoordinacionView.html',
            controller: 'CoordinacionController'
        })
       .when('/Programa', {
           templateUrl: 'Views/ProgramaView.html',
           controller: 'ProgramaController'
       })
       .when('/Resultado', {
           templateUrl: 'Views/ResultadoView.html',
           controller: 'ResultadoController'
       })
       .when('/Login', {
           templateUrl: 'Views/LoginView.html',
           controller: 'LoginController'
       })

      .when('/Solicitud', {
          templateUrl: 'Views/SolicitudView.html',
          controller: 'SolicitudController'
      })

})
.run(['$rootScope', '$location', '$cookies', '$cookieStore', '$http', '$templateCache',
    function ($rootScope, $location, $cookies, $cookieStore, $http, $templateCache) {
        $rootScope.$on('$locationChangeStart', function (event, next, current) {
            $rootScope.globals = $cookieStore.get('username');

            //Variable global para asignar el Id del programa transversal
            $rootScope.ProgramaTransversal = 1414;
            //----------------------------------------------------------


            if ($location.path() == "/Solicitud" && !$rootScope.globals) {
                var url = $location.search();
                if (url != "") {
                    $cookies.put("solicitud", url.GUID);
                    $location.url("/Login");
                    return;
                }
            }

            if ($rootScope.globals != undefined) {
                if ($location.path() !== '/Login' && !$rootScope.globals) {
                    if ($location.path() == "/Solicitud") {
                        var url = $location.search();
                        if (url != "") {
                            $cookies.put("solicitud", url.GUID);
                            $location.url("/Login");
                            return;
                        }
                    }
                    $cookies.put("solicitud", undefined);
                } else {
                    $("#BodyPrincipal").css("display", "block");
                    $("#username").text($rootScope.globals.currentUser.nombre + " " + $rootScope.globals.currentUser.apellido);
                    if ($rootScope.globals.currentUser.tipousuario == 4) {
                        $(".items-menu-principal").css("display", "none");
                        if ($location.path() == "/Sede" || $location.path() == "/Area" || $location.path() == "/Instructor"
                            || $location.path() == "/Ficha" || $location.path() == "/Competencia" || $location.path() == "/Ambiente"
                            || $location.path() == "/Programa" || $location.path() == "/Resultado") {
                            $location.path('/Programacion');
                        }
                    }
                    if ($rootScope.globals.currentUser.tipousuario == 3) {
                        $(".items-menu-principal").css("display", "none");
                        if ($location.path() == "/Sede" || $location.path() == "/Area" || $location.path() == "/Instructor"
                            || $location.path() == "/Ficha" || $location.path() == "/Competencia" || $location.path() == "/Ambiente"
                            || $location.path() == "/Programa" || $location.path() == "/Resultado") {
                            $location.path('/Programacion');
                        }
                    }
                    if ($rootScope.globals.currentUser.tipousuario == 2) {
                        $(".not-coor").css("display", "none");
                        if ($location.path() == "/Sede" || $location.path() == "/Area"
                            || $location.path() == "/Competencia"
                            || $location.path() == "/Resultado") {
                            $location.path('/Programacion');
                        }
                    }
                    if ($rootScope.globals.currentUser.tipousuario == 1) {
                        $(".not-admin").css("display", "none");
                        //if ($location.path() == "/Programacion") {
                        //    $location.path('/Coordinacion');
                        //}
                    }
                }
            }

            if ($location.path() !== '/Login' && !$rootScope.globals) {
                $location.path('/Login');
            }
        });
    }]);


// create the controller and inject Angular's $scope
ProgramacionApp.controller('PrincipalController',
    ['$scope', '$rootScope', '$cookies', '$cookieStore', 'InstructorService', 'ProgramacionService', 'LoginService', '$http', '$location',
    function ($scope, $rootScope, $cookies, $cookieStore, InstructorService, ProgramacionService, LoginService, $http, $location) {
        $scope.CerrarSesion = function () {
            $cookies.remove("username");
            $location.url('/Login');
        };

        $scope.AbrirCoodinador = function () {
            $location.url("/Coordinacion");
        };
        $scope.AbrirSede = function () {
            $location.url("/Sede");
        };
        $scope.AbrirArea = function () {
            $location.url("/Area");
        };

        $scope.AbrirAmbiente = function () {
            $location.url("/Ambiente");
        };
        $scope.AbrirPrograma = function () {
            $location.url("/Programa");
        };
        $scope.AbrirCompetencia = function () {
            $location.url("/Competencia");
        };
        $scope.AbrirResultado = function () {
            $location.url("/Resultado");
        };
        $scope.AbrirFicha = function () {
            $location.url("/Ficha");
        };
        $scope.AbrirInstructor = function () {
            $location.url("/Instructor");
        };
        $scope.AbrirProgramacion = function () {
            $location.url("/Programacion");
        };

        //setInterval(function () {
        //    InstructorService.EnviarCorreoInstructor(function (response) {
        //        //if (response.success == true) {
        //        var a = 0;
        //        console.log(a+=1);
        //        //}
        //    });
        //}, 120000);

        setInterval(function () {
            ProgramacionService.RegistrarProgramacionesPrestamoLLaves(function (response) {
                if (response.success == true) {
              
               }
            });
        }, 2400000);

        //2400000 milisegundos

        $scope.UsuarioCambiarPass = {
            Password: "",
            newPass: ""
        };

        $scope.AbrirModalCambiarPass = function () {
            $("#ModalCambiarPass").modal("show");
        };

        $scope.CambiarPass = function () {
            LoginService.CambiarPassword($scope.UsuarioCambiarPass, $rootScope.globals.currentUser.id, function (response) {
                if (response.success = true) {

                    bootbox.dialog({
                        title: "Información",
                        message: "El cambio de contraseña se realizó con éxito",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                }
            })
        };
    }]);


