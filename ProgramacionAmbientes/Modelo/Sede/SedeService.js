ProgramacionApp.factory('SedeService',
    ['$http', '$rootScope', '$routeParams',
    function ($http, $rootScope, $routeParams) {
        var service = {};



        service.SubirArchivo = function (data, callback) {
            waitingDialog.show();
            var ajaxRequest = $.ajax({
                type: "POST",
                url: URLServices + "File/UploadFileSede",
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

        service.GuardarSede = function (Sede, callback) {
            $http.post(URLServices + "Sede/GuardarSede/", Sede)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarSedes = function (callback) {            
            $http.get(URLServices + "Sede/ConsultarSedes/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarColegios = function (callback) {
            $http.post(URLServices + "Sede/ConsultarColegios/")
                .success(function (response) {
                    callback(response);
                });
        };


        service.ModificarSede = function (Sede, callback) {
            
            Item = {
                Parametro1: Sede[0].Parametro1
            };

            $http.post(URLServices + "Sede/ModificarSede/", Item)
              .success(function (response) {
                  callback(response);
              })
        };


        service.GuardarModificacionSede = function (Sede, callback) {
            $http.post(URLServices + "Sede/GuardarModificacionSede", Sede)
                .success(function (response) {
                    callback(response);
                });
        };



        service.BorrarSede = function (Sede, callback) {
            
            var Item = {
                Parametros: []
            }
            $.each(Sede, function (index, value) {
                Item.Parametros.push(
                    {
                        Parametro1: value.Parametro1
                    })
            });

            $http.post(URLServices + "Sede/EliminarSede/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

    
        service.ConsultarDepartamentos = function (callback) {
            
            $http.post(URLServices + "Sede/ConsultarDepartamentos/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.consultarDepartamentoxMunicipio = function (IdMunicipio, callback) {

            Item = {
                Parametro1: IdMunicipio.toString(),
            
            };
            $http.post(URLServices + "Sede/consultarDepartamentoxMunicipio/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarMunicipios = function (IdDepartamento, callback) {
            
            Item = {
                Parametro1: IdDepartamento
            };
            $http.post(URLServices + "Sede/ConsultarMunicipios/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.CodigoSede = function (callback) {
            debugger;
            $http.post(URLServices + "Sede/CodigoSede/")
                .success(function (response) {
                    callback(response);
                });
        };

        return service;

    }]);