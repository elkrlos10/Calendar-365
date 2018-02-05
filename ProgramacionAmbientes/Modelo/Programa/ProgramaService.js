ProgramacionApp.factory('ProgramaService',
    ['$http', '$rootScope', '$routeParams',
    function ($http, $rootScope, $routeParams) {
        var service = {};


        service.SubirArchivo = function (data, callback) {
            //waitingDialog.show();
            var ajaxRequest = $.ajax({
                type: "POST",
                url: URLServices + "File/UploadFilePrograma",
                contentType: false,
                processData: false,
                data: data
            }).done(function (responseData, textStatus) {
                callback(responseData);
                //waitingDialog.hide();
            }).fail(function () {
                //waitingDialog.hide();
            });
        };


        service.GuardarPrograma = function (Programa, callback) {
            $http.post(URLServices + "Programa/GuardarPrograma/", Programa)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarProgramas = function (callback) {
            
            $http.get(URLServices + "Programa/ConsultarProgramas/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ProgramaxCoordinacion = function (IdCoordinador, callback) {
            Item = {
                Parametro1: IdCoordinador
            };

            $http.post(URLServices + "Programa/ProgramaxCoordinacion/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ModificarPrograma = function (Programa, callback) {
            
            Item = {
                Parametro1: Programa[0].Parametro1
            };

            $http.post(URLServices + "Programa/ModificarPrograma/", Item)
              .success(function (response) {
                  callback(response);
              })
        };


        service.GuardarModificacionPrograma = function (Programa, callback) {
            $http.post(URLServices + "Programa/GuardarModificacionPrograma", Programa)
                .success(function (response) {
                    callback(response);
                });
        };



        service.BorrarPrograma = function (Programa, callback) {
            
            var Item = {
                Parametros: []
            }
            $.each(Programa, function (index, value) {
                Item.Parametros.push(
                    {
                        Parametro1: value.Parametro1
                    })
            });

            $http.post(URLServices + "Programa/EliminarPrograma/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarAreas = function (callback) {
            
            $http.get(URLServices + "Programa/ConsultarAreas/")
                .success(function (response) {
                    callback(response);
                });
        };
       
        service.ReporteProgramas = function (coordinador, callback) {
            var Item = {
                Parametro1: coordinador
            }
            $http.post(URLServices + "Programa/ReporteProgramas/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        return service;

    }]);