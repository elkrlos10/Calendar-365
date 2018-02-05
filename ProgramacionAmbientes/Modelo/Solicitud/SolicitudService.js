ProgramacionApp.factory('SolicitudService',
    ['$http', '$rootScope', '$routeParams',
    function ($http, $rootScope, $routeParams) {
        var service = {};

        service.AprovarSolicitud = function (IdProgramacion, Estado, callback) {
            waitingDialog.show();
            Item = {
                Parametro1: IdProgramacion.toString(),
               Parametro2: Estado
            };
            $http.post(URLServices + "Programacion/AprobarSolicitud/", Item)
                .success(function (response) {
                    callback(response);
                    waitingDialog.hide();
                });
        };


        service.ConsultarAmbienteSolicitado = function (IdProgramacion, callback) {
          
            Item = {
                Parametro1: IdProgramacion.toString(),
           
            };
            $http.post(URLServices + "Programacion/ConsultarAmbienteSolicitado/", Item)
                .success(function (response) {
                    callback(response);
                   
                });
        };

        return service;

    }]);