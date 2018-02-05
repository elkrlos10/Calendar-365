ProgramacionApp.controller('SolicitudController',
    ['$scope', '$rootScope', '$location', '$cookies', '$cookieStore', 'SolicitudService', '$routeParams', '$sce',
        function ($scope, $rootScope, $location, $cookies, $cookieStore, SolicitudService, $routeParams, $sce) {


            var url = $location.search();


            $scope.AprovarSolicitud = function () {
                var Estado = true;
                SolicitudService.AprovarSolicitud(url.GUID, Estado, function (response) {
                    if (response.success == true) {

                        $cookies.remove("solicitud");
                        $location.url('/Programacion');
                    }

                })
            }


            $scope.CancelarSolicitud = function () {
                var Estado = false;
                SolicitudService.AprovarSolicitud(url.GUID, Estado, function (response) {
                    if (response.success == true) {

                        $cookies.remove("solicitud");
                        $location.url('/Programacion');
                    }

                })
            }

            SolicitudService.ConsultarAmbienteSolicitado(url.GUID, function (response) {
                if (response.success == true) {

                    
                    $scope.datalist = response.datos;
                    var fechaIni = response.datos.FechaInicio.toString().split("T");
                    var fechaFin = response.datos.FechaFin.toString().split("T");
                    $scope.datalist.FechaInicio = fechaIni[0];
                    $scope.datalist.FechaFin = fechaFin[0];
                }

            });

        }]);


