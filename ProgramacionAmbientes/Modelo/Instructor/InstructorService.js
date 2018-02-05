ProgramacionApp.factory('InstructorService',
    ['$http', '$rootScope', '$routeParams',
    function ($http, $rootScope, $routeParams) {
        var service = {};


        service.SubirArchivo = function (data, callback) {
            waitingDialog.show();
            console.log(data);
            var ajaxRequest = $.ajax({
                type: "POST",
                url: URLServices + "File/UploadFile",
                contentType: false,
                processData: false,
                data: data,
                async: true,
            }).done(function (responseData, textStatus) {
                callback(responseData);
                waitingDialog.hide();
            }).fail(function () {
                waitingDialog.hide();
                location.reload();
            });
        };

        service.ConsultarProgramas = function (callback) {
            $http.get(URLServices + "Instructor/ConsultarProgramas/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.GuardarInstructor = function (Instructor, callback) {
            waitingDialog.show();
            $http.post(URLServices + "Instructor/GuardarInstructor", Instructor)
                .success(function (response) {
                    callback(response);
                    waitingDialog.hide();
                });
        };

        service.ConsultarInstructores = function (callback) {
            $http.post(URLServices + "Instructor/ConsultarInstructores/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarInstructoresxArea = function (idArea, callback) {
            item = {
                parametro1: idArea
            };
            $http.post(URLServices + "Instructor/ConsultarInstructoresxArea/", item)
                .success(function (response) {
                    callback(response);
                });
        };


        service.CambiarEstado = function (Instructor, callback) {
            $http.post(URLServices + "Instructor/CambiarEstado/", Instructor)
              .success(function (response) {
                  callback(response);
              })
        };

        service.HabilitarInstructor = function (Instructor, callback) {
            $http.post(URLServices + "Instructor/HabilitarInstructor/", Instructor)
              .success(function (response) {
                  callback(response);
              })
        };

        service.ModificarInstructor = function (Instructor, callback) {
            $http.post(URLServices + "Instructor/ModificarInstructor/", Instructor[0])
              .success(function (response) {
                  callback(response);
              })
        };


        service.GuardarModificacionInstructor = function (Instructor, callback) {
            $http.post(URLServices + "Instructor/GuardarModificacionInstructor", Instructor)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarAreas = function (callback) {
            $http.get(URLServices + "Area/ConsultarAreas/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarInhabilitados = function (callback) {
            $http.post(URLServices + "Instructor/ConsultarInhabilitados")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarInhabilitadosxArea = function (IdArea, callback) {
            item = {
                parametro1: IdArea
            };
            $http.post(URLServices + "Instructor/ConsultarInhabilitadosxArea", item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ReporteDeProgramacionInstructores = function (idInstructor, fechas, callback) {
            
            item = {
                Parametro1: idInstructor,
                Parametro2: fechas.FechaInicio,
                Parametro3: fechas.FechaFin
            };
            $http.post(URLServices + "Instructor/ReporteDeProgramacionInstructores", item)
                .success(function (response) {
                    callback(response);
                });
        };


        service.ConsultarFichas = function (callback) {
     
            $http.post(URLServices + "Instructor/ConsultarFichas/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarAmbientes = function ( callback) {
           
            $http.post(URLServices + "Instructor/ConsultarAmbientes/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarResultados = function (callback) {
            
            $http.post(URLServices + "Instructor/ConsultarResultados/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.EnviarCorreoInstructor = function (callback) {
            $http.post(URLServices + "Instructor/EnviarCorreoInstructor/")
                .success(function (response) {
                    callback(response);
                });
        };        

        service.EnviarProgramacionInstructor = function (FechaInicio, FechaFin, Instructor, callback) {
            var Item = {
                Parametro1: FechaInicio,
                Parametro2: FechaFin,
                Parametro3: Instructor[0].IdInstructor
            };
            $http.post(URLServices + "Instructor/EnviarProgramacionInstructor/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.RepoorteHorasInstructores = function (Fechas, IdCoordinador, callback) {
            var Item = {
                Parametro1: Fechas.FechaInicio,
                Parametro2: Fechas.FechaFin,
                Parametro3: IdCoordinador
            };
            $http.post(URLServices + "Instructor/RepoorteHorasInstructores/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        return service;

    }]);