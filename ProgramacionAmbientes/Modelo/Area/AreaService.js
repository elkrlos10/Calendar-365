ProgramacionApp.factory('AreaService',
    ['$http', '$rootScope', '$routeParams',
    function ($http, $rootScope, $routeParams) {
        var service = {};

        service.SubirArchivo = function (data, callback) {
            waitingDialog.show();
            var ajaxRequest = $.ajax({
                type: "POST",
                url: URLServices + "File/UploadFileArea",
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


        service.GuardarArea = function (Area, callback) {
            $http.post(URLServices + "Area/GuardarArea/", Area)
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

        service.ModificarArea = function (Area, callback) {
            $http.post(URLServices + "Area/ModificarArea/", Area[0])
              .success(function (response) {
                  callback(response);
              })
        };


        service.GuardarModificacionArea = function (Area, callback) {
            $http.post(URLServices + "Area/GuardarModificacionArea", Area)
                .success(function (response) {
                    callback(response);
                });
        };



        service.BorrarArea = function (Area, callback) {
            var Item = {
                Parametros: []
            }
            $.each(Area, function (index, value) {
                Item.Parametros.push(
                    {
                        Parametro1: value.Parametro1
                    })
            });
            $http.post(URLServices + "Area/EliminarArea/", Item)
                .success(function (response) {
                    callback(response);
                });
        };
        
        return service;

    }]);