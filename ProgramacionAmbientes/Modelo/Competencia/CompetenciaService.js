ProgramacionApp.factory('CompetenciaService',
    ['$http', '$rootScope', '$routeParams',
    function ($http, $rootScope, $routeParams) {
        var service = {};

        service.SubirArchivo = function (data, callback) {
            waitingDialog.show();
            var ajaxRequest = $.ajax({
                type: "POST",
                url: URLServices + "File/UploadFileCompetencia",
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

        service.GuardarCompetencia = function (Competencia, callback) {
            $http.post(URLServices + "Competencia/GuardarCompetencia/", Competencia)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarCompetencias = function (callback) {
            
            $http.get(URLServices + "Competencia/ConsultarCompetencia/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ModificarCompetencia = function (Competencia, callback) {
            $http.post(URLServices + "Competencia/ModificarCompetencia/", Competencia[0])
              .success(function (response) {
                  callback(response);
              })
        };


        service.GuardarModificacionCompetencia = function (Competencia, callback) {
            $http.post(URLServices + "Competencia/GuardarModificacionCompetencia", Competencia)
                .success(function (response) {
                    callback(response);
                });
        };



        service.BorrarCompetencia = function (Competencia, callback) {
            var Item = {
                Parametros: []
            };

            $.each(Competencia, function (index, value) {
                Item.Parametros.push(
                    {
                        Parametro1: value.Parametro1
                    })
            });
            $http.post(URLServices + "Competencia/EliminarCompetencia/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarProgramas = function (callback) {
            
            $http.get(URLServices + "Competencia/ConsultarProgramas/")
                .success(function (response) {
                    callback(response);
                });
        };

        return service;

    }]);