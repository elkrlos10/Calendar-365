ProgramacionApp.factory('ProgramacionService',
    ['$http', '$rootScope', '$routeParams',
    function ($http, $rootScope, $routeParams) {
        var service = {};

        service.ConsultarFichas = function (IdArea, callback) {
            var item = {
                Parametro1: IdArea
            };
            $http.post(URLServices + "Programacion/GetFichas/", item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarAmbientes = function (IdArea, callback) {
            var item = {
                Parametro1: IdArea
            };
            $http.post(URLServices + "Programacion/GetAmbientes/", item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarColegios = function (callback) {
            $http.post(URLServices + "Sede/ConsultarColegios")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarInstructores = function (IdArea, callback) {

            $http.get(URLServices + "Programacion/GetInstructores/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarCompetencias = function (IdArea, callback) {
            
            var item = {
                Parametro1: IdArea
            };
            $http.post(URLServices + "Programacion/GetCompetencias/", item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.GuardarProgramacion = function (Programacion, callback) {
            waitingDialog.show();
            $http.post(URLServices + "Programacion/GuardarProgramacion", Programacion)
                .success(function (response) {
                    callback(response);
                    waitingDialog.hide();
                });
        };

        service.ConsultarProgramacion = function (IdArea, callback) {
            
            var item = {
                Parametro1: IdArea
            };

            $http.post(URLServices + "Programacion/ConsultarProgramacion/", item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarProgramacionSabados = function (IdArea, callback) {
            
            var item = {
                Parametro1: IdArea
            };

            $http.post(URLServices + "Programacion/ConsultarProgramacionSabados/", item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarProgramacionTransversales = function (IdArea, callback) {
            
            var item = {
                Parametro1: IdArea
            };

            $http.post(URLServices + "Programacion/ConsultarProgramacionTransversales/", item)
                .success(function (response) {
                    callback(response);
                });
        };


        service.FiltrarAmbiente = function (IdAmbiente, jornada, callback) {
            var Item = {
                Parametro1: IdAmbiente,
                Parametro2: jornada
            };
            $http.post(URLServices + "Programacion/FiltrarAmbiente", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarResultados = function (IdCompetencia, callback) {
            Item = {
                Parametro1: IdCompetencia.toString()
            };
            $http.post(URLServices + "Programacion/ConsultarResultados/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarPragamaTransversal = function (callback) {
           
            $http.post(URLServices + "Programa/ConsultarPragamaTransversal/")
                .success(function (response) {
                    callback(response);
                });
        };

        service.Disponibilidad = function (Transversal, IdCoordinacion, Programacion, IdSede, IdPrograma, FinSemana,sede,Jornada, callback) {
            var Item = {
                Parametro1: IdCoordinacion,
                Parametro2: Programacion.FechaInicio,
                Parametro3: Programacion.FechaFin,
                Parametro4: Programacion.HoraInicio,
                Parametro5: Programacion.HoraFin,
                Parametro6: Transversal,
                Parametro7: IdSede,
                Parametro8: IdPrograma,
                Parametro9: Programacion.Lunes,
                Parametro10: Programacion.Martes,
                Parametro11: Programacion.Miercoles,
                Parametro12: Programacion.Jueves,
                Parametro13: Programacion.Viernes,
                Parametro14: FinSemana.Sabado,
                Parametro15: FinSemana.Domingo,
                Parametro16: sede,
                Parametro17: Jornada.Manana,
                Parametro18: Jornada.Tarde,
                Parametro19: Jornada.Noche,
            }
            $http.post(URLServices + "Programacion/Disponibilidad/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.DisponibilidadTransversalCJ = function (Transversal, IdCoordinacion, Programacion, IdSede, IdPrograma, FinSemana, callback) {
            var Item = {
                Parametro1: IdCoordinacion,
                Parametro2: Programacion.FechaInicio,
                Parametro3: Programacion.FechaFin,
                Parametro4: Programacion.HoraInicio,
                Parametro5: Programacion.HoraFin,
                Parametro6: Transversal,
                Parametro7: IdSede,
                Parametro8: IdPrograma,
                Parametro9: Programacion.Lunes,
                Parametro10: Programacion.Martes,
                Parametro11: Programacion.Miercoles,
                Parametro12: Programacion.Jueves,
                Parametro13: Programacion.Viernes,
                Parametro14: FinSemana.Sabado,
                Parametro15: FinSemana.Domingo
            }
            $http.post(URLServices + "Programacion/DisponibilidadTransversalCJ/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.Disponibilidad1 = function (Transversal, IdCoordinacion, Programacion, callback) {
            var Item = {
                Parametro1: IdCoordinacion,
                Parametro2: Programacion.FechaInicio,
                Parametro3: Programacion.FechaFin,
                Parametro4: Programacion.HoraInicio,
                Parametro5: Programacion.HoraFin,
                Parametro6: Transversal,
                Parametro7: Programacion.Jornada,
                Parametro8: Programacion.IdInstructor,
                Parametro9: Programacion.IdFicha
            }
            $http.post(URLServices + "Programacion/Disponibilidad1/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarProgramacionxInstructor = function (IdInstructor,Jornada,  callback) {
            Item = {
                Parametro1: IdInstructor.toString(),
                Parametro2: Jornada
            };
            $http.post(URLServices + "Programacion/ConsultarProgramacionxInstructor/", Item)
                .success(function (response) {
                    callback(response);
                });
        };


        service.DisponibilidadTrasversal = function (IdCoordinacion, Programacion, FinSemana, callback) {
            
            var Item = {
                Parametro1: IdCoordinacion,
                Parametro2: Programacion.FechaInicio,
                Parametro3: Programacion.FechaFin,
                Parametro4: Programacion.HoraInicio,
                Parametro5: Programacion.HoraFin,
                Parametro6: Programacion.ProgramacionPrincipal,
                Parametro7: Programacion.DiaSemana,
                Parametro8: Programacion.Lunes,
                Parametro9: Programacion.Martes,
                Parametro10: Programacion.Miercoles,
                Parametro11: Programacion.Jueves,
                Parametro12: Programacion.Viernes,
                Parametro13: FinSemana.Sabado,
                Parametro14: FinSemana.Domingo
            }
            $http.post(URLServices + "Programacion/DisponibilidadTrasversal/", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.GuardarProgramacionTrasversal = function (Programacion, callback) {
            var Item = {
                Parametro1: Programacion.Ambiente,
                Parametro2: Programacion.FechaInicio,
                Parametro3: Programacion.FechaFin,
                Parametro4: Programacion.HoraInicio,
                Parametro5: Programacion.HoraFin,

            }

            $http.post(URLServices + "Programacion/GuardarProgramacionTrasversal", Programacion)
                .success(function (response) {
                    callback(response);
                });
        };



        service.ConsultarResultados1 = function (callback) {
            $http.get(URLServices + "Resultado/ConsultarResultados1/")
                .success(function (response) {
                    callback(response);
                });
        };




        service.EliminarProgramacion = function (Programacion, callback) {
            $http.post(URLServices + "Programacion/EliminarProgramacion/", Programacion[0])
              .success(function (response) {
                  callback(response);
              })
        };

        service.ConsultaEliminarProgramacionTransversales = function (Programacion, callback) {
            $http.post(URLServices + "Programacion/ConsultaEliminarProgramacionTransversales/", Programacion[0])
              .success(function (response) {
                  callback(response);
              })
        };

        service.ConsultarTransversales = function (Programacion, callback) {
            var Item = {
                Parametro1: Programacion
            };
            $http.post(URLServices + "Programacion/ConsultarTransversales", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarSedes = function (callback) {
            $http.get(URLServices + "Ambiente/ConsultarSedes/")
              .success(function (response) {
                  callback(response);
              })
        };

        service.ConsultarProgramaxCoordinacion = function (IdCoordinador, callback) {
            var Item = {
                Parametro1: IdCoordinador
            };
            $http.post(URLServices + "Programa/ConsultarProgramaxCoordinacion", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ProgramaTituladosxCoordinacion = function (IdCoordinador, callback) {
            var Item = {
                Parametro1: IdCoordinador
            };
            $http.post(URLServices + "Programa/ProgramaTituladosxCoordinacion", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ProgramaVirtualesTitulados = function (IdCoordinador, callback) {
            var Item = {
                Parametro1: IdCoordinador
            };
            $http.post(URLServices + "Programa/ProgramaVirtualesTitulados", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ProgramasComplementariosxCoordinacion = function (IdCoordinador, callback) {
            var Item = {
                Parametro1: IdCoordinador
            };
            $http.post(URLServices + "Programa/ProgramasComplementariosxCoordinacion", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarCompetenciasxPrograma = function (IdPrograma, callback) {
            var Item = {
                Parametro1: IdPrograma
            };
            $http.post(URLServices + "Competencia/ConsultarCompetenciasxPrograma", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarCoordinadores = function (callback) {
            $http.get(URLServices + "Coordinacion/ConsultarCoordinacion/")
              .success(function (response) {
                  callback(response);
              })
        };

        service.FiltroProgramacionxCoordinador = function (IdCoordinador, jornada, Ambiente, callback) {
            waitingDialog.show();
            var Item = {
                Parametro1: IdCoordinador,
                Parametro2: jornada,
                Parametro3: Ambiente
            };
            $http.post(URLServices + "Programacion/FiltroProgramacionxCoordinador", Item)
                .success(function (response) {
                    callback(response);
                    waitingDialog.hide();
                });
        };

        service.ConsultarAmbientesCoordinador = function (IdCoordinador, callback) {
            var Item = {
                Parametro1: IdCoordinador,
            };
            $http.post(URLServices + "Ambiente/ConsultarAmbientesCoordinador", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarDiaTransversal = function (Programacion, ficha, callback) {
            var Item = {
                Parametro1: Programacion.FechaInicio,
                Parametro2: Programacion.FechaFin,
                Parametro3: Programacion.HoraInicio,
                Parametro4: Programacion.HoraFin,
                Parametro5: ficha
            };
            $http.post(URLServices + "Programacion/ConsultarDiaTransversal", Item)
                .success(function (response) {
                    callback(response);
                });
        };


        service.GenerarReporte = function (fechas,IdCoordinador, callback) {
            var Item = {
                Parametro1: fechas.FechaInicio,
                Parametro2: fechas.FechaFin,
                Parametro3: IdCoordinador
            };
            $http.post(URLServices + "Programacion/GenerarReporte", Item)
                .success(function (response) {
                    callback(response);
                });
        };

        service.EnviarProgramacion = function (Programaciones, callback) {
            
            $http.post(URLServices + "Programacion/EnviarProgramacion", Programaciones)
                .success(function (response) {
                    callback(response);
                });
        };

        service.ConsultarAmbientesxSede = function (IdSede, callback) {
            var Item = {
                Parametro1: IdSede
               
            };
            $http.post(URLServices + "Programacion/ConsultarAmbientesxSede/", Item)
              .success(function (response) {
                  callback(response);
              })
        };


        service.ProgramasMediaTecnica = function (IdCoordinador, callback) {
            var Item = {
                Parametro1: IdCoordinador

            };
            $http.post(URLServices + "Programacion/ProgramasMediaTecnica/", Item)
              .success(function (response) {
                  callback(response);
              })
        };
        
        service.ConsultarSedexId = function (IdSede, callback) {
            var Item = {
                Parametro1: IdSede

            };
            $http.post(URLServices + "Sede/ConsultarSedexId/", Item)
              .success(function (response) {
                  callback(response);
              })
        };


        service.FichasxPrograma = function (IdPrograma, callback) {
            var Item = {
                Parametro1: IdPrograma
            };
            $http.post(URLServices + "Ficha/ConsultarFichasxPrograma/", Item)
              .success(function (response) {
                  callback(response);
              })
        };


        return service;

    }]);