ProgramacionApp.factory('FichaService',
    ['$http', '$rootScope', '$routeParams',
    function ($http, $rootScope, $routeParams) {
        var service = {};

        service.SubirArchivo = function (data, callback) {
            waitingDialog.show();
            var ajaxRequest = $.ajax({
                type: "POST",
                url: URLServices + "File/UploadFileFicha",
                contentType: false,
                processData: false,
                data: data
            }).done(function (responseData, textStatus) {
                callback(responseData);
                waitingDialog.hide();
            }).fail(function () {
                waitingDialog.hide();
            });
        };

        service.GuardarFicha = function (Ficha, callback) {
            $http.post(URLServices + "Ficha/GuardarFicha", Ficha)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarFichas = function (callback) {
            $http.get(URLServices + "Ficha/ConsultarFichas/")
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

        service.ConsultarFichasxArea = function (IdCoordinador, callback) {
            
            item = {
                Parametro1: IdCoordinador
            };

            $http.post(URLServices + "Ficha/ConsultarFichasxArea/", item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarProgramaxArea = function (IdArea, callback) {

            item = {
                Parametro1: IdArea
            };

            $http.post(URLServices + "Programa/ConsultarProgramaxArea/", item)
                .success(function (response) {
                    callback(response);
                });
        };

        
        service.ConsultarFichasInactivas = function (callback) {
            $http.get(URLServices + "Ficha/ConsultarFichasInactivas/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.BorrarFicha = function (Ficha, callback) {
            var Item = {
                Parametros: []
            };

            $.each(Ficha, function (index, value) {
                Item.Parametros.push(
                    {
                        Parametro1: value.Parametro1
                    })
            });
            //EliminarFicha
            $http.post(URLServices + "Ficha/InHabilitarFicha/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ModificarFicha = function (Ficha, callback) {
            $http.post(URLServices + "Ficha/ModificarFicha/", Ficha[0])
              .success(function (response) {
                  callback(response);
              })
        };

        service.GuardarModificacionFicha = function (Ficha, callback) {
            $http.post(URLServices + "Ficha/GuardarModificacionFicha", Ficha)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarProgramas = function (callback) {
            
            $http.get(URLServices + "Ficha/ConsultarProgramas/")
                .success(function (response) {
                    callback(response);
                });
        };

        

        service.ConsultarAreaxPrograma = function (IdPrograma,callback) {
            var Item = {
                Parametro1: IdPrograma
            };
            $http.post(URLServices + "Ficha/ConsultarAreaxPrograma/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ReporteProgramacion = function (Fechas, IdFicha, callback) {
            
            item = {
                Parametro1: Fechas.FechaInicio,
                Parametro2: Fechas.FechaFin,
                Parametro3: IdFicha
            };

            $http.post(URLServices + "Ficha/ReporteProgramacion/", item)
                .success(function (response) {
                    callback(response);
                });
        };


        service.ConsultarResultados = function (callback) {
            
          
            $http.post(URLServices + "Resultado/ConsultarResultados/")
                .success(function (response) {
                    callback(response);
                });
        };
        service.ConsultarInstructores = function (callback) {
            

            $http.post(URLServices + "Instructor/ConsultarInstructores/")
                .success(function (response) {
                    callback(response);
                });
        };

        return service;

    }]);