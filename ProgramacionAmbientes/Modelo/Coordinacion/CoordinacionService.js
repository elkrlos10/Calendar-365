ProgramacionApp.factory('CoordinacionService',
    ['$http', '$rootScope', '$routeParams',
    function ($http, $rootScope, $routeParams) {
        var service = {};

        service.GuardarCoordinacion = function (Coordinacion, callback) {
            $http.post(URLServices + "Coordinacion/GuardarCoordinacion/", Coordinacion)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarCoordinacion = function (callback) {
            $http.get(URLServices + "Coordinacion/ConsultarCoordinacion/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ModificarCoordinacion = function (Coordinacion, callback) {
            $http.post(URLServices + "Coordinacion/ModificarCoordinacion/", Coordinacion[0])
              .success(function (response) {
                  callback(response);
              })
        };

        service.ConsultarAreas = function (callback) {
            $http.get(URLServices + "Ambiente/ConsultarAreas/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.GuardarModificacionCoordinacion = function (Coordinacion, callback) {
            $http.post(URLServices + "Coordinacion/GuardarModificacionCoordinacion", Coordinacion)
                .success(function (response) {
                    callback(response);
                })
        };

        service.BorrarCoordinaciones = function (Coordinaciones, callback) {
            var Item = {
                Parametros: []
            };

            $.each(Coordinaciones, function (index, value) {
                Item.Parametros.push(
                    {
                        Parametro1: value.IdCoordinacion
                    })
            });
            $http.post(URLServices + "Coordinacion/EliminarCoordinacion/", Item)
                .success(function (response) {
                    callback(response);
                });
        };


        return service;

    }]);