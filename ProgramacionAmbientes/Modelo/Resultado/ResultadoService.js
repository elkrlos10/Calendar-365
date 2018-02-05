ProgramacionApp.factory('ResultadoService',
    ['$http', '$rootScope', '$routeParams',
    function ($http, $rootScope, $routeParams) {
        var service = {};

        service.SubirArchivo = function (data, callback) {
            waitingDialog.show();
            var ajaxRequest = $.ajax({
                type: "POST",
                url: URLServices + "File/UploadFileResultado",
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

        service.ConsultarCompetencias = function (callback) {
            
            $http.get(URLServices + "Resultado/ConsultarCompetencias/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.GuardarResultado = function (Resultado, callback) {
            $http.post(URLServices + "Resultado/GuardarResultado/", Resultado)
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

        service.ModificarResultado = function (Resultado, callback) {
            $http.post(URLServices + "Resultado/ModificarResultado/", Resultado[0])
              .success(function (response) {
                  callback(response);
              })
        };

        service.GuardarModificacionResultado = function (Resultado, callback) {
            $http.post(URLServices + "Resultado/GuardarModificacionResultado", Resultado)
                .success(function (response) {
                    callback(response);
                });
        };

        service.BorrarResultados = function (Resultado, callback) {
            var Item = {
                Parametros: []
            };

            $.each(Resultado, function (index, value) {
                Item.Parametros.push(
                    {
                        Parametro1: value.Parametro1
                    })
            });
            $http.post(URLServices + "Resultado/EliminarResultado/", Item)
                .success(function (response) {
                    callback(response);
                });
        };


        return service;

    }]);