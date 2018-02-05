ProgramacionApp.factory('LoginService',
    ['$http', '$rootScope', '$routeParams',
    function ($http, $rootScope, $routeParams) {
        var service = {};

        service.IniciarSesion = function (Usuario, callback) {
            waitingDialog.show();
            $http.post(URLServices + "Usuario/IniciarSesion", Usuario)
                .success(function (response) {
                    callback(response);
                    waitingDialog.hide();
                });
        };

        service.RecuperarPassword = function (Usuario, callback) {
            Item = {
                Parametro1: Usuario.Email,
                Parametro2: Usuario.Cedula
            };
            $http.post(URLServices + "Usuario/RecuperarPassword", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.CambiarPassword = function (Usuario,id, callback) {
            Item = {
                Parametro1: Usuario.Password,
                Parametro2: Usuario.newPass,
                Parametro3: id
            };
            $http.post(URLServices + "Usuario/CambiarPassword", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        return service;

    }]);