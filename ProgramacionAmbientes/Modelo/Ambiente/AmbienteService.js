ProgramacionApp.factory('AmbienteService',
    ['$http', '$rootScope', '$routeParams',
    function ($http, $rootScope, $routeParams) {
        var service = {};

        service.SubirArchivo = function (data, callback) {
            waitingDialog.show();
            var ajaxRequest = $.ajax({
                type: "POST",
                url: URLServices + "File/UploadFileAmbiente",
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


        service.ConsultarListas = function (callback) {
            $http.post(URLServices + "Core/ConsultarListas/", null)
              .success(function (response) {
                  callback(response);
              });
        };

        service.ConsultarAmbiente = function (callback) {
            debugger;
            $http.post(URLServices + "Ambiente/ConsultarAmbiente/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.AmbientesxArea = function (IdCoordinador, callback) {
            var Item = {
                Parametro1: IdCoordinador
            };

            $http.post(URLServices + "Ambiente/AmbientesxArea/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.OrganizarPisoAsc = function (callback) {
            $http.get(URLServices + "Ambiente/OrganizarPisoAsc/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.OrganizarPisoDesc = function (callback) {
            $http.get(URLServices + "Ambiente/OrganizarPisoDesc/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.OrganizarNumeroAsc = function (callback) {
            $http.get(URLServices + "Ambiente/OrganizarNumeroAsc/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.OrganizarNumeroDesc = function (callback) {
            $http.get(URLServices + "Ambiente/OrganizarNumeroDesc/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.BorrarAmbientes = function (Ambiente, callback) {
            var Item = {
                Parametros: []
            };

            $.each(Ambiente, function (index, value) {
                Item.Parametros.push(
                    {
                        Parametro1: value.Parametro1
                    })
            });
            $http.post(URLServices + "Ambiente/EliminarAmbiente/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.GuardarAmbiente = function (Ambiente, callback) {
            $http.post(URLServices + "Ambiente/GuardarAmbiente/", Ambiente)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ModificarAmbiente = function (Ambiente, callback) {
            $http.post(URLServices + "Ambiente/ModificarAmbiente/", Ambiente[0])
              .success(function (response) {
                  callback(response);
              })
        };

        service.GuardarModificacionAmbiente = function (Ambiente, callback) {
            $http.post(URLServices + "Ambiente/GuardarModificacionAmbiente", Ambiente)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarAreas = function (callback) {
            
            $http.get(URLServices + "Ambiente/ConsultarAreas/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarSedes = function (callback) {
            $http.get(URLServices + "Ambiente/ConsultarSedes")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarSedesColegios = function (callback) {
            $http.post(URLServices + "Ambiente/ConsultarSedesColegios")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarAmbientesColegios = function (callback) {
            $http.post(URLServices + "Ambiente/ConsultarAmbientesColegios")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarAmbienteSinColegios = function (callback) {
            $http.post(URLServices + "Ambiente/ConsultarAmbienteSinColegios")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ambientesDisponibles = function (Fechas,Idcoordinador, Dias ,callback) {
            
            var Item = {
                Parametro1: Fechas.FechaInicio,
                Parametro2: Fechas.FechaFin,
                Parametro3: Fechas.HoraInicio,
                Parametro4: Fechas.HoraFin,
                Parametro5: Fechas.opc,
                Parametro6: Idcoordinador,
                Parametro7: Dias.Lunes,
                Parametro8: Dias.Martes,
                Parametro9: Dias.Miercoles,
                Parametro10: Dias.Jueves,
                Parametro11: Dias.Viernes,



            };
          
            $http.post(URLServices + "Ambiente/ambientesDisponibles/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        return service;

    }]);