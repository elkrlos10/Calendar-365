ProgramacionApp.controller('ProgramacionController',
    ['$scope', '$rootScope', '$location', '$cookies', '$cookieStore', 'ProgramacionService', '$routeParams', '$sce', '$http', '$notify',
        function ($scope, $rootScope, $location, $cookies, $cookieStore, ProgramacionService, $routeParams, $sce, $http, $notify) {

            var nav = $("#navbar").hasClass("gn-menu-wrapper gn-open-all");
            $("#btntransversaldia").css("display", "block");
            if (nav == true) {
                $(".Principal").css("width", "80%");

            } else {
                $(".Principal").css("width", "95%");
            }

            //Clase para ocultar botones colegio, titulados y complementarias
            $(".ocultar").hide();
            $(".ocultarLlaves").hide();

            // var dato = $("#calendar .fc-celda-act").attr("data-date");

            if ($rootScope.globals.currentUser.tipousuario == 1) {
                $(".coord").css("display", "none");
                $(".admin").css("display", "block");
                //$(".admin").css("margin-left", "-435%");
                $(".instructor").css("display", "none");
                $(".consulta").css("display", "none");
                $("#ProgInstructor").css("display", "none");
                $("#panel2").hide();
            }
            if ($rootScope.globals.currentUser.tipousuario == 2) {
                $(".coord").css("display", "block");
                $(".admin").css("display", "none");
                $(".instructor").css("display", "none");
                $(".consulta").css("display", "none");
                $("#btntransversaldia").hide();
                $("#ProgInstructor").css("display", "none");
                $("#panel2").hide();
                $("#llaves").hide();
                $(".noCor").hide();
            }
            if ($rootScope.globals.currentUser.tipousuario == 3) {
                $(".instructor").css("display", "block");
                $(".coord").css("display", "none");
                $(".admin").css("display", "none");
                $(".consulta").css("display", "none");
                $(".instructor").css("margin-left", "-210%");
                $("#ProgInstructor").css("display", "none");
                $("#panel2").hide();
            }
            if ($rootScope.globals.currentUser.tipousuario == 4) {
                $(".instructor").css("display", "none");
                $(".coord").css("display", "none");
                $(".admin").css("display", "none");
                $(".instructor").css("display", "none");
                $(".consulta").css("display", "none");
                //$("#transversal3").css("display", "block");
                $("#panel2").show();
                $("#panel1").hide();
                $("#Filtro").css("display", "none");
                $("#BuscarCedulaInstructor").css("display", "block");
                $(".noConsulta").css("display", "none");
                $(".ocultarLlaves").show();
                $("#cedulaBuscar").focus();
            }


            $scope.UsuarioConsulta = $rootScope.globals.currentUser.tipousuario;

            // Variables y metodos para la paginación----------------------
            $scope.curPage = 0;
            $scope.pageSize = 5;

            $scope.numberOfPages = function () {
                return Math.ceil($scope.datalists.length / $scope.pageSize);
            };
            //-------------------------------------------------------------

            //Variable para validar que si la sede es virtual la programación se realice desde la modal de programacion técnica 
            var ModalJornada = 0;

            //Ocultar botonones -------------------------------------------
            $("#transversal").hide();
            $("#atras").hide();
            $("#dia").hide();
            $(".dia1").hide();

            //$("#form1").css("display", "none");
            //$("#form2").css("display", "none");
            //$("#form3").css("display", "none");
            //$("#form4").css("display", "none");
            //-------------------------------------------------------------

            //Función para abrir la modal de tipos de programaciones------
            $scope.AbrirModalesProgramaciones = function () {
                $("#ModalTipoProgramaciones").modal("show");
            };
            //------------------------------------------------------------

            // FUNCIONES PARA OCULTAR LOS PANELES DE DISPONIBLIDAD EN LAS MODALES DE PROGRAMACIÒN
            $(".OcultarPanelDatos").click(function () {
                $("#Datos").css("display", "none");
                $("#guardarTecnica").css("display", "none");
            });

            $(".OcultarPanelDatosTransversal").click(function () {
                $("#Datos2").css("display", "none");
                $("#guardartraslape").css("display", "none");
            });

            $(".OcultarPanelDatosCJ").click(function () {
                $("#DatosTC").css("display", "none");
                $("#guardarCJ").css("display", "none");
            });

            $(".OcultarPanelDatosJor").click(function () {
                $("#DatosJornada").css("display", "none");
                $("#guardarTecnicaJornada").css("display", "none");
            });
            //---------------------------------------------------------------------

            //Función para consultar las sedes-------------------------------------
            $scope.ConsultaSedes = function () {
                if ($scope.ObjetoSedes.Sede == true) {

                    $scope.ObjetoSedes.Colegio = false;

                    ProgramacionService.ConsultarSedes(function (response) {
                        if (response.success == true) {

                            $scope.Sede = response.datos;

                            ProgramacionService.ConsultarProgramaxCoordinacion($rootScope.globals.currentUser.idpersona, function (response) {
                                if (response.success == true) {
                                    $scope.Programa = response.programa;
                                    if (response.programa.length == 0) {
                                        bootbox.dialog({
                                            title: "Información",
                                            message: "No tiene programas titulados asociados, por favor cambie su selección a colegios",
                                            buttons: {
                                                success: {
                                                    label: "Cerrar",
                                                    className: "btn-primary",
                                                }
                                            }
                                        });

                                        $scope.ObjetoSedes.Sede = false;
                                        $scope.ObjetoSedes.Colegio = true;
                                        $scope.ConsultarColegios();

                                        return;
                                    }
                                }
                            });
                        }
                    });
                }
                if ($scope.ObjetoSedes.Sede == false) {
                    $scope.Programa = "";
                    $scope.Sede = "";
                    $scope.ObjetoPrograma.Titulado = false;
                    $scope.ObjetoPrograma.Complementario = false;
                    //$("#form1").css("display", "none");
                    //$("#form2").css("display", "none");
                    $("#divDerecho").css("margin-top", "5px");
                    $("#divDerecho1").css("margin-top", "5px");
                    //$("#form3").css("display", "none");
                    //$("#form4").css("display", "none");
                }
            };
            //---------------------------------------------------------------------

            //Funcíon para consultar los colegios----------------------------------
            $scope.ConsultarColegios = function () {
                if ($scope.ObjetoSedes.Colegio == true) {
                    $scope.ObjetoSedes.Sede = false;
                    ProgramacionService.ConsultarColegios(function (response) {
                        if (response.success == true) {
                            $scope.Sede = response.colegios;

                            ProgramacionService.ProgramasMediaTecnica($rootScope.globals.currentUser.idpersona, function (response) {
                                if (response.success == true) {
                                    $scope.Programa = response.datos;
                                    if (response.datos.length == 0) {
                                        bootbox.dialog({
                                            title: "Información",
                                            message: "No tiene programas de media técnica asociados, por favor cambie su selección a sedes",
                                            buttons: {
                                                success: {
                                                    label: "Cerrar",
                                                    className: "btn-primary",
                                                }
                                            }
                                        });

                                        $scope.ObjetoSedes.Colegio = false;
                                        $scope.ObjetoSedes.Sede = true;
                                        $scope.ConsultaSedes();
                                        $scope.ObjetoPrograma.Titulado = false;
                                        $scope.ObjetoPrograma.Complementario = false;
                                        //$("#form1").css("display", "none");
                                        //$("#form2").css("display", "none");
                                        //$("#form3").css("display", "none");
                                        //$("#form4").css("display", "none");
                                        //$("#divDerecho").css("margin-top", "47px");
                                        //$("#divDerecho1").css("margin-top", "47px");

                                        return;
                                    }
                                }
                            });

                            //$scope.ObjetoPrograma.Titulado = false;
                            //$scope.ObjetoPrograma.Complementario = false;
                            //$("#form1").css("display", "none");
                            //$("#form2").css("display", "none");
                            //$("#form3").css("display", "none");
                            //$("#form4").css("display", "none");
                            //$("#divDerecho").css("margin-top", "47px");
                            //$("#divDerecho1").css("margin-top", "47px");
                        }
                    });

                } else {
                    $scope.Sede = "";
                }

            };
            //---------------------------------------------------------------------

            //Consulta de los programas titulados por coordinación-----------------
            $scope.ProgramasTitulados1 = function () {
                if ($scope.ObjetoPrograma.Titulado == true) {
                    $scope.ObjetoPrograma.Complementario = false;
                    ProgramacionService.ProgramaTituladosxCoordinacion($rootScope.globals.currentUser.idpersona, function (response) {
                        if (response.success == true) {
                            $scope.Programa = response.datos;
                        }
                    });
                }
            }
            //---------------------------------------------------------------------

            //Consulta de los programas complementarios por coordinación-----------
            $scope.ProgramasComplementarios = function () {
                if ($scope.ObjetoPrograma.Complementario == true) {
                    $scope.ObjetoPrograma.Titulado = false;
                    ProgramacionService.ProgramasComplementariosxCoordinacion($rootScope.globals.currentUser.idpersona, function (response) {
                        if (response.success == true) {
                            $scope.Programa = response.datos;
                        }
                    });
                }
            }
            //---------------------------------------------------------------------

            //Función para abri la modal donde se realizan las programaciones técnicas
            $scope.AbrirModal = function () {
                $("#Datos").css("display", "none");
                $("#ModalProgramacion").modal("show");
                $("#panel2").hide();
                $("#panel1").show();
                $("#ModalTipoProgramaciones").modal("hide");
                ModalJornada = 0;
                $scope.ConsultaSedes();

            };
            //-----------------------------------------------------------------------

            //Función para abri la modal donde se realizan las programaciones técnicas con validación de jornadas
            $scope.AbrirModalJornada = function () {
                $("#DatosJornada").css("display", "none");
                $("#ModalProgramacionxJornada").modal("show");
                $("#panel2").hide();
                $("#panel1").show();
                $("#ModalTipoProgramaciones").modal("hide");
                ModalJornada = 1;
                $scope.ConsultaSedes();

            };
            //------------------------------------------------------------------------

            //
            $scope.AbrirModalDias = function () {
                $("#Datos").css("display", "none");
                $("#ModalProgramacionDias").modal("show");
                $("#panel2").hide();
                $("#panel1").show();
                //$scope.VaciarCampos();
                ProgramacionService.ConsultarSedes(function (response) {
                    if (response.success == true) {
                        $scope.Sede = response.datos;
                    }
                });

                // consultas de información por defecto 
                ProgramacionService.ConsultarProgramaxCoordinacion($rootScope.globals.currentUser.idpersona, function (response) {
                    if (response.success == true) {
                        $scope.Programa = response.programa;

                    }
                });
            };


            //Consulta de todas las sede---------------------------------------------
            ProgramacionService.ConsultarSedes(function (response) {
                if (response.success == true) {
                    $scope.Sede1 = response.datos;
                }
            });
            //----------------------------------------------------------------------

            //------------------------------------Configuración de los campos de fechas ---------------------------------------
            $('#FechaInicio').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#FechaFin').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#HoraInicio').datetimepicker({
                format: 'HH:mm',
            });

            $('#HoraFin').datetimepicker({
                format: 'HH:mm'
            });

            $('#HoraInicio1').datetimepicker({
                format: 'HH:mm',
            });

            $('#HoraFin1').datetimepicker({
                format: 'HH:mm'
            });

            $('#HoraInicio2').datetimepicker({
                format: 'HH:mm',
            });

            $('#HoraFin2').datetimepicker({
                format: 'HH:mm'
            });

            $('#FechaInicioT').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#FechaFinT').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#HoraInicioT').datetimepicker({
                format: 'HH:mm',
            });

            $('#HoraFinT').datetimepicker({
                format: 'HH:mm'
            });

            $('#FechaInicioTC').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#FechaFinTC').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#HoraInicioTC').datetimepicker({
                format: 'HH:mm',
            });

            $('#HoraFinTC').datetimepicker({
                format: 'HH:mm'
            });

            $('#FechaInicioD').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#FechaFinD').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#HoraInicioD').datetimepicker({
                format: 'HH:mm',
            });

            $('#HoraFinD').datetimepicker({
                format: 'HH:mm'
            });

            $('#FechaInicioJornada').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#FechaFinJornada').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#HoraInicioJornada').datetimepicker({
                format: 'HH:mm',
            });

            $('#HoraFinJornada').datetimepicker({
                format: 'HH:mm'
            });
            //------------------------------------------------------------------------------------------------------------


            // --------------------------------------Declaración de Objetos ----------------------------------------------
            $scope.AmbienteFiltro = {
                IdAmbiente: "       ",
                Numero: "",
                IdArea: "",
                Piso: "",
                Pantalla: "",
                NumeroEquipos: "",
            };

            $scope.Competencia = {
                IdCompetencia: "",
                Nombre: "",
                Descripcion: "",
                IdPrograma: "",
                Nombre: ""
            };

            $scope.Competencia1 = {
                IdCompetencia: "",
                Nombre: "",
                Descripcion: "",
                IdPrograma: "",
                Nombre: ""
            };

            $scope.Resultado = {
                Codigo: "",
                Resultado: "",
                IdCompetencia: ""
            };

            $scope.Resultado1 = {
                Codigo: "",
                Resultado: "",
                IdCompetencia: ""
            };

            $scope.Instructor = {
                IdInstructor: "",
                Nombre: "",
                Apellido: "",
                Cedula: "",
                Email: "",
                Estado: "",
                IdCompetencia: ""
            };

            $scope.Instructor1 = {
                IdInstructor: "",
                Nombre: "",
                Apellido: "",
                Cedula: "",
                Email: "",
                Estado: "",
                IdCompetencia: ""
            };

            $scope.Programacion = {
                IdFicha: '',
                IdResultado: '',
                IdAmbiente: '',
                IdInstructor: '',
                FechaInicio: '',
                FechaFin: '',
                HoraInicio: '',
                HoraFin: '',
                Jornada: "",
                DiaSemana: "",
                Lunes: false,
                Martes: false,
                Miercoles: false,
                Jueves: false,
                Viernes: false,
                Transversal: false,
                NombreEmpresa: ""
            };

            $scope.Programacion1 = {
                IdFicha: '',
                IdResultado: '',
                IdAmbiente: '',
                IdInstructor: '',
                FechaInicio: '',
                FechaFin: '',
                HoraInicio: '',
                HoraFin: '',
                Jornada: "",
                DiaSemana: ""
            };

            $scope.ObjetoSedes = {
                Sede: false,
                Colegio: false
            };

            $scope.ObjetoPrograma = {
                Titulado: false,
                Complementario: false
            };

            $scope.Sabado = {
                sabado: ""
            };

            $scope.SabadoDomingo = {
                Sabado: false,
                Domingo: false
            };

            $scope.Transversal = {
                Transversal: false
            }

            $scope.Ambiente = {
                Numero: "",
                IdArea: "",
                Piso: "",
                Pantalla: "",
                NumeroEquipos: "",
            };

            $scope.Ambiente1 = {
                Numero: "",
                IdArea: "",
                Piso: "",
                Pantalla: "",
                NumeroEquipos: "",
            };

            $scope.Ficha = {
                IdFicha: "",
                IdArea: "",
                Ficha1: "",
                NumAprendices: "",
                Estado: "",
                TipoEscolaridad: ""
            };

            $scope.Ficha1 = {
                IdFicha: "",
                IdArea: "",
                Ficha1: "",
                NumAprendices: "",
                Estado: "",
                TipoEscolaridad: ""
            };

            $scope.consultaProgramaciones = {

                IdFicha: '',
                Resultado: '',
                Ambiente: '',
                IdInstructor: '',
                FechaInicio: '',
                FechaFin: '',
                HoraInicio: '',
                HoraFin: '',
                Estado: "",
                Jornada: ""

            };

            $scope.datalists = {
                FechaInicio: "",
                FechaFin: "",
                HoraInicio: "",
                HoraFin: "",
                Ambiente: "",
                Ficha: "",
                Resultado: ""

            };

            $scope.Jornada = {
                Manana: false,
                Tarde: false,
                Noche: false
            };

            $scope.ProgramacionSeleccionada1 = {

                IdFicha: '',
                Resultado: '',
                Ambiente: '',
                IdInstructor: '',
                FechaInicio: '',
                FechaFin: '',
                HoraInicio: '',
                HoraFin: '',
                Estado: "",
                Jornada: "",
                Ficha: "",
                DiaSemana: "",
                ProgramacionPrincipal: "",
                Lunes: false,
                Martes: false,
                Miercoles: false,
                Jueves: false,
                Viernes: false,
                Transversal: false
            }


            //-----------------------------------------------------------------------------------------------------

            //Funciones asociadas a la programación los fines de semana--------------------------------------------
            $scope.ModalSabado = function () {

                $('#ModalSabado').modal('show');
            }

            $("#finsemana").hide();
            $("#finsemanaJornada").hide();

            $scope.SoloFinesSemana = function () {
                $scope.Programacion.Lunes = false;
                $scope.Programacion.Martes = false;
                $scope.Programacion.Miercoles = false;
                $scope.Programacion.Jueves = false;
                $scope.Programacion.Viernes = false;

                if ($scope.ValidarFinSemana == true) {
                    $scope.Sabado.sabado = false;
                    $("#finsemana").css("display", "block");
                    $("#semana").css("display", "none");
                    $("#finsemanaJornada").css("display", "block");
                    $("#semanaJornada").css("display", "none");
                } else {
                    $("#finsemana").css("display", "none");
                    $("#semana").css("display", "block");
                    $("#finsemanaJornada").css("display", "none");
                    $("#semanaJornada").css("display", "block");
                }

                if ($scope.SabadoDomingo.Sabado == true && $scope.SabadoDomingo.Domingo == true) {
                    setTimeout(function () {
                        $scope.SabadoDomingo.Domingo = false;
                    }, 2000);
                }
                if ($scope.SabadoDomingo.Domingo == true) {
                    $scope.SabadoDomingo.Sabado = false;
                }
            }

            $scope.ValidarJornada1 = function () {
                $scope.Jornada.Tarde = false;
                $scope.Jornada.Noche = false;

            };

            $scope.ValidarJornada2 = function () {
                $scope.Jornada.Manana = false;
                $scope.Jornada.Noche = false;
            };

            $scope.ValidarJornada3 = function () {
                $scope.Jornada.Manana = false;
                $scope.Jornada.Tarde = false;
            };

            $scope.DesactivarSabado = function () {
                if ($scope.SabadoDomingo.Domingo == true) {
                    $scope.SabadoDomingo.Sabado = false;
                }

            }

            $scope.DesactivarDomingo = function () {
                if ($scope.SabadoDomingo.Sabado == true) {
                    $scope.SabadoDomingo.Domingo = false;
                }

            }

            $scope.CerrarValidarFinSemana = function () {
                $scope.ValidarFinSemana = false;
                $("#finsemana").css("display", "none");
                $("#semana").css("display", "block");
                $("#finsemanaJornada").css("display", "none");
                $("#semanaJornada").css("display", "block");
            };
            //------------------------------------------------------------------------------------------------------

            //Limpia los datos del fullcalenar----------------------------------------------------------------------
            $scope.Limpiar = function (ambiente) {

                $('#calendar').fullCalendar('removeEventSource', $scope.events);
                $('#calendar').fullCalendar('refetchEvents');
                $('#calendar').fullCalendar('destroy');
                $scope.events = [];
            };
            //------------------------------------------------------------------------------------------------------

            //Función para seleccionar todos los días de la semana a la hora de programar
            $scope.SeleccionarTodosDias = function () {
                if ($scope.TodosDias == true) {
                    $scope.Programacion.Lunes = true;
                    $scope.Programacion.Martes = true;
                    $scope.Programacion.Miercoles = true;
                    $scope.Programacion.Jueves = true;
                    $scope.Programacion.Viernes = true;
                }
            };
            //-----------------------------------------------------------------------------------------------------

            //Consultar programación a instructor logueado---------------------------------------------------------
            $scope.consultaProgramacionesxInstructor = function () {
                if ($rootScope.globals.currentUser.tipousuario == 3) {
                    $(".not-inst").css("display", "none");

                    ProgramacionService.ConsultarProgramacionxInstructor($rootScope.globals.currentUser.idpersona, $scope.RangoHoras.Jornada, function (response) {
                        $scope.Limpiar();
                        if (response.success == true) {

                            $("#ModalFiltroInstructor").modal("hide");
                            $.each(response.programacion, function (index, value) {
                                var Inicio = value.FechaInicio.split('-');
                                var Fin = value.FechaFin.split('-');
                                var HoraInicio = value.HoraInicio.split(':');
                                var HoraFin = value.HoraFin.split(':');
                                $scope.events.push({
                                    title: ' Ficha: ' + value.Ficha + '- Ambiente: ' + value.Ambiente,
                                    start: new Date(parseInt(Inicio[0]), parseInt(Inicio[1]) - 1, parseInt(Inicio[2].substring(0, 2)), parseInt(HoraInicio[0]), parseInt(HoraInicio[1])),
                                    end: new Date(parseInt(Fin[0]), parseInt(Fin[1]) - 1, parseInt(Fin[2].substring(0, 2)), parseInt(HoraFin[0]), parseInt(HoraFin[1])),
                                    allDay: false,
                                    color: value.Color,
                                    tip: "PROGRAMA: " + value.Programa.toUpperCase() + " - " + "INSTRUCTOR:" + value.NombreInstructor.toUpperCase() + " - " + "RESULTADO:" + value.Competencia.toUpperCase()
                                });

                            });
                            var calendar = $('#calendar').fullCalendar({
                                header: {
                                    left: 'title',
                                    center: 'agendaDay,agendaWeek,month',
                                    right: 'prev,next today'

                                },
                                height: 700,
                                theme: false,
                                ignoreTimezone: false,
                                events: $scope.events,
                                eventRender: function (event, element) {
                                    element.attr('title', event.tip);
                                },
                                editable: false,
                                firstDay: 1, //  1(Monday) this can be changed to 0(Sunday) for the USA system
                                selectable: false,
                                defaultView: 'month',
                                axisFormat: 'H:mm',
                                columnFormat: {
                                    month: 'ddd',    // Mon
                                    week: 'ddd d', // Mon 7
                                    day: 'dddd M/d',  // Monday 9/7
                                    agendaDay: 'dddd d'
                                },
                                titleFormat: {
                                    month: 'MMMM yyyy', // September 2009
                                    week: "MMMM yyyy", // September 2009
                                    day: 'MMMM yyyy'                  // Tuesday, Sep 8, 2009
                                },
                                allDaySlot: false,
                                selectHelper: true,
                                select: function (start, end, allDay) {
                                    var title = prompt('Event Title:');
                                    if (title) {
                                        calendar.fullCalendar('renderEvent',
                                            {
                                                title: title,
                                                start: start,
                                                end: end,
                                                allDay: allDay
                                            },
                                            true // make the event "stick"
                                        );
                                    }
                                    calendar.fullCalendar('unselect');
                                },
                                droppable: true, // this allows things to be dropped onto the calendar !!!
                                drop: function (date, allDay) { // this function is called when something is dropped

                                    // retrieve the dropped element's stored Event Object
                                    var originalEventObject = $(this).data('eventObject');

                                    // we need to copy it, so that multiple events don't have a reference to the same object
                                    var copiedEventObject = $.extend({}, originalEventObject);

                                    // assign it the date that was reported
                                    copiedEventObject.start = date;
                                    copiedEventObject.allDay = allDay;

                                    // render the event on the calendar
                                    // the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
                                    $('#calendar').fullCalendar('renderEvent', copiedEventObject, true);

                                    // is the "remove after drop" checkbox checked?
                                    if ($('#drop-remove').is(':checked')) {
                                        // if so, remove the element from the "Draggable Events" list
                                        $(this).remove();
                                    }

                                },


                            });
                        }
                    });
                    $scope.Limpiar();
                }
            }
            //-----------------------------------------------------------------------------------------------------

            //Filtra la programación por instructor-------------------------------------------------------------
            $scope.filtrarProgramacionInstructor = function () {

                $("#ModalFiltroInstructor").modal("show");
                $("#panel2").hide();
                $("#panel1").show();
            };
            //--------------------------------------------------------------------------------------------------


            //Disponibilidad de instructor, ambiente y ficha del día sábado----------------------------------------
            $scope.Disponibilidad1 = function () {

                $scope.Programacion1.HoraInicio = $("#HoraInicio1").val();
                $scope.Programacion1.HoraFin = $("#HoraFin1").val();
                if ($scope.Programacion1.HoraInicio == "" || $scope.Programacion1.HoraFin == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe diligenciar todos los campos",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }

                if ($scope.Programacion1.HoraInicio >= $scope.Programacion1.HoraFin) {
                    bootbox.dialog({
                        title: "Información",
                        message: "La hora final debe ser mayor a la hora inicial",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                var x = $("#FechaInicio").val().split('/');
                var y = $("#FechaFin").val().split('/');

                var StartDate = x[1] + "-" + x[2] + "-" + x[0];
                var EndDate = y[1] + "-" + y[2] + "-" + y[0];
                if (Date.parse(StartDate) > Date.parse(EndDate)) {
                    bootbox.dialog({
                        title: "Información",
                        message: "La fecha final debe ser mayor a la fecha inicial",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }

                $scope.Programacion1.FechaInicio = $scope.Programacion.FechaInicio;
                $scope.Programacion1.FechaFin = $scope.Programacion.FechaFin;
                $scope.Programacion1.HoraInicio = $("#HoraInicio1").val();
                $scope.Programacion1.HoraFin = $("#HoraFin1").val();
                $scope.Programacion1.IdInstructor = $scope.Programacion.IdInstructor;
                $scope.Programacion1.IdFicha = $scope.Programacion.IdFicha;
                if ($scope.FinSemana.dia == 1) {

                    $scope.Programacion1.Jornada = 2;

                }

                if ($scope.FinSemana.dia == 2) {

                    $scope.Programacion1.Jornada = 3;

                }
                if ($scope.Programacion1.Jornada == "" || $scope.Programacion1.HoraInicio == "" || $scope.Programacion1.HoraFin == "") {

                    bootbox.dialog({
                        title: "Información",
                        message: "Debe diligenciar todos los campos",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;

                } else {

                    ProgramacionService.Disponibilidad1($scope.Transversal.Transversal, $rootScope.globals.currentUser.idpersona, $scope.Programacion1, function (response) {
                        if (response.success == true) {

                            $scope.Ambiente1 = response.ambiente;
                            $scope.Instructor1 = response.instructor;
                            $("#Datos1").css("display", "block");

                            if (response.MensajeInstructor != "") {
                                bootbox.dialog({
                                    title: "Información",
                                    message: response.MensajeInstructor,
                                    buttons: {
                                        success: {
                                            label: "Cerrar",
                                            className: "btn-primary",
                                        }
                                    }
                                });
                                return;

                            }

                            if (response.MensajeFicha != "") {
                                bootbox.dialog({
                                    title: "Información",
                                    message: response.MensajeFicha,
                                    buttons: {
                                        success: {
                                            label: "Cerrar",
                                            className: "btn-primary",
                                        }
                                    }
                                });
                                return;

                            }
                        }
                    });
                }
            }
            //-----------------------------------------------------------------------------------------------------

            //Disponiblidad instructores transversales-------------------------------------------------------------            
            $scope.DisponibilidadTransversal = function () {
                $scope.Transversal.Transversal = true;
                $scope.Programacion.HoraInicio = $("#HoraInicioT").val();
                $scope.Programacion.HoraFin = $("#HoraFinT").val();
                if ($scope.Programacion.HoraInicio >= $scope.Programacion.HoraFin) {
                    bootbox.dialog({
                        title: "Información",
                        message: "La hora final debe ser mayor a la hora inicial",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                var x = $("#FechaInicioT").val().split('/');
                var y = $("#FechaFinT").val().split('/');

                var StartDate = x[1] + "-" + x[2] + "-" + x[0];
                var EndDate = y[1] + "-" + y[2] + "-" + y[0];
                if (Date.parse(StartDate) > Date.parse(EndDate)) {
                    bootbox.dialog({
                        title: "Información",
                        message: "La fecha final debe ser mayor a la fecha inicial",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                $scope.Programacion.HoraInicio = $("#HoraInicioT").val();
                $scope.Programacion.HoraFin = $("#HoraFinT").val();
                if ($scope.Transversal.Transversal) {

                }
                ProgramacionService.Disponibilidad($scope.Transversal.Transversal, $rootScope.globals.currentUser.idpersona, $scope.Programacion, $scope.Sede.IdSede, $scope.Programa.IdPrograma, $scope.SabadoDomingo, function (response) {
                    if (response.success == true) {
                        $scope.Ficha = response.ficha;
                        $scope.Ambiente = response.ambiente;
                        $scope.Instructor = response.instructor;
                        ProgramacionService.ConsultarProgramaxCoordinacion($rootScope.globals.currentUser.idpersona, function (response) {
                            if (response.success == true) {
                                $scope.Programa1 = response.programa;

                                $.each($scope.Ficha, function (index, value) {
                                    $.each($scope.Programa1, function (index1, value1) {
                                        if ($scope.Ficha[index].IdPrograma == value1.IdPrograma) {
                                            $scope.Ficha[index].IdPrograma = value1.NombrePrograma;
                                        }
                                    });
                                });
                            }
                        });
                        if (response.AmbientesOtraArea == true) {

                            bootbox.dialog({
                                title: "Información",
                                message: "No tiene ambientes de su área disponibles en ese horario, los que salen a continuación pertenecen a otras coordinaciones y debe esperar confirmación",
                                buttons: {
                                    success: {
                                        label: "Cerrar",
                                        className: "btn-primary",
                                    }
                                }
                            });

                        }

                        $("#DatosT").css("display", "block");

                    }
                });
            }
            //----------------------------------------------------------------------------------------------------

            //Disponiblidad instructor, ambiente y ficha para programar tranversal en contra jornada---------------
            $scope.DisponibilidadTransversalCJ = function () {
                $scope.Transversal.Transversal = true;
                $scope.Programacion.HoraInicio = $("#HoraInicioTC").val();
                $scope.Programacion.HoraFin = $("#HoraFinTC").val();

                if ($scope.Programacion.FechaInicio == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe ingresar una fecha incio",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.Programacion.FechaFin == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe ingresar una fecha fin",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.Programacion.HoraInicio == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe ingresar una hora incio",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.Programacion.HoraFin == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe ingresar una hora fin",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.Sede.IdSede == null) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe seleccionar una Sede",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.Programa.IdPrograma == null) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe seleccionar un programa",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }

                if ($scope.Programacion.Lunes == false && $scope.Programacion.Martes == false && $scope.Programacion.Miercoles == false && $scope.Programacion.Jueves == false && $scope.Programacion.Viernes == false &&
                     $scope.SabadoDomingo.Sabado == false && $scope.SabadoDomingo.Domingo == false) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe seleccionar al menos un día",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }

                if ($scope.Programacion.HoraInicio >= $scope.Programacion.HoraFin) {
                    bootbox.dialog({
                        title: "Información",
                        message: "La hora final debe ser mayor a la hora inicial",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                var x = $("#FechaInicio").val().split('/');
                var y = $("#FechaFin").val().split('/');

                var StartDate = x[1] + "-" + x[2] + "-" + x[0];
                var EndDate = y[1] + "-" + y[2] + "-" + y[0];
                if (Date.parse(StartDate) > Date.parse(EndDate)) {
                    bootbox.dialog({
                        title: "Información",
                        message: "La fecha final debe ser mayor a la fecha inicial",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }

                //$scope.ValidarCamposProgramacion();

                $scope.Programacion.HoraInicio = $("#HoraInicioTC").val();
                $scope.Programacion.HoraFin = $("#HoraFinTC").val();
                if ($scope.Transversal.Transversal) {

                }
                $scope.Programacion.DiaSemana = $scope.DiasSemana.dia;
                ProgramacionService.DisponibilidadTransversalCJ($scope.Transversal.Transversal, $rootScope.globals.currentUser.idpersona, $scope.Programacion, $scope.Sede.IdSede, $scope.Programa.IdPrograma, $scope.SabadoDomingo, function (response) {
                    if (response.success == true) {
                        $scope.Ficha = response.ficha;
                        $scope.Ambiente = response.ambiente;
                        $scope.Instructor = response.instructor;
                        $("#guardarCJ").css("display", "inline-block");
                        ProgramacionService.ConsultarProgramaxCoordinacion($rootScope.globals.currentUser.idpersona, function (response) {
                            if (response.success == true) {
                                $scope.Programa1 = response.programa;

                                $.each($scope.Ficha, function (index, value) {
                                    $.each($scope.Programa1, function (index1, value1) {
                                        if ($scope.Ficha[index].IdPrograma == value1.IdPrograma) {
                                            $scope.Ficha[index].IdPrograma = value1.NombrePrograma;
                                        }
                                    });
                                });
                            }
                        });

                        ProgramacionService.ConsultarCompetenciasxPrograma($rootScope.ProgramaTransversal, function (response) {
                            if (response.success == true) {
                                $.each(response.competencia, function (index, value) {
                                    var nombreCompetencia = value.Nombre.split(' ');
                                    value.Codigo = value.Codigo + " " + nombreCompetencia[0];
                                });
                                $scope.Competencia = response.competencia;
                            }
                        });


                        if (response.AmbientesOtraArea == true) {

                            bootbox.dialog({
                                title: "Información",
                                message: "No tiene ambientes de su área disponibles en ese horario, los que salen a continuación pertenecen a otras coordinaciones y debe esperar confirmación",
                                buttons: {
                                    success: {
                                        label: "Cerrar",
                                        className: "btn-primary",
                                    }
                                }
                            });

                        }

                        $("#DatosTC").css("display", "block");

                    }
                });
            }

            //------Disponibilidad de instructor, ambiente y ficha de lunes a viernes----------------------------
            $scope.Disponibilidad = function () {
                $scope.Transversal.Transversal = false;
                $scope.Programacion.HoraInicio = $("#HoraInicio").val();
                $scope.Programacion.HoraFin = $("#HoraFin").val();

                if ($scope.Programacion.FechaInicio == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe ingresar una fecha incio",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.Programacion.FechaFin == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe ingresar una fecha fin",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.Programacion.HoraInicio == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe ingresar una hora incio",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.Programacion.HoraFin == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe ingresar una hora fin",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.Sede.IdSede == null) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe seleccionar una Sede",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.Programa.IdPrograma == null) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe seleccionar un programa",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }

                if ($scope.Programacion.Lunes == false && $scope.Programacion.Martes == false && $scope.Programacion.Miercoles == false && $scope.Programacion.Jueves == false && $scope.Programacion.Viernes == false &&
                     $scope.SabadoDomingo.Sabado == false && $scope.SabadoDomingo.Domingo == false) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe seleccionar al menos un día",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }

                if ($scope.Programacion.HoraInicio >= $scope.Programacion.HoraFin) {
                    bootbox.dialog({
                        title: "Información",
                        message: "La hora final debe ser mayor a la hora inicial",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                var x = $("#FechaInicio").val().split('/');
                var y = $("#FechaFin").val().split('/');

                var StartDate = x[1] + "-" + x[2] + "-" + x[0];
                var EndDate = y[1] + "-" + y[2] + "-" + y[0];
                if (Date.parse(StartDate) > Date.parse(EndDate)) {
                    bootbox.dialog({
                        title: "Información",
                        message: "La fecha final debe ser mayor a la fecha inicial",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                $scope.Programacion.HoraInicio = $("#HoraInicio").val();
                $scope.Programacion.HoraFin = $("#HoraFin").val();
                if ($scope.Transversal.Transversal) {

                }

                $scope.Jornada.Manana = false;
                $scope.Jornada.Tarde = false;
                $scope.Jornada.Noche = false;

                ProgramacionService.Disponibilidad($scope.Transversal.Transversal, $rootScope.globals.currentUser.idpersona, $scope.Programacion, $scope.Sede.IdSede, $scope.Programa.IdPrograma, $scope.SabadoDomingo, $scope.ObjetoSedes.Colegio, $scope.Jornada, function (response) {
                    if (response.success == true) {
                        $scope.Ficha = response.ficha;
                        $scope.Ambiente = response.ambiente;
                        $scope.Instructor = response.instructor;
                        $("#guardarTecnica").css("display", "inline-block");
                        $.each($scope.Ambiente, function (index, value) {
                            $.each($scope.Sede, function (index1, value1) {
                                if (value1.TipoSede == 2 && value1.IdSede == $scope.Sede.IdSede) {
                                    $("#Empresa").css("display", "block");
                                } else {
                                    $("#Empresa").css("display", "none");
                                }

                                if ($scope.Ambiente[index].IdSede == value1.IdSede) {
                                    $scope.Ambiente[index].Numero = $scope.Ambiente[index].Numero + " - " + value1.Nombre_Sede;

                                }
                            });
                        });
                        if (response.AmbientesOtraArea == true) {

                            bootbox.dialog({
                                title: "Información",
                                message: "No tiene ambientes de su área disponibles en ese horario, los que salen a continuación pertenecen a otras coordinaciones y debe esperar confirmación",
                                buttons: {
                                    success: {
                                        label: "Cerrar",
                                        className: "btn-primary",
                                    }
                                }
                            });

                        }

                        $("#Datos").css("display", "block");

                        $.each($scope.Sede, function (index1, value1) {
                            if ($scope.Sede.IdSede == value1.IdSede) {

                                if (value1.TipoSede == 2) {
                                    $("#Empresa").css("display", "block");
                                }
                            }
                        });


                    }
                });
            }
            //---------------------------------------------------------------------------------------------------

            //------Disponibilidad de instructor, ambiente y ficha de lunes a viernes con validaciones de jornada-
            $scope.DisponibilidadxJornada = function () {
                $scope.Transversal.Transversal = false;
                $scope.Programacion.HoraInicio = $("#HoraInicioJornada").val();
                $scope.Programacion.HoraFin = $("#HoraFinJornada").val();

                if ($scope.Programacion.FechaInicio == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe ingresar una fecha incio",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.Programacion.FechaFin == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe ingresar una fecha fin",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.Programacion.HoraInicio == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe ingresar una hora incio",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.Programacion.HoraFin == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe ingresar una hora fin",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.Sede.IdSede == null) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe seleccionar una Sede",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.Programa.IdPrograma == null) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe seleccionar un programa",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }

                if ($scope.Programacion.Lunes == false && $scope.Programacion.Martes == false && $scope.Programacion.Miercoles == false && $scope.Programacion.Jueves == false && $scope.Programacion.Viernes == false &&
                     $scope.SabadoDomingo.Sabado == false && $scope.SabadoDomingo.Domingo == false) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe seleccionar al menos un día",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }

                if ($scope.Programacion.HoraInicio >= $scope.Programacion.HoraFin) {
                    bootbox.dialog({
                        title: "Información",
                        message: "La hora final debe ser mayor a la hora inicial",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                var x = $("#FechaInicioJornada").val().split('/');
                var y = $("#FechaFinJornada").val().split('/');

                var StartDate = x[1] + "-" + x[2] + "-" + x[0];
                var EndDate = y[1] + "-" + y[2] + "-" + y[0];
                if (Date.parse(StartDate) > Date.parse(EndDate)) {
                    bootbox.dialog({
                        title: "Información",
                        message: "La fecha final debe ser mayor a la fecha inicial",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }

                if ($scope.Jornada.Manana == false && $scope.Jornada.Tarde == false && $scope.Jornada.Noche == false) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe seleccionar una jornada",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }


                if (($scope.Programacion.HoraInicio < "06:00" || $scope.Programacion.HoraInicio >= "12:00") && $scope.Jornada.Manana == true) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Seleccionó jornada mañana, las horas inicio y final deben estar en el rango de 6 a 12",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }


                if (($scope.Programacion.HoraInicio < "12:00" || $scope.Programacion.HoraInicio >= "18:00") && $scope.Jornada.Tarde == true) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Seleccionó jornada tarde, las horas inicio y final deben estar en el rango de 12 a 18",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }

                if (($scope.Programacion.HoraInicio < "18:00" || $scope.Programacion.HoraInicio > "22:00") && $scope.Jornada.Noche == true) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Seleccionó jornada noche, las horas inicio y final deben estar en el rango de 18 a 22",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }

                ProgramacionService.Disponibilidad($scope.Transversal.Transversal, $rootScope.globals.currentUser.idpersona, $scope.Programacion, $scope.Sede.IdSede, $scope.Programa.IdPrograma, $scope.SabadoDomingo, $scope.ObjetoSedes.Colegio, $scope.Jornada, function (response) {
                    if (response.success == true) {
                        $scope.Ficha = response.ficha;
                        $scope.Ambiente = response.ambiente;
                        $scope.Instructor = response.instructor;

                        var candidadFichas = $scope.Ficha.length;
                        if (candidadFichas == 0) {

                            bootbox.dialog({
                                title: "Información",
                                message: "No tiene fichas disponibles, cambie la jornada seleccionada o las fechas de cosnulta.",
                                buttons: {
                                    success: {
                                        label: "Cerrar",
                                        className: "btn-primary",
                                    }
                                }
                            });
                            return;
                        }

                        $("#guardarTecnicaJornada").css("display", "inline-block");
                        //$("#guardarTecnicaJornada").css("display", "block");
                        $.each($scope.Ambiente, function (index, value) {
                            $.each($scope.Sede, function (index1, value1) {
                                if (value1.TipoSede == 2 && value1.IdSede == $scope.Sede.IdSede) {
                                    $("#EmpresaJornada").css("display", "block");
                                } else {
                                    $("#EmpresaJornada").css("display", "none");
                                }

                                if ($scope.Ambiente[index].IdSede == value1.IdSede) {
                                    $scope.Ambiente[index].Numero = $scope.Ambiente[index].Numero + " - " + value1.Nombre_Sede;

                                }
                            });
                        });
                        if (response.AmbientesOtraArea == true) {

                            bootbox.dialog({
                                title: "Información",
                                message: "No tiene ambientes de su área disponibles en ese horario, los que salen a continuación pertenecen a otras coordinaciones y debe esperar confirmación",
                                buttons: {
                                    success: {
                                        label: "Cerrar",
                                        className: "btn-primary",
                                    }
                                }
                            });

                        }

                        $("#DatosJornada").css("display", "block");

                        $.each($scope.Sede, function (index1, value1) {
                            if ($scope.Sede.IdSede == value1.IdSede) {

                                if (value1.TipoSede == 2) {
                                    $("#EmpresaJornada").css("display", "block");
                                }
                            }
                        });


                    }
                });
            }
            //---------------------------------------------------------------------------------------------------

            //Función para consultar la disponibilidad en un rango de fechas de los instructores transversales
            $scope.DisponibilidadTrasversal = function () {

                var ProgramacionSeleccionada = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });

                $scope.ProgramacionSeleccionada1.HoraInicio = $("#HoraInicio2").val();
                $scope.ProgramacionSeleccionada1.HoraFin = $("#HoraFin2").val();
                if ($scope.ProgramacionSeleccionada1.HoraInicio >= $scope.ProgramacionSeleccionada1.HoraFin) {
                    bootbox.dialog({
                        title: "Información",
                        message: "La hora final debe ser mayor a la hora inicial",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                if ($scope.ProgramacionSeleccionada1.HoraInicio < ProgramacionSeleccionada[0].HoraInicio || $scope.ProgramacionSeleccionada1.HoraFin > ProgramacionSeleccionada[0].HoraFin) {
                    bootbox.dialog({
                        title: "Información",
                        message: "El rango de horas disponible para programar es entre: " + ProgramacionSeleccionada[0].HoraInicio +" - "+ ProgramacionSeleccionada[0].HoraFin,
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                $scope.ProgramacionSeleccionada1.HoraInicio = $("#HoraInicio2").val();
                $scope.ProgramacionSeleccionada1.HoraFin = $("#HoraFin2").val();

                var hora1 = (ProgramacionSeleccionada[0].HoraInicio.toString());
                var hora2 = (ProgramacionSeleccionada[0].HoraFin.toString());
                //var hora3 = ($scope.ProgramacionSeleccionada1.HoraInicio.toString());
                //var hora4 = ($scope.ProgramacionSeleccionada1.HoraFin.toString());
                $scope.ProgramacionSeleccionada1.ProgramacionPrincipal = ProgramacionSeleccionada[0].Id;
                $scope.ProgramacionSeleccionada1.DiaSemana = $scope.DiasSemana.dia;

                if ($scope.ProgramacionSeleccionada1.Lunes == false && $scope.ProgramacionSeleccionada1.Martes == false && $scope.ProgramacionSeleccionada1.Miercoles == false &&
                    $scope.ProgramacionSeleccionada1.Jueves == false && $scope.ProgramacionSeleccionada1.Viernes == false && $scope.SabadoDomingo.Sabado == false && $scope.SabadoDomingo.Domingo == false) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe seleccionar por lo menos un día",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                //NO BORRAR ---- NO BORRAR----------
                //if ($scope.Resultado.IdResultado == undefined) {
                //    bootbox.dialog({
                //        title: "Información",
                //        message: "Debe seleccionar un resultado de aprendizaje",
                //        buttons: {
                //            success: {
                //                label: "Cerrar",
                //                className: "btn-primary",
                //            }
                //        }
                //    });
                //    return;
                //}
                if ($scope.Competencia.IdCompetencia == undefined) {
                    
                        bootbox.dialog({
                            title: "Información",
                            message: "Debe seleccionar una competencia",
                            buttons: {
                                success: {
                                    label: "Cerrar",
                                    className: "btn-primary",
                                }
                            }
                        });

                        return;
                    
                }
                //if ((hora3 >= hora1 && hora3 <= hora2) && (hora4 <= hora2 && hora4 > hora1)) {

                ProgramacionService.DisponibilidadTrasversal($rootScope.globals.currentUser.idpersona, $scope.ProgramacionSeleccionada1, $scope.SabadoDomingo, function (response) {
                    if (response.success == true) {
                        $scope.Instructor1 = response.datos;
                        $("#Datos2").css("display", "block");
                        $("#guardartraslape").css("display", "inline-block");
                    }
                });

                //} else {
                //    bootbox.dialog({
                //        title: "Información",
                //        message: "La hora inicial debe estar entre el siguiente rango " + ProgramacionSeleccionada[0].HoraInicio + " y " + ProgramacionSeleccionada[0].HoraFin,
                //        buttons: {
                //            success: {
                //                label: "Cerrar",
                //                className: "btn-primary",
                //            }
                //        }
                //    });
                //}

                //if ((hora3 >= hora1 && hora4 <= hora2) && (hora4 > hora1 && hora4 <= hora2) && ((hora4) - (hora3) == 2)) {

                //    ProgramacionService.DisponibilidadTrasversal($rootScope.globals.currentUser.idpersona, $scope.ProgramacionSeleccionada1, function (response) {
                //        if (response.success == true) {
                //            $scope.Instructor1 = response.datos;
                //            $("#Datos2").css("display", "block");
                //        }
                //    });

                //} else {

                //    bootbox.dialog({
                //        title: "Información",
                //        message: "Debe seleccionar dos horas entre el siguiente rango " + ProgramacionSeleccionada[0].HoraInicio + " y " + ProgramacionSeleccionada[0].HoraFin,
                //        buttons: {
                //            success: {
                //                label: "Cerrar",
                //                className: "btn-primary",
                //            }
                //        }
                //    });

                //}


            };
            //--------------------------------------------------------------------------------------------------

            //Función para consultar las competencias de cada programa-------------------------------------------
            $scope.ConsultarCompetenciasxPrograma = function (Programa) {
                ProgramacionService.ConsultarCompetenciasxPrograma(Programa.IdPrograma, function (response) {
                    if (response.success == true) {

                        $.each(response.competencia, function (index, value) {
                            var nombreCompetencia = value.Nombre.split(' ');
                            value.Codigo = value.Codigo + " " + nombreCompetencia[0];
                        });

                        $scope.Competencia = response.competencia;
                    }
                });
            };
            //---------------------------------------------------------------------------------------------------

            //Guardar programación transversal-------------------------------------------------------------------
            $scope.GuardarTransversal = function () {


                $scope.Programacion.IdFicha = $scope.Ficha.IdFicha;
                $scope.Programacion.IdAmbiente = $scope.Ambiente.IdAmbiente;
                $scope.Programacion.IdInstructor = $scope.Instructor.IdInstructor;
                $scope.Programacion.IdResultado = $scope.Resultado.IdResultado;
                $scope.Programacion.HoraInicio = $("#HoraInicioT").val();
                $scope.Programacion.HoraFin = $("#HoraFinT").val();
                $scope.Programacion.DiaSemana = $scope.DiasSemana.dia;
                if ($scope.Sabado.sabado == false) {


                    $scope.Programacion.Jornada = 1;
                    $scope.Programacion.Transversal = true;
                    ProgramacionService.GuardarProgramacion($scope.Programacion, function (response) {
                        if (response.success == true) {

                            if (response.mensaje == null) {
                                $("#ModalProgramacion").modal("hide");
                                location.reload();
                            } else {
                                bootbox.dialog({
                                    title: "Información",
                                    message: response.mensaje,
                                    buttons: {
                                        success: {
                                            label: "Cerrar",
                                            className: "btn-primary",
                                        }
                                    }
                                });
                            }
                        }
                    });

                } else {
                    var contador = 0;
                    var x = $("#FechaInicioT").val().split('/');
                    var y = $("#FechaFinT").val().split('/');

                    var start = new Date(x[0] + "-" + x[1] + "-" + x[2]);
                    var end = new Date(y[0] + "-" + y[1] + "-" + y[2]);

                    setTimeout(function () {
                        while (start <= end) {
                            start.setDate(start.getDate() + 1);
                            var day = moment(start).day();
                            if (day == 6 || day == 7) {
                                if (contador > 0) {
                                    break;
                                }
                                //return;
                                contador++;
                            }
                        }
                        if (contador > 0) {
                            $("#ModalSabado").modal("show");
                        } else {
                            bootbox.dialog({
                                title: "Información",
                                message: "El rango de fechas seleccionado no contiene por lo menos un fin de semana",
                                buttons: {
                                    success: {
                                        label: "Cerrar",
                                        className: "btn-primary",
                                    }
                                }
                            });
                        }
                    }, 500);

                }
            };
            //---------------------------------------------------------------------------------------------------

            //Función para seleccionar varios resultados de aprendizaje------------------------------------------
            $scope.selectedValues = [];

            $scope.$watch('selected', function (nowSelected) {
                $scope.selectedValues = [];

                if (!nowSelected) {
                    // here we've initialized selected already
                    // but sometimes that's not the case
                    // then we get null or undefined
                    return;
                }
                angular.forEach(nowSelected, function (val) {
                    $scope.selectedValues.push(val.Id);
                });
            });
            //---------------------------------------------------------------------------------------------------

            //Guardar la programación los días en semana--------------------------------------------------------- 
            $scope.Guardar = function () {

                var contador = 0;

                $.each($scope.Sede, function (index1, value1) {
                    if ($scope.Sede.IdSede == value1.IdSede) {
                        if (value1.Empresa == true) {
                            contador = 1;
                        }
                    }
                });

                if ($scope.Ficha.IdFicha == null || $scope.Ambiente.IdAmbiente == null || $scope.Instructor.IdInstructor == null || (contador == 1 && $scope.Programacion.NombreEmpresa == "")) {

                    bootbox.dialog({
                        title: "Información",
                        message: "Debe diligenciar todos los campos",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }

                $scope.Programacion.IdFicha = $scope.Ficha.IdFicha;
                $scope.Programacion.IdAmbiente = $scope.Ambiente.IdAmbiente;
                $scope.Programacion.IdInstructor = $scope.Instructor.IdInstructor;
                $scope.ListaProgramacion = [];

                if ($scope.Sabado.sabado == false) {

                    if ($scope.SabadoDomingo.Sabado == false && $scope.SabadoDomingo.Domingo == false) {
                        $scope.Programacion.Jornada = 1;
                    }
                    if ($scope.SabadoDomingo.Sabado == true) {
                        $scope.Programacion.Jornada = 2;
                    }
                    if ($scope.SabadoDomingo.Domingo == true) {
                        $scope.Programacion.Jornada = 3;
                    }
                    $scope.Programacion.Transversal = false;
                    $scope.Programacion.ProgramacionPrincipal = null;
                    $scope.Programacion.NombreEmpresa = $scope.Programacion.NombreEmpresa.toString().toUpperCase();

                    //Para volver a programar todos los resultados activar de nuevo el each y cambiar el idResultado por value 
                    //$.each($scope.selectedValues, function (index, value) {
                    $scope.ListaProgramacion.push({
                        IdFicha: $scope.Programacion.IdFicha, IdResultado: $scope.Resultado[0].IdResultado, IdAmbiente: $scope.Programacion.IdAmbiente,
                        IdInstructor: $scope.Programacion.IdInstructor, FechaInicio: $scope.Programacion.FechaInicio,
                        FechaFin: $scope.Programacion.FechaFin, HoraInicio: $scope.Programacion.HoraInicio, HoraFin: $scope.Programacion.HoraFin,
                        Jornada: $scope.Programacion.Jornada, DiaSemana: $scope.Programacion.DiaSemana, Lunes: $scope.Programacion.Lunes,
                        Martes: $scope.Programacion.Martes, Miercoles: $scope.Programacion.Miercoles, Jueves: $scope.Programacion.Jueves,
                        Viernes: $scope.Programacion.Viernes, Transversal: $scope.Programacion.Transversal, NombreEmpresa: $scope.Programacion.NombreEmpresa
                    })
                    //})


                    ProgramacionService.GuardarProgramacion($scope.ListaProgramacion, function (response) {
                        if (response.success == true) {
                            if (response.mensaje == null) {
                                $("#ModalProgramacion").modal("hide");
                                location.reload();

                            } else {
                                bootbox.dialog({
                                    title: "Información",
                                    message: response.mensaje,
                                    buttons: {
                                        success: {
                                            label: "Cerrar",
                                            className: "btn-primary",
                                        }
                                    }
                                });
                            }

                        }
                    });


                } else {
                    var contador = 0;
                    var x = $("#FechaInicio").val().split('/');
                    var y = $("#FechaFin").val().split('/');

                    var start = new Date(x[0] + "-" + x[1] + "-" + x[2]);
                    var end = new Date(y[0] + "-" + y[1] + "-" + y[2]);

                    //Aqui se llena el input resultado, con los codigos de resultados seleccionados previamente en la modal de programación 
                    var Resultados = "";

                    $.each($scope.Resultado, function (index, value) {

                        $.each($scope.selectedValues, function (index, value1) {
                            if (value.IdResultado == value1) {
                                Resultados += value.Codigo + ", ";
                            }
                        })
                    });

                    $scope.Resultado.IdResultado = Resultados;

                    //Validación de que el rango de fechas seleccionado tenga por lo menos un fin de semana incluido  
                    setTimeout(function () {
                        while (start <= end) {
                            start.setDate(start.getDate() + 1);
                            var day = moment(start).day();
                            if (day == 6 || day == 7) {
                                if (contador > 0) {
                                    break;
                                }
                                //return;
                                contador++;
                            }
                        }
                        if (contador > 0) {


                            $("#ModalSabado").modal("show");
                            $("#ModalProgramacionxJornada").modal("hide");


                        } else {
                            bootbox.dialog({
                                title: "Información",
                                message: "El rango de fechas seleccionado no contiene por lo menos un fin de semana",
                                buttons: {
                                    success: {
                                        label: "Cerrar",
                                        className: "btn-primary",
                                    }
                                }
                            });
                        }
                    }, 500);

                }


            };
            //---------------------------------------------------------------------------------------------------

            //Guadrar progrmación transversal contra jornada-----------------------------------------------------
            $scope.GuardarTransversalCJ = function () {

                $scope.Programacion.IdFicha = $scope.Ficha.IdFicha
                $scope.Programacion.IdAmbiente = $scope.Ambiente.IdAmbiente
                $scope.Programacion.IdInstructor = $scope.Instructor.IdInstructor
                $scope.Programacion.IdResultado = $scope.Resultado[0].IdResultado;

                if ($scope.Programacion.IdFicha == "" || $scope.Programacion.IdAmbiente == "" || $scope.Programacion.IdInstructor == "" || $scope.Programacion.IdResultado == undefined) {

                    bootbox.dialog({
                        title: "Información",
                        message: "Debe diligenciar todos los campos",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                $scope.Programacion.ProgramacionPrincipal = null;
                $scope.Programacion.HoraInicio = $("#HoraInicioTC").val();
                $scope.Programacion.HoraFin = $("#HoraFinTC").val();
                $scope.Programacion.DiaSemana = $scope.DiasSemana.dia;
                if ($scope.SabadoDomingo.Sabado == true) {
                    $scope.Programacion.Jornada = 2;
                } else if ($scope.SabadoDomingo.Domingo == true) {
                    $scope.Programacion.Jornada = 3;
                } else {
                    $scope.Programacion.Jornada = 1;
                }
                if ($scope.Sabado.sabado == false) {

                    $scope.Programacion.Transversal = true;

                    $scope.ListaProgramacion = [];

                    $scope.ListaProgramacion.push($scope.Programacion);

                    ProgramacionService.GuardarProgramacion($scope.ListaProgramacion, function (response) {
                        if (response.success == true) {

                            if (response.mensaje == null) {
                                $("#ModalProgramacion").modal("hide");
                                location.reload();
                            } else {
                                bootbox.dialog({
                                    title: "Información",
                                    message: response.mensaje,
                                    buttons: {
                                        success: {
                                            label: "Cerrar",
                                            className: "btn-primary",
                                        }
                                    }
                                });
                            }
                        }
                    });

                } else {
                    var contador = 0;
                    var x = $("#FechaInicioT").val().split('/');
                    var y = $("#FechaFinT").val().split('/');

                    var start = new Date(x[0] + "-" + x[1] + "-" + x[2]);
                    var end = new Date(y[0] + "-" + y[1] + "-" + y[2]);

                    setTimeout(function () {
                        while (start <= end) {
                            start.setDate(start.getDate() + 1);
                            var day = moment(start).day();
                            if (day == 6 || day == 7) {
                                if (contador > 0) {
                                    break;
                                }
                                //return;
                                contador++;
                            }
                        }
                        if (contador > 0) {
                            $("#ModalSabado").modal("show");
                        } else {
                            bootbox.dialog({
                                title: "Información",
                                message: "El rango de fechas seleccionado no contiene por lo menos un fin de semana",
                                buttons: {
                                    success: {
                                        label: "Cerrar",
                                        className: "btn-primary",
                                    }
                                }
                            });
                        }
                    }, 500);

                }
            };
            //---------------------------------------------------------------------------------------------------


            //Guardar programación de los fines de semana -------------------------------------------------------

            $scope.Guardar1 = function () {

                $.each($scope.Ambiente1, function (index, value) {
                    if (value.IdAmbiente == $scope.Ambiente1.IdAmbiente) {
                        $scope.Programacion1.IdAmbiente = value.IdAmbiente
                    }
                });
                //$.each($scope.Instructor1, function (index, value) {
                //    if (value.IdInstructor == $scope.Instructor1.IdInstructor) {
                //        $scope.Programacion1.IdInstructor = value.IdInstructor
                //    }
                //});

                $scope.Programacion.Jornada = 1;
                $scope.Programacion1.IdFicha = $scope.Programacion.IdFicha;
                $scope.Programacion1.FechaInicio = $scope.Programacion.FechaInicio;
                $scope.Programacion1.FechaFin = $scope.Programacion.FechaFin;
                $scope.Programacion1.IdInstructor = $scope.Programacion.IdInstructor;
                $scope.Programacion1.IdResultado = $scope.Programacion.IdResultado;
                $scope.Programacion1.HoraInicio = $("#HoraInicio1").val();
                $scope.Programacion1.HoraFin = $("#HoraFin1").val();

                if ($scope.FinSemana.dia == 1) {

                    $scope.Programacion1.Jornada = 2;
                }

                if ($scope.FinSemana.dia == 2) {

                    $scope.Programacion1.Jornada = 3;
                }
                $scope.Programacion.Transversal = false;

                //Para volver a programar todos los resultados activar de nuevo el each y cambiar el idResultado por value 
                //$.each($scope.selectedValues, function (index, value) {

                $scope.ListaProgramacion.push({
                    IdFicha: $scope.Programacion.IdFicha, IdResultado: $scope.Resultado[0].IdResultado, IdAmbiente: $scope.Programacion.IdAmbiente,
                    IdInstructor: $scope.Programacion.IdInstructor, FechaInicio: $scope.Programacion.FechaInicio,
                    FechaFin: $scope.Programacion.FechaFin, HoraInicio: $scope.Programacion.HoraInicio, HoraFin: $scope.Programacion.HoraFin,
                    Jornada: $scope.Programacion.Jornada, DiaSemana: $scope.Programacion.DiaSemana, Lunes: $scope.Programacion.Lunes,
                    Martes: $scope.Programacion.Martes, Miercoles: $scope.Programacion.Miercoles, Jueves: $scope.Programacion.Jueves,
                    Viernes: $scope.Programacion.Viernes, Transversal: $scope.Programacion.Transversal, NombreEmpresa: $scope.Programacion.NombreEmpresa
                    //})

                })

                ProgramacionService.GuardarProgramacion($scope.ListaProgramacion, function (response) {
                    if (response.success == true) {
                        $("#ModalProgramacion").modal("hide");
                        $scope.Programacion1.Transversal = false;

                        if ($scope.Programacion1.Jornada == "" || $scope.Programacion1.IdAmbiente == undefined || $scope.Programacion1.IdAmbiente == "" || $scope.Programacion1.HoraInicio == "" || $scope.Programacion1.HoraFin == "") {

                            bootbox.dialog({
                                title: "Información",
                                message: "Debe diligenciar todos los campos",
                                buttons: {
                                    success: {
                                        label: "Cerrar",
                                        className: "btn-primary",
                                    }
                                }
                            });
                            return;

                        } else {

                            $scope.ListaProgramacionSabado = [];

                            //Para volver a programar todos los resultados activar de nuevo el each y cambiar el idResultado por value 
                            //$.each($scope.selectedValues, function (index, value) {

                            $scope.ListaProgramacionSabado.push({
                                IdFicha: $scope.Programacion1.IdFicha, IdResultado: $scope.Resultado[0].IdResultado, IdAmbiente: $scope.Programacion1.IdAmbiente,
                                IdInstructor: $scope.Programacion1.IdInstructor, FechaInicio: $scope.Programacion1.FechaInicio,
                                FechaFin: $scope.Programacion1.FechaFin, HoraInicio: $scope.Programacion1.HoraInicio, HoraFin: $scope.Programacion1.HoraFin,
                                Jornada: $scope.Programacion1.Jornada, DiaSemana: $scope.Programacion1.DiaSemana, Lunes: null,
                                Martes: null, Miercoles: null, Jueves: null,
                                Viernes: null, Transversal: $scope.Programacion.Transversal, NombreEmpresa: $scope.Programacion.NombreEmpresa
                                //})

                            })

                            ProgramacionService.GuardarProgramacion($scope.ListaProgramacionSabado, function (response) {
                                if (response.success == true) {

                                    if (response.mensaje == null) {
                                        $("#ModalProgramacion").modal("hide");
                                        $("#ModalSabado").modal("hide");
                                        location.reload();
                                    } else {
                                        bootbox.dialog({
                                            title: "Información",
                                            message: response.mensaje,
                                            buttons: {
                                                success: {
                                                    label: "Cerrar",
                                                    className: "btn-primary",
                                                }
                                            }
                                        });
                                    }

                                }
                            });
                        }
                    }
                });


            }
            //---------------------------------------------------------------------------------------------------

            //Función para consultar los resultados de aprendizaje por competencia ---------------------------------
            $('#Resultados_apredizaje').select2();
            $('#Resultados_apredizajeJornada').select2();
            $scope.ConsultarResultados = function (IdCompetencia) {

                ProgramacionService.ConsultarResultados(IdCompetencia, function (response) {
                    if (response.success == true) {
                        $('#resultados').css('display', 'block');

                        var opciones = [];

                        $.each(response.datos, function (index, value) {
                            var nombreResultado = value.Resultado.split(' ');
                            value.Codigo = value.Codigo + " " + nombreResultado[0];
                            //$("#Resultados_apredizaje").append($('<option>',
                            //   {
                            //      value: value.IdResultado,
                            //      text:value.Codigo
                            //  }));
                        });
                        $scope.Resultado = response.datos;
                    }
                });
            }
            //------------------------------------------------------------------------------------------------------

            //Metodo para consultar los resultados de aprendizaje por competencia----------------------------------- 
            $scope.ConsultarResultados1 = function (IdCompetencia) {
                $.each($scope.Competencia1, function (index, value) {
                    if (value.IdCompetencia == IdCompetencia) {
                        ProgramacionService.ConsultarResultados(value, function (response) {

                            if (response.success == true) {
                                $('#resultados1').css('display', 'block');
                                $scope.Resultado1 = response.datos;
                            }
                        });
                    }

                });
            }
            //------------------------------------------------------------------------------------------------------

            //Metodo para consultar los ambientes de cada sede------------------------------------------------------
            $scope.ConsultarAmbientesxSede = function (IdSede) {

                ProgramacionService.ConsultarAmbientesxSede(IdSede, function (response) {

                    if (response.success == true) {

                        $scope.AmbientexSede = response.sedes;
                        $('#ambientes').css('display', 'block');
                    }
                });
            };
            //------------------------------------------------------------------------------------------------------


            //-------------Inicialización del calendario------------------------------------------------------------ 
            var date = new Date();
            var d = date.getDate();
            var m = date.getMonth();
            var y = date.getFullYear();

            $scope.events = [];
            $('#calendar').fullCalendar({
                header: {
                    left: 'title',
                    center: 'agendaDay,agendaWeek,month',
                    right: 'prev,next today'

                },
                theme: false,
                height: 700,

            });

            //---------------------------------------------------------------


            //-----------Función que filtra las programaciones por jornadas--------------------------------- 
            $scope.FiltroAmbiente = function (ambiente) {
                if (ambiente == "" || $scope.RangoHoras.Jornada == null) {

                    bootbox.dialog({
                        title: "Información",
                        message: "Seleccione un ambiente y un rango horario",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                } else {

                    ProgramacionService.FiltrarAmbiente(ambiente.IdAmbiente, $scope.RangoHoras.Jornada, function (response) {
                        $scope.Limpiar();
                        $.each(response.programacion, function (index, value) {
                            var Inicio = value.FechaInicio.split('-');
                            var Fin = value.FechaFin.split('-');
                            var HoraInicio = value.HoraInicio.split(':');
                            var HoraFin = value.HoraFin.split(':');
                            $scope.events.push({
                                title: ' Ficha:' + value.Ficha,
                                start: new Date(parseInt(Inicio[0]), parseInt(Inicio[1]) - 1, parseInt(Inicio[2].substring(0, 2)), parseInt(HoraInicio[0]), parseInt(HoraInicio[1])),
                                end: new Date(parseInt(Fin[0]), parseInt(Fin[1]) - 1, parseInt(Fin[2].substring(0, 2)), parseInt(HoraFin[0]), parseInt(HoraFin[1])),
                                allDay: false,
                                color: value.Color,
                                tip: "PROGRAMA: " + value.Programa.toUpperCase() + " - " + "INSTRUCTOR:" + value.NombreInstructor.toUpperCase() + " - " + "RESULTADO:" + value.Competencia.toUpperCase()
                            });

                        });

                        var calendar = $('#calendar').fullCalendar({
                            header: {
                                left: 'title',
                                center: 'agendaDay,agendaWeek,month',
                                right: 'prev,next today'

                            },
                            height: 700,
                            theme: false,
                            ignoreTimezone: false,
                            events: $scope.events,
                            eventRender: function (event, element) {
                                element.attr('title', event.tip);
                            },
                            editable: false,
                            firstDay: 1, //  1(Monday) this can be changed to 0(Sunday) for the USA system
                            selectable: false,
                            defaultView: 'month',
                            axisFormat: 'H:mm',
                            columnFormat: {
                                month: 'ddd',    // Mon
                                week: 'ddd d', // Mon 7
                                day: 'dddd M/d',  // Monday 9/7
                                agendaDay: 'dddd d'
                            },
                            titleFormat: {
                                month: 'MMMM yyyy', // September 2009
                                week: "MMMM yyyy", // September 2009
                                day: 'MMMM yyyy'                  // Tuesday, Sep 8, 2009
                            },
                            allDaySlot: false,
                            selectHelper: true,
                            select: function (start, end, allDay) {
                                var title = prompt('Event Title:');
                                if (title) {
                                    calendar.fullCalendar('renderEvent',
                                        {
                                            title: title,
                                            start: start,
                                            end: end,
                                            allDay: allDay
                                        },
                                        true // make the event "stick"
                                    );
                                }
                                calendar.fullCalendar('unselect');
                            },
                            droppable: true, // this allows things to be dropped onto the calendar !!!
                            drop: function (date, allDay) { // this function is called when something is dropped

                                // retrieve the dropped element's stored Event Object
                                var originalEventObject = $(this).data('eventObject');

                                // we need to copy it, so that multiple events don't have a reference to the same object
                                var copiedEventObject = $.extend({}, originalEventObject);

                                // assign it the date that was reported
                                copiedEventObject.start = date;
                                copiedEventObject.allDay = allDay;

                                // render the event on the calendar
                                // the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
                                $('#calendar').fullCalendar('renderEvent', copiedEventObject, true);

                                // is the "remove after drop" checkbox checked?
                                if ($('#drop-remove').is(':checked')) {
                                    // if so, remove the element from the "Draggable Events" list
                                    $(this).remove();
                                }

                            },


                        });

                    });
                    $scope.Limpiar();
                    $("#ModalFiltro").modal("hide");
                    $("#divDerecho").css("margin-top", "5px");
                    $("#divDerecho1").css("margin-top", "5px");
                }
            };
            //----------------------------------------------------------------------------------------------

            //Función para poder visualizar todas las programaciones en una tabla--------------------------
            $scope.VerProgramaciones = function () {

                $("#panel2").show();
                $("#panel1").hide();
                $scope.DiasSemana.dia = 0;

            };
            //----------------------------------------------------------------------------------------------

            //Consulta de las programación por coordinador--------------------------------------------------
            if ($rootScope.globals.currentUser.tipousuario == 2) {
                $scope.Nombre = "Resultado";
                $("#transversal").show();
                ProgramacionService.ConsultarProgramacion($rootScope.globals.currentUser.idpersona, function (response) {
                    if (response.success == true) {
                        $scope.ListaCompleta = response.datos;
                        $scope.datalists = response.datos;
                        $.each($scope.datalists, function (index, value) {
                            $scope.datalists[index].FechaInicio = value.FechaInicio.toString().substring(0, 10);
                            $scope.datalists[index].FechaFin = value.FechaFin.toString().substring(0, 10);
                            $scope.datalists[index].HoraInicio = value.HoraInicio.toString().substring(0, 5);
                            $scope.datalists[index].HoraFin = value.HoraFin.toString().substring(0, 5);
                            //$scope.datalists[index].DiasProgramados = value.DiasProgramados.toString().substring(-1)
                        });
                    };
                });
            }
            //----------------------------------------------------------------------------------------------

            //Consulta de las programación por coordinador--------------------------------------------------
            $scope.ProgramacionTecnica = function () {
                $("#atras").hide();
                $("#dia").hide();
                $(".dia1").hide();
                $("#AnadirTransversales").show();
                $scope.Nombre = "Resultado";
                if ($rootScope.globals.currentUser.tipousuario == 2) {

                    $("#transversal").show();
                    ProgramacionService.ConsultarProgramacion($rootScope.globals.currentUser.idpersona, function (response) {
                        if (response.success == true) {

                            $scope.datalists = response.datos;
                            $.each($scope.datalists, function (index, value) {

                                $scope.datalists[index].FechaInicio = value.FechaInicio.toString().substring(0, 10);
                                $scope.datalists[index].FechaFin = value.FechaFin.toString().substring(0, 10);
                            }

                        )
                            $scope.DiasSemana.dia = 0;

                        };

                    });

                }
            }
            //----------------------------------------------------------------------------------------------

            //Consulta de las programación por coordinador los fines de semana-----------------------------
            $scope.ConsultarProgramacionSabados = function () {
                $("#atras").show();
                $("#dia").show();
                $(".dia1").show();
                if ($rootScope.globals.currentUser.tipousuario == 2) {

                    $("#transversal").show();
                    ProgramacionService.ConsultarProgramacionSabados($rootScope.globals.currentUser.idpersona, function (response) {
                        if (response.success == true) {

                            $scope.datalists = response.datos;
                            $.each($scope.datalists, function (index, value) {

                                $scope.datalists[index].FechaInicio = value.FechaInicio.toString().substring(0, 10);
                                $scope.datalists[index].FechaFin = value.FechaFin.toString().substring(0, 10);
                                $scope.datalists[index].HoraInicio = value.HoraInicio.toString().substring(0, 5);
                                $scope.datalists[index].HoraFin = value.HoraFin.toString().substring(0, 5);

                                $scope.datalists[index].DiaSemana = $scope.datalists[index].Jornada;
                                $scope.datalists[index].DiaSemana = $scope.datalists[index].Jornada;

                            }

                        )
                        };

                    });
                }
                $scope.DiasSemana.dia = 6;
            };
            //---------------------------------------------------------------------------------------------

            //Consulta de las programación transversales---------------------------------------------------
            $scope.ConsultarProgramacionTransversales = function () {
                $("#atras").show();
                $("#AnadirTransversales").hide();
                if ($rootScope.globals.currentUser.tipousuario == 2) {

                    $("#transversal").show();
                    ProgramacionService.ConsultarProgramacionTransversales($rootScope.globals.currentUser.idpersona, function (response) {
                        if (response.success == true) {
                            $scope.ListaCompleta = response.datos
                            $scope.datalists = response.datos;
                            $.each($scope.datalists, function (index, value) {

                                $scope.datalists[index].FechaInicio = value.FechaInicio.toString().substring(0, 10);
                                $scope.datalists[index].FechaFin = value.FechaFin.toString().substring(0, 10);
                                $scope.datalists[index].HoraInicio = value.HoraInicio.toString().substring(0, 5);
                                $scope.datalists[index].HoraFin = value.HoraFin.toString().substring(0, 5);
                                $scope.datalists[index].CodigoResultado = value.CodigoCompetencia;
                                $scope.datalists[index].Resultado = value.Competencia;
                                $("#dia").show();
                                $("#dia1").show();
                                $scope.Nombre = "Competencia";
                                //Variable para setear la paginación 
                                $scope.curPage = 0;
                            }
                        )
                        };
                    });
                }
            };
            //---------------------------------------------------------------------------------------------


            //Función para editar una programación---------------------------------------------------------  
            $scope.Modificar = function () {

                //$("#Datos").css("display", "none");
                //$("#ModalProgramacion").modal("show");
                $("#panel2").show();
                $("#panel1").hide()
                $("#ModalTipoProgramaciones").modal("hide");
                //$("#HoraInicio2").prop("disabled", true);
                //$("#HoraFin2").prop("disabled", true);

                ProgramacionService.ConsultarCompetenciasxPrograma($rootScope.ProgramaTransversal, function (response) {
                    if (response.success == true) {

                        $.each(response.competencia, function (index, value) {
                            var nombreCompetencia = value.Nombre.split(' ');
                            value.Codigo = value.Codigo + " " + nombreCompetencia[0];
                        });
                        $scope.Competencia = response.competencia;
                    }
                });
                var ProgramacionSeleccionada = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });

                var hora1 = "";
                var hora2 = "";
                $.each(ProgramacionSeleccionada, function (index, value) {
                    hora1 = ProgramacionSeleccionada[ProgramacionSeleccionada.length - 1].HoraInicio;
                    hora2 = ProgramacionSeleccionada[0].HoraFin;

                    if (ProgramacionSeleccionada[0].IdAmbiente != value.IdAmbiente || ProgramacionSeleccionada[0].IdFicha != value.IdFicha) {
                        bootbox.dialog({
                            title: "Información",
                            message: "Las programaciones seleccionadas deben tener el mismo ambiente y la misma ficha",
                            buttons: {
                                success: {
                                    label: "Cerrar",
                                    className: "btn-primary",
                                }
                            }
                        });
                        return;
                    }
                });

                //if (ProgramacionSeleccionada.length == 1) {

                $scope.ProgramacionSeleccionada1.Ambiente = ProgramacionSeleccionada[0].Ambiente;
                $scope.ProgramacionSeleccionada1.IdAmbiente = ProgramacionSeleccionada[0].IdAmbiente;
                $scope.ProgramacionSeleccionada1.Resultado = ProgramacionSeleccionada[0].Resultado;
                $scope.ProgramacionSeleccionada1.FechaInicio = ProgramacionSeleccionada[0].FechaInicio;
                $scope.ProgramacionSeleccionada1.FechaFin = ProgramacionSeleccionada[0].FechaFin;
                $scope.ProgramacionSeleccionada1.Ficha = ProgramacionSeleccionada[0].Ficha;
                $scope.ProgramacionSeleccionada1.Lunes = false;
                $scope.ProgramacionSeleccionada1.Martes = false;
                $scope.ProgramacionSeleccionada1.Miercoles = false;
                $scope.ProgramacionSeleccionada1.Jueves = false;
                $scope.ProgramacionSeleccionada1.Viernes = false;

                //Seteando el IdInstructor. 
                ProgramacionSeleccionada[0].IdInstructor = "";

                //$scope.ProgramacionSeleccionada1.HoraFin = ProgramacionSeleccionada[0].HoraFin;
                //$scope.ProgramacionSeleccionada1.HoraInicio = ProgramacionSeleccionada[0].HoraInicio;
                //var hora1 = (ProgramacionSeleccionada[0].HoraInicio.toString()).substring(0, 2);
                //var hora2 = (ProgramacionSeleccionada[0].HoraFin.toString()).substring(0, 2);
                ProgramacionService.ConsultarTransversales(ProgramacionSeleccionada[0].Id, function (response) {
                    if (response.success == true) {
                        if (response.resp == true) {
                            var array = response.dias.split(".");
                            var arrayOrdenado = array.sort();
                            $scope.cadena = "";
                            for (var i = 0; i < array.length; i++) {
                                $scope.cadena += array[i] + "<br/>"
                            }
                            bootbox.dialog({
                                title: "Información",
                                message: "Esta programación ya tiene transversales programadas los días <br/>" + $scope.cadena,
                                buttons: {
                                    success: {
                                        label: "Cerrar",
                                        className: "btn-primary",
                                    }
                                }
                            });
                        }
                        var DIAS = response.dias
                        $("#Lunes").show();
                        $("#Martes").show();
                        $("#Miercoles").show();
                        $("#Jueves").show();
                        $("#Viernes").show();
                        $("#Sabado").show();
                        $("#Domingo").show();

                        $scope.DiasSemana = [
                            { dia: 1, Nombre: "Lunes" },
                            { dia: 2, Nombre: "Martes" },
                            { dia: 3, Nombre: "Miércoles" },
                            { dia: 4, Nombre: "Jueves" },
                            { dia: 5, Nombre: "Viernes" },
                            { dia: 6, Nombre: "Sábado" },
                            { dia: 7, Nombre: "Domingo" },
                        ];

                        $scope.DiasSemana.dia = 0;

                        var ProgramacionAsocidada = response.ProgramacionPrincipal;
                        if (ProgramacionAsocidada.Lunes == null) {

                            $.each($scope.DiasSemana, function (index, value) {
                                if (value.Nombre == "Lunes") {
                                    $("#Lunes").hide();
                                    $scope.DiasSemana.splice(index, 1);

                                    return false;
                                }
                            });
                        }

                        if (ProgramacionAsocidada.Martes == null) {
                            $.each($scope.DiasSemana, function (index, value) {
                                if (value.Nombre == "Martes") {
                                    $("#Martes").hide();
                                    $scope.DiasSemana.splice(index, 1);
                                    return false;
                                }
                            });
                        }
                        if (ProgramacionAsocidada.Miercoles == null) {
                            $.each($scope.DiasSemana, function (index, value) {
                                if (value.Nombre == "Miércoles") {
                                    $("#Miercoles").hide();
                                    $scope.DiasSemana.splice(index, 1);
                                    return false;
                                }
                            });
                        }
                        if (ProgramacionAsocidada.Jueves == null) {
                            $.each($scope.DiasSemana, function (index, value) {
                                if (value.Nombre == "Jueves") {
                                    $("#Jueves").hide();
                                    $scope.DiasSemana.splice(index, 1);
                                    return false;
                                }
                            });
                        }
                        if (ProgramacionAsocidada.Viernes == null) {
                            $.each($scope.DiasSemana, function (index, value) {
                                if (value.Nombre == "Viernes") {
                                    $("#Viernes").hide();
                                    $scope.DiasSemana.splice(index, 1);
                                    return false;
                                }
                            });
                        }
                        if (ProgramacionAsocidada.Jornada != 2) {
                            $.each($scope.DiasSemana, function (index, value) {
                                if (value.Nombre == "Sábado") {
                                    $("#Sabado").hide();
                                    $scope.DiasSemana.splice(index, 1);
                                    return false;
                                }
                            });
                        }
                        if (ProgramacionAsocidada.Jornada != 3) {
                            $.each($scope.DiasSemana, function (index, value) {
                                if (value.Nombre == "Domingo") {
                                    $("#Domingo").hide();
                                    $scope.DiasSemana.splice(index, 1);
                                    return false;
                                }
                            });
                        }
                    }

                    Cortada = DIAS.split(",");
                    console.log(Cortada);
                    $.each(Cortada, function (index, value) {
                        if (value == "Lunes") {
                            $("#Lunes").hide();
                        }
                        if (value == "Martes") {
                            $("#Martes").hide();
                        }
                        if (value == "Miércoles") {
                            $("#Miercoles").hide();
                        }
                        if (value == "Jueves") {
                            $("#Jueves").hide();
                        }
                        if (value == "Viernes") {
                            $("#Viernes").hide();
                        }
                        if (value = "Sábado") {
                            $("#Sabado").hide();
                        }
                        if (value == "Domingo") {
                            $("#Domingo").hide();
                        }
                    });
                });

                //if ((parseInt(hora2) - parseInt(hora1)) == 2) {
                $scope.ProgramacionSeleccionada1.HoraFin = hora2;
                $scope.ProgramacionSeleccionada1.HoraInicio = hora1;
                $("#ModalRegistroTransversal").modal("show");
                $("#Datos2").css("display", "none");
            };
            //--------------------------------------------------------------------------------------------

            //Función para guardar la programación de un instructor trasnversal con traslape-------------------
            $scope.GuardarProgramacionTrasversal = function () {

                var ProgramacionSeleccionada = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });

                $.each($scope.Instructor1, function (index, value) {
                    if (value.IdInstructor == $scope.Instructor1.IdInstructor) {
                        $scope.ProgramacionSeleccionada1.IdInstructor = value.IdInstructor
                    }
                });

                $scope.ProgramacionSeleccionada1.DiaSemana = $scope.DiasSemana.dia;

                $.each($scope.Ficha, function (index, value) {
                    if (value.Ficha1 == ProgramacionSeleccionada[0].Ficha) {
                        $scope.ProgramacionSeleccionada1.Ficha = value.IdFicha
                    }
                });
                $.each($scope.Ambiente, function (index, value) {
                    if (value.IdAmbiente == ProgramacionSeleccionada[0].IdAmbiente) {
                        $scope.ProgramacionSeleccionada1.IdAmbiente = value.IdAmbiente
                    }
                });
            
                //$.each($scope.Resultado, function (index, value) {
                //    if (value.Resultado == ProgramacionSeleccionada[0].Resultado) {

                //    }
                //});

                $scope.ProgramacionSeleccionada1.Resultado = $scope.Resultado[0].IdResultado
                //$scope.ProgramacionSeleccionada1.IdResultado == "" ||
                if ($scope.ProgramacionSeleccionada1.FechaFin == "") {
                    bootbox.dialog({
                        title: "Información",
                        message: "Debe diligenciar todos los campos",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });

                    return;
                } else {


                    if ($scope.ProgramacionSeleccionada1.Lunes == false && $scope.ProgramacionSeleccionada1.Martes == false && $scope.ProgramacionSeleccionada1.Miercoles == false && $scope.ProgramacionSeleccionada1.Jueves == false
                        && $scope.ProgramacionSeleccionada1.Viernes == false && $scope.SabadoDomingo.Sabado == false && $scope.SabadoDomingo.Domingo == false) {
                        bootbox.dialog({
                            title: "Información",
                            message: "Debe seleccionar por lo menos un día",
                            buttons: {
                                success: {
                                    label: "Cerrar",
                                    className: "btn-primary",
                                }
                            }
                        });
                        return;
                    }

                    if ($scope.Instructor1.IdInstructor == "") {
                        bootbox.dialog({
                            title: "Información",
                            message: "Debe dar click en el botón consultar y seleccionar un instructor",
                            buttons: {
                                success: {
                                    label: "Cerrar",
                                    className: "btn-primary",
                                }
                            }
                        });
                        return;
                    }

                    $scope.ListaProgramacion = [];
                    $.each(ProgramacionSeleccionada, function (index, value) {

                        $scope.Programacion.IdAmbiente = $scope.ProgramacionSeleccionada1.IdAmbiente;
                        $scope.Programacion.IdFicha = $scope.ProgramacionSeleccionada1.Ficha;
                        $scope.Programacion.IdInstructor = $scope.ProgramacionSeleccionada1.IdInstructor;
                        $scope.Programacion.FechaInicio = $scope.ProgramacionSeleccionada1.FechaInicio;
                        $scope.Programacion.FechaFin = $scope.ProgramacionSeleccionada1.FechaFin;
                        $scope.Programacion.HoraInicio = $scope.ProgramacionSeleccionada1.HoraInicio;
                        $scope.Programacion.HoraFin = $scope.ProgramacionSeleccionada1.HoraFin;
                        $scope.Programacion.IdResultado = $scope.ProgramacionSeleccionada1.Resultado;
                        $scope.Programacion.Lunes = $scope.ProgramacionSeleccionada1.Lunes;
                        $scope.Programacion.Martes = $scope.ProgramacionSeleccionada1.Martes;
                        $scope.Programacion.Miercoles = $scope.ProgramacionSeleccionada1.Miercoles;
                        $scope.Programacion.Jueves = $scope.ProgramacionSeleccionada1.Jueves;
                        $scope.Programacion.Viernes = $scope.ProgramacionSeleccionada1.Viernes;
                        $scope.Programacion.DiaSemana = null;
                        $scope.Programacion.ProgramacionPrincipal = ProgramacionSeleccionada[0].Id;
                        $scope.Programacion.Transversal = true;
                        if ($scope.SabadoDomingo.Sabado == true) {
                            $scope.Programacion.Jornada = 2;
                        } else if ($scope.SabadoDomingo.Domingo == true) {
                            $scope.Programacion.Jornada = 3;
                        } else {
                            $scope.Programacion.Jornada = 1;
                        }

                        $scope.ListaProgramacion.push($scope.Programacion);
                    });

                    //if ($scope.Programacion.DiaSemana < 6) {
                    //} else if ($scope.Programacion.DiaSemana == 6) {
                    //    $scope.Programacion.Jornada = 2;
                    //} else if ($scope.Programacion.DiaSemana == 7) {
                    //    $scope.Programacion.Jornada = 3;
                    //}

                    ProgramacionService.GuardarProgramacion($scope.ListaProgramacion, function (response) {
                        if (response.success == true) {
                            if (response.mensaje == null) {
                                $("#ModalRegistroTransversal").modal("hide");
                                $scope.Instructor1.IdInstructor = "";
                                location.reload();
                            } else {
                                bootbox.dialog({
                                    title: "Información",
                                    message: response.mensaje + $scope.cadena + " Por favor consulte de nuevo en horarios diferentes a los anteriores.",
                                    buttons: {
                                        success: {
                                            label: "Cerrar",
                                            className: "btn-primary",
                                        }
                                    }
                                });
                            }

                        }
                    });
                }
            };
            //-------------------------------------------------------------------------------------------------

            //-----------Objetos de fechas---------------------------------------------------------------------
            $scope.DiasSemana = [
                { dia: 1, Nombre: "Lunes" },
                { dia: 2, Nombre: "Martes" },
                { dia: 3, Nombre: "Miércoles" },
                { dia: 4, Nombre: "Jueves" },
                { dia: 5, Nombre: "Viernes" },
                { dia: 6, Nombre: "Sábado" },
                { dia: 7, Nombre: "Domingo" }

            ];

            $scope.FinSemana = [
                { dia: 1, Nombre: "Sábado" },
                { dia: 2, Nombre: "Domingo" }

            ];

            $scope.RangoHoras = [
              { Jornada: 1, Nombre: "06 am - 12 pm" },
              { Jornada: 2, Nombre: "12 pm - 06 pm" },
              { Jornada: 3, Nombre: "06 pm - 10 pm" }
            ];
            //------------------------------------------------------------------------------------------------

            // Función para cambiar entre el calendario y la tabla donde se visualizan las programaciones
            $scope.filtrarProgramacion = function () {

                $("#ModalFiltro").modal("show");
                $("#panel2").hide();
                $("#panel1").show();
            };
            //--------------------------------------------------------------------------------------------------



            //Función para traer los coordinadores--------------------------------------------------------------   
            $scope.ModalCoordinador = function () {
                $("#ModalFiltroCoordinador").modal("show");
                ProgramacionService.ConsultarCoordinadores(function (response) {
                    if (response.success == true) {
                        $scope.Coordinador = response.datos;
                    }
                });
            };
            //--------------------------------------------------------------------------------------------------

            //Función para traer las programaciones por coordinador---------------------------------------------  
            $scope.ConsultarAmbientesCoordinador = function (IdCoordinador) {
                ProgramacionService.ConsultarAmbientesCoordinador(IdCoordinador, function (response) {
                    if (response.success == true) {
                        $scope.Ambiente = response.ambiente;
                        $('#coor').css('display', 'block');
                    }
                });
            };
            //--------------------------------------------------------------------------------------------------

            //Función para el filtrar las programaciones de un coordinador-------------------------------------
            $scope.FiltroCoordinador = function (coordinador, ambiente) {
                if (coordinador.IdCoordinacion == undefined || $scope.RangoHoras.Jornada == null || ambiente.IdAmbiente == undefined) {
                    bootbox.dialog({
                        title: "Información",
                        message: "Seleccione un coordinador, un ambiente y un rango horario",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                } else {
                    ProgramacionService.FiltroProgramacionxCoordinador(coordinador.IdCoordinacion, $scope.RangoHoras.Jornada, ambiente.IdAmbiente, function (response) {
                        if (response.success == true) {
                            $scope.Programacion = response.programacion;
                            $scope.Limpiar();
                            $.each(response.programacion, function (index, value) {
                                var Inicio = value.FechaInicio.split('-');
                                var Fin = value.FechaFin.split('-');
                                var HoraInicio = value.HoraInicio.split(':');
                                var HoraFin = value.HoraFin.split(':');
                                $scope.events.push({
                                    title: ' Ficha:' + value.Ficha,
                                    start: new Date(parseInt(Inicio[0]), parseInt(Inicio[1]) - 1, parseInt(Inicio[2].substring(0, 2)), parseInt(HoraInicio[0]), parseInt(HoraInicio[1])),
                                    end: new Date(parseInt(Fin[0]), parseInt(Fin[1]) - 1, parseInt(Fin[2].substring(0, 2)), parseInt(HoraFin[0]), parseInt(HoraFin[1])),
                                    allDay: false,
                                    color: value.Color,
                                    tip: "PROGRAMA: " + value.Programa.toUpperCase() + " - " + "INSTRUCTOR:" + value.NombreInstructor.toUpperCase() + " - " + "RESULTADO:" + value.Competencia.toUpperCase()
                                });

                            });

                            var calendar = $('#calendar').fullCalendar({
                                header: {
                                    left: 'title',
                                    center: 'agendaDay,agendaWeek,month',
                                    right: 'prev,next today'

                                },
                                height: 700,
                                theme: false,
                                ignoreTimezone: false,
                                events: $scope.events,
                                eventRender: function (event, element) {
                                    element.attr('title', event.tip);
                                },
                                editable: false,
                                firstDay: 1, //  1(Monday) this can be changed to 0(Sunday) for the USA system
                                selectable: false,
                                defaultView: 'month',
                                axisFormat: 'H:mm',
                                columnFormat: {
                                    month: 'ddd',    // Mon
                                    week: 'ddd d', // Mon 7
                                    day: 'dddd M/d',  // Monday 9/7
                                    agendaDay: 'dddd d'
                                },
                                titleFormat: {
                                    month: 'MMMM yyyy', // September 2009
                                    week: "MMMM yyyy", // September 2009
                                    day: 'MMMM yyyy'                  // Tuesday, Sep 8, 2009
                                },
                                allDaySlot: false,
                                selectHelper: true,
                                select: function (start, end, allDay) {
                                    var title = prompt('Event Title:');
                                    if (title) {
                                        calendar.fullCalendar('renderEvent',
                                            {
                                                title: title,
                                                start: start,
                                                end: end,
                                                allDay: allDay
                                            },
                                            true // make the event "stick"
                                        );
                                    }
                                    calendar.fullCalendar('unselect');
                                },
                                droppable: true, // this allows things to be dropped onto the calendar !!!
                                drop: function (date, allDay) { // this function is called when something is dropped

                                    // retrieve the dropped element's stored Event Object
                                    var originalEventObject = $(this).data('eventObject');

                                    // we need to copy it, so that multiple events don't have a reference to the same object
                                    var copiedEventObject = $.extend({}, originalEventObject);

                                    // assign it the date that was reported
                                    copiedEventObject.start = date;
                                    copiedEventObject.allDay = allDay;

                                    // render the event on the calendar
                                    // the last `true` argument determines if the event "sticks" (http://arshaw.com/fullcalendar/docs/event_rendering/renderEvent/)
                                    $('#calendar').fullCalendar('renderEvent', copiedEventObject, true);

                                    // is the "remove after drop" checkbox checked?
                                    if ($('#drop-remove').is(':checked')) {
                                        // if so, remove the element from the "Draggable Events" list
                                        $(this).remove();
                                    }

                                },


                            });

                        }
                    });

                }
                $scope.CoordinacionesExport = [];
                $.each($scope.Programacion, function (index, value) {
                    $scope.CoordinacionesExport.push({
                        IdCompetencia: value.IdCompetencia, Nombre: value.Nombre, Descripcion: value.Descripcion
                    });

                });
                $scope.Limpiar();
                $("#ModalFiltroCoordinador").modal("hide");
            };
            //--------------------------------------------------------------------------------------------------

            //Función para la  consulta de los ambientes -------------------------------------------------------
            ProgramacionService.ConsultarAmbientes($rootScope.globals.currentUser.idpersona, function (response) {
                if (response.success == true) {
                    $scope.Ambiente = response.datos
                    $scope.AmbienteFiltro = response.datos
                }
            });
            //--------------------------------------------------------------------------------------------------

            //Función para la  consulta de las fichas ----------------------------------------------------------
            ProgramacionService.ConsultarFichas($rootScope.globals.currentUser.idpersona, function (response) {
                if (response.success == true) {
                    $scope.Ficha = response.datos;
                }
            });

            //---------------------------------------------------------------------------------------------------------------------------------

            //Función para realizar el filtro sobre cualquier item de la tabla
            $scope.Filter = function (e) {
                var Busqueda = $("#Buscar").val();
                var exp = new RegExp(Busqueda);
                var Programaciones = [];
                $scope.datalists = $scope.ListaCompleta;
                Programaciones = $scope.datalists.filter(function (item) {
                    if (exp.test(item.FechaInicio.toLowerCase()) || exp.test(item.FechaFin.toLowerCase()) || exp.test(item.HoraInicio.toLowerCase()) || exp.test(item.HoraFin.toLowerCase()) || exp.test(item.Ambiente.toLowerCase()) || exp.test(item.Ficha.toLowerCase()) || exp.test(item.Resultado.toLowerCase())) {
                        return item;
                    }
                    if (exp.test(item.FechaInicio.toLowerCase()) || exp.test(item.FechaInicio.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.FechaFin.toLowerCase()) || exp.test(item.FechaFin.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.HoraInicio.toLowerCase()) || exp.test(item.HoraInicio.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.HoraFin.toLowerCase()) || exp.test(item.HoraFin.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.Ambiente.toLowerCase()) || exp.test(item.Ambiente.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.Ficha.toLowerCase()) || exp.test(item.Ficha.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.Resultado.toLowerCase()) || exp.test(item.Resultado.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.NombreInstructor.toLowerCase()) || exp.test(item.NombreInstructor.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.DiasProgramados.toLowerCase()) || exp.test(item.DiasProgramados.toUpperCase())) {
                        return item;
                    }
                    else if (exp.test(item.CodigoResultado)) {
                        return item;
                    }

                });
                $scope.datalists = Programaciones;
                //Variable para setear la paginación 
                $scope.curPage = 0;
            };

            $scope.ClearFilter = function () {
                $('.searchable tr').show();
            };
            //---------------------------------------------------------------------------------------------------------------------------------

            //Función para abrir la modal de confirmación para el cambio del estado del instructor de activo a inactivo
            $scope.CambiarEstadoSeleccionados = function () {
                var UsariosBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });

                if (UsariosBorrar.length == 0 || UsariosBorrar.length > 1) {

                    bootbox.dialog({
                        title: "Inhabilitar",
                        message: "Debe seleccionar una programación",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                } else {

                    ProgramacionService.ConsultaEliminarProgramacionTransversales(UsariosBorrar, function (response) {

                        if (response.success == true) {
                            if (response.programaciones.length > 0) {

                                bootbox.dialog({
                                    title: "Información",
                                    message: "La programación seleccionada tiene programaciones transversales asociadas, si la elimina también se eliminaran dichas transversales",
                                    buttons: {
                                        success: {
                                            label: "Cerrar",
                                            className: "btn-primary",
                                        }
                                    }
                                });
                            }
                        }
                    });
                    $("#modalInhabilitar").modal("show");
                }
            };
            //--------------------------------------------------------------------------------------------------------------------

            //Función para eliminar programaciones--------------------------------------------------------------------------------- 
            $scope.inhabilitar = function () {

                var ProgramacionBorrar = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });

                ProgramacionService.EliminarProgramacion(ProgramacionBorrar, function (response) {
                    if (response.success == true) {
                        if ($('#atras').is(":visible")) {
                            $scope.ConsultarProgramacionTransversales();
                        } else {
                            ProgramacionService.ConsultarProgramacion($rootScope.globals.currentUser.idpersona, function (response) {
                                if (response.success == true) {
                                    $scope.datalists = response.datos;
                                    $.each($scope.datalists, function (index, value) {
                                        $scope.datalists[index].FechaInicio = value.FechaInicio.toString().substring(0, 10);
                                        $scope.datalists[index].FechaFin = value.FechaFin.toString().substring(0, 10);
                                    })

                                };
                            });
                        }
                    }
                });
            };
            //-------------------------------------------------------------------------------------------------------------------

            //Función para consultar el programa transversal y las sedes donde se va a programar el instructor transversal--------
            $scope.ProgramaTranversalxDia = function () {
                $("#modalTranversalxDia").modal("show");
                ProgramacionService.ConsultarSedes(function (response) {
                    if (response.success == true) {
                        $scope.Sede = response.datos;
                    }
                });
                ProgramacionService.ConsultarPragamaTransversal(function (response) {
                    if (response.success == true) {
                        $scope.Programa = response.programa;
                    }
                });
            };
            //--------------------------------------------------------------------------------------------------------------------

            // Función para abrir la modal de programción contra jornada----------------------------------------------------------
            $scope.ModalContraJornada = function () {
                $("#ModalTipoProgramaciones").modal("hide");
                $("#modalTranversalContraJornada").modal("show");
                $("#panel2").hide();
                $("#panel1").show();
                ProgramacionService.ConsultarSedes(function (response) {
                    if (response.success == true) {
                        $scope.Sede = response.datos;
                    }
                });
                ProgramacionService.ConsultarPragamaTransversal(function (response) {
                    if (response.success == true) {
                        $scope.Programa = response.programa;
                        ProgramacionService.ConsultarCompetenciasxPrograma($scope.Programa.IdPrograma, function (response) {
                            if (response.success == true) {
                                $.each(response.competencia, function (index, value) {
                                    var nombreCompetencia = value.Nombre.split(' ');
                                    value.Codigo = value.Codigo + " " + nombreCompetencia[0];
                                });
                                $scope.Competencia = response.competencia;
                            }
                        });
                    }
                });
                ProgramacionService.ProgramaTituladosxCoordinacion($rootScope.globals.currentUser.idpersona, function (response) {
                    if (response.success == true) {
                        $scope.ProgramasTitulados = response.datos;
                    }
                });
            };
            //--------------------------------------------------------------------------------------------------------------------

            //Función que consulta las fichas asociadas a un programa en especifico-----------------------------------------------
            $scope.FichasxPrograma = function (Programa) {
                ProgramacionService.FichasxPrograma(Programa.IdPrograma, function (response) {
                });
            }
            //--------------------------------------------------------------------------------------------------------------------

            // Consultar si una ficha tiene ya una clase transversal programada --------------------------------------------------
            $scope.ConsultarDiaTransversal = function (ficha) {
                $scope.Programacion.FechaInicio = $("#FechaInicioT").val();
                $scope.Programacion.FechaFin = $("#FechaFinT").val();
                $scope.Programacion.HoraInicio = $("#HoraInicioT").val();
                $scope.Programacion.HoraFin = $("#HoraFinT").val();
                ProgramacionService.ConsultarDiaTransversal($scope.Programacion, ficha, function (response) {
                    if (response.success == true) {
                        $("#InstructorT").prop("disabled", false);
                        $scope.RespuestaT = response.resp;
                        $scope.Instructor = response.instructor;
                        $scope.DiasSemana.dia = response.diasemana;
                        if ($scope.RespuestaT == 1) {
                            $("#ListaDias").css("display", "block");
                            $("#DiaSemanaT").css("display", "none");
                            $("#combodiasemana").css("display", "block");
                        } else {
                            switch ($scope.DiasSemana.dia) {
                                case 1:
                                    $("#DiaSemanaT").val("Lunes");
                                    break;
                                case 2:
                                    $("#DiaSemanaT").val("Martes");
                                    break;

                                case 3:
                                    $("#DiaSemanaT").val("Miércoles");
                                    break;
                                case 4:
                                    $("#DiaSemanaT").val("Jueves");
                                    break;
                                case 5:
                                    $("#DiaSemanaT").val("Viernes");
                                    break;

                                case 6:
                                    $("#DiaSemanaT").val("Sábado");
                                    break;

                                case 7:
                                    $("#DiaSemanaT").val("Domingo");
                                    break;
                            }
                            $("#ListaDias").css("display", "none");
                            $("#DiaSemanaT").css("display", "block");
                            $("#combodiasemana").css("display", "block");
                        }
                    }
                });
            };
            //--------------------------------------------------------------------------------------------------------------------

            //Función para realizar la consulta de los programas curso especial siempre y cuando la sede sea empresa
            $scope.SedeEmpresa = function (sede) {

                if ($scope.ObjetoSedes.Sede == true) {
                    ProgramacionService.ConsultarSedexId(sede.IdSede, function (response) {
                        if (response.success == true) {
                            if (response.sede.TipoSede == 2) {

                                //$("#form1").css("display", "block");
                                //$("#form2").css("display", "block");
                                //$("#form3").css("display", "block");
                                //$("#form4").css("display", "block");
                                //$("#divDerecho").css("margin-top", "5px");
                                //$("#divDerecho1").css("margin-top", "5px");
                                //$scope.Programa = "";
                            } else if (response.sede.TipoSede == 4) {
                                if (ModalJornada == 1) {
                                    bootbox.dialog({
                                        title: "Información",
                                        message: "Desde la programación jornadas no se puede programar en la sede virtual, por favor vaya a la programación técnica",
                                        buttons: {
                                            success: {
                                                label: "Cerrar",
                                                className: "btn-primary",
                                            }
                                        }
                                    });
                                    $scope.ConsultaSedes();
                                    return;
                                }
                                $scope.ObjetoPrograma.Titulado = false;
                                $scope.ObjetoPrograma.Complementario = false;
                                //$("#form1").css("display", "none");
                                //$("#form2").css("display", "none");
                                //$("#form3").css("display", "none");
                                //$("#form4").css("display", "none");
                                //$("#divDerecho").css("margin-top", "47px");
                                //$("#divDerecho1").css("margin-top", "47px");
                                ProgramacionService.ProgramaVirtualesTitulados($rootScope.globals.currentUser.idpersona, function (response) {
                                    if (response.success == true) {
                                        if (response.datos.length > 0) {
                                            $scope.Programa = response.datos;
                                        } else {
                                            bootbox.dialog({
                                                title: "Información",
                                                message: "No tiene programas virtuales asociados a su coordinación",
                                                buttons: {
                                                    success: {
                                                        label: "Cerrar",
                                                        className: "btn-primary",
                                                    }
                                                }
                                            });
                                            $scope.ConsultaSedes();
                                        }
                                    }
                                });
                            } else {

                                $scope.ObjetoPrograma.Titulado = false;
                                $scope.ObjetoPrograma.Complementario = false;
                                //$("#form1").css("display", "block");
                                //$("#form2").css("display", "block");
                                //$("#form3").css("display", "block");
                                //$("#form4").css("display", "block");
                                //$("#divDerecho").css("margin-top", "5px");
                                //$("#divDerecho1").css("margin-top", "5px");
                                ProgramacionService.ProgramaTituladosxCoordinacion($rootScope.globals.currentUser.idpersona, function (response) {
                                    if (response.success == true) {
                                        $scope.Programa = response.datos;
                                    }
                                });
                            }
                        }
                    });
                }
            }
            //--------------------------------------------------------------------------------------------------------------------


            ////----------------------------------------------------REPORTE DE PROGRAMACIÓN ---------------------------------------------


            $scope.Fechas = {
                FechaInicio: "",
                FechaFin: ""
            }

            $('#FechaInicio3').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#FechaFin3').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });


            //------------Reporte de progrmacion -------------------------------------------------------------------------------
            $scope.ModalReporte = function () {

                $("#ModalReporte").modal("show");
            }

            $scope.GenerarReporte = function () {

                if ($rootScope.globals.currentUser.idpersona == "") {
                    $rootScope.globals.currentUser.idpersona = 0;
                }
                var x = $("#FechaInicio3").val().split('/');
                var y = $("#FechaFin3").val().split('/');

                var StartDate = x[1] + "-" + x[2] + "-" + x[0];
                var EndDate = y[1] + "-" + y[2] + "-" + y[0];
                if (Date.parse(StartDate) > Date.parse(EndDate)) {
                    bootbox.dialog({
                        title: "Información",
                        message: "La fecha final debe ser mayor a la fecha inicial",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                ProgramacionService.GenerarReporte($scope.Fechas, $rootScope.globals.currentUser.idpersona, function (response) {

                    if (response.success == true) {
                        $scope.Programacion = response.datos;
                        $scope.ProgramacionFichaExport = [];

                        $.each($scope.Programacion, function (index, value) {
                            var fechaIini = value.FechaInicio.split('T');
                            var fechafin = value.FechaFin.split('T');

                            var x = fechaIini[0].split('-');
                            var y = fechafin[0].split('-');

                            //Aquí se formatea la fecha con la estructura dd/mm/aaaa
                            //var fechaInicio = x[1] + "/" + x[2] + "/" + x[0];
                            //var fechafinal = y[1] + "/" + y[2] + "/" + y[0];

                            $scope.ProgramacionFichaExport.push({
                                Area: value.Area, Nombre_Instructor: value.NombreInstructor, Cedula: value.CedulaInstructor, Codigo_Programa: value.CodigoPrograma.toString(),
                                Version_Programa: value.VersionPrograma, Programa: value.Programa, Nivel_Programa: value.NivelPrograma, Ficha: parseInt(value.Ficha), Nombre_Sede: value.Sede,
                                Ambiente: value.IdAmbiente, Fecha_Inicio: fechaIini[0], Fecha_Fin: fechafin[0],
                                Hora_Inicio: value.HoraInicio, Hora_Fin: value.HoraFin, Total_Horas: value.TotalHoras,
                                Competencia: value.Competencia, Resultado: value.Resultado, Dias_Semana: value.Jornada
                            });
                        });
                        alasql('SELECT * INTO XLSX("Reporte Programación.xlsx",{headers:true}) FROM ?', [$scope.ProgramacionFichaExport]);
                    }
                });
            }

            // ------------------------------------------envió de correo al instuctor con la clase programada----------------------------------

            $scope.EnviarCorreo = function () {

                var Programaciones = $scope.datalists.filter(function (item) {
                    return item.Seleccionado === true;
                });

                ProgramacionService.EnviarProgramacion(Programaciones, function (response) {
                    if (response.success == true) {
                        bootbox.dialog({
                            title: "Inhabilitar",
                            message: "El envío de correo se realizó con éxito ",
                            buttons: {
                                success: {
                                    label: "Cerrar",
                                    className: "btn-primary",
                                }
                            }
                        });
                    }
                });
            }

            $scope.AbrirModalSabado = function () {
                $('#ModalSabado').modal('show');
            }

            //------------------------------------------- Consulta de Programacion por Instructor-----------------------------------------------

            $scope.ConsultarPogramacionesInstructor = function () {
                ProgramacionService.ConsultarPogramacionesInstructor($scope.cedula, function (response) {
                    if (response.success) {
                        $scope.datalists = response.datos;
                        $.each($scope.datalists, function (index, value) {

                            $scope.datalists[index].FechaInicio = value.FechaInicio.toString().substring(0, 10);
                            $scope.datalists[index].FechaFin = value.FechaFin.toString().substring(0, 10);
                            $scope.datalists[index].HoraInicio = value.HoraInicio.toString().substring(0, 5);
                            $scope.datalists[index].HoraFin = value.HoraFin.toString().substring(0, 5);

                            if ($scope.datalists[index].RecibioLLaves) {
                                $("input[name='recibio" + index + "']").prop("disabled", true);

                            }
                            //Variable para setear la 
                            $scope.curPage = 0;
                        });
                    }
                })
            }

            //--------------------------------------------Entrega de llaves----------------------------------------------------------------------
            $scope.Ob = { Observacion: "" }

            $scope.ModalPrestamo = function (posicion, opc) {
                $scope.Ob.Observacion = null;
                $("#Préstamo").modal("show");
                $scope.progrmacionSelec = $scope.datalists[posicion];
                if (opc == 1) {
                    $scope.Ob.mensaje = "¿Desea confirmar el prestamo de las llaves?";
                    $scope.progrmacionSelec.RecibioLLaves = true;
                    $("#Observacion").show();
                } else {
                    $scope.Ob.mensaje = "¿Desea confirmar la recepción de las llaves?";
                    $scope.progrmacionSelec.EntregoLLaves = true;
                    $("#Observacion").hide();
                }
            }

            $scope.PrestamoLlaves = function () {
                $scope.progrmacionSelec.Observacion = $scope.Ob.Observacion;

                ProgramacionService.GuardarPrestamoLLaves($scope.progrmacionSelec, function (response) {
                    if (response.success) {
                        $scope.datalists = response.datos;
                        $.each($scope.datalists, function (index, value) {

                            $scope.datalists[index].FechaInicio = value.FechaInicio.toString().substring(0, 10);
                            $scope.datalists[index].FechaFin = value.FechaFin.toString().substring(0, 10);
                            $scope.datalists[index].HoraInicio = value.HoraInicio.toString().substring(0, 5);
                            $scope.datalists[index].HoraFin = value.HoraFin.toString().substring(0, 5);
                            if ($scope.datalists[index].RecibioLLaves == false) {
                                $("input[name='recibio" + index + "']").prop("disabled", true);

                            }
                            //Variable para setear la paginación 
                            $scope.curPage = 0;
                        });
                        bootbox.dialog({
                            title: "Información",
                            message: "El prestamo de llaves se registro con éxito.",
                            buttons: {
                                success: {
                                    label: "Cerrar",
                                    className: "btn-primary",
                                }
                            }
                        });
                    }
                })
            }

            $scope.CancelarLlaves = function () {
                if ($scope.progrmacionSelec.EntregoLLaves == true) {
                    $scope.progrmacionSelec.EntregoLLaves = false;
                } else {
                    $scope.progrmacionSelec.RecibioLLaves = false;
                }
            }


            $scope.AbrirReporteLlaves = function () {
                $("#RepLlaves").modal("show");
            }


            $('#FechaInicio4').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });

            $('#FechaFin4').datepicker({
                language: 'es',
                autoclose: true,
                //daysOfWeekDisabled: [0]
            });


            $scope.Fechas1 = {
                FechaInicio: "",
                FechaFin: ""
            }

            $scope.ReporteLlaves = function () {

                var x = $("#FechaInicio4").val().split('/');
                var y = $("#FechaFin4").val().split('/');

                var StartDate = x[1] + "-" + x[2] + "-" + x[0];
                var EndDate = y[1] + "-" + y[2] + "-" + y[0];
                if (Date.parse(StartDate) > Date.parse(EndDate)) {
                    bootbox.dialog({
                        title: "Información",
                        message: "La fecha final debe ser mayor a la fecha inicial",
                        buttons: {
                            success: {
                                label: "Cerrar",
                                className: "btn-primary",
                            }
                        }
                    });
                    return;
                }
                ProgramacionService.ReporteLlaves($scope.Fechas1, function (response) {

                    if (response.success == true) {
                        $scope.Llaves = response.datos;
                        $scope.ProgramacionFichaExport = [];

                        $.each($scope.Llaves, function (index, value) {
                            var fechaIini = value.FechaInicio.split('T');
                            var x = fechaIini[0].split('-');
                            if (value.Observacion == null) {
                                value.Observacion = "";
                            }
                            $scope.ProgramacionFichaExport.push({
                                Nombre_Instructor: value.NombreInstructor, Cedula: value.CedulaIns,
                                Ficha: parseInt(value.Ficha),
                                Ambiente: value.Ambiente, Fecha: fechaIini[0],
                                Hora_Reciibio: value.HoraInicio, Hora_Entrego: value.HoraFin,
                                Competencia: value.Competencia, Observacion: value.Observacion
                            });
                        });
                        alasql('SELECT * INTO XLSX("Reporte Entrega llaves.xlsx",{headers:true}) FROM ?', [$scope.ProgramacionFichaExport]);
                    }
                });
            }

            $scope.AmbientesDisponibles = function () {
                //$("#Filtro").show();
                $("#BuscarCedulaInstructor").hide();
                $(".filtroCedula").show();

                ProgramacionService.AmbientesDisponibles(function (response) {
                    if (response.success) {
                        $scope.datalists = response.datos;
                        $scope.ListaCompleta = response.datos;
                        $.each($scope.datalists, function (index, value) {

                            $scope.datalists[index].FechaInicio = value.FechaInicio.toString().substring(0, 10);
                            $scope.datalists[index].FechaFin = value.FechaFin.toString().substring(0, 10);
                            $scope.datalists[index].HoraInicio = value.HoraInicio.toString().substring(0, 5);
                            $scope.datalists[index].HoraFin = value.HoraFin.toString().substring(0, 5);
                            if ($scope.datalists[index].RecibioLLaves == false) {
                                $("input[name='recibio" + index + "']").prop("disabled", true);
                            }
                            //Variable para setear la paginación 
                            $scope.curPage = 0;
                        });
                    }
                })
            }

            $scope.RegresarLlavesAmbientes = function () {
                $("#BuscarCedulaInstructor").hide();
                $(".filtroCedula").show();

                ProgramacionService.RegresarLlavesAmbientesDisponibles(function (response) {
                    if (response.success) {
                        $scope.datalists = response.datos;
                        $scope.ListaCompleta = response.datos;
                        $.each($scope.datalists, function (index, value) {

                            $scope.datalists[index].FechaInicio = value.FechaInicio.toString().substring(0, 10);
                            $scope.datalists[index].FechaFin = value.FechaFin.toString().substring(0, 10);
                            $scope.datalists[index].HoraInicio = value.HoraInicio.toString().substring(0, 5);
                            $scope.datalists[index].HoraFin = value.HoraFin.toString().substring(0, 5);
                            if ($scope.datalists[index].RecibioLLaves == false) {
                                $("input[name='recibio" + index + "']").prop("disabled", true);
                            }
                            //Variable para setear la paginación 
                            $scope.curPage = 0;
                        });
                    }
                })
            }

            $scope.MostrarFiltroCedula = function () {
                $("#BuscarCedulaInstructor").show();
                $(".filtroCedula").hide();
                $scope.datalists = "";
            }

            $scope.FilterAmbiente = function (e) {
                var Busqueda = $("#Buscar1").val();
                var exp = new RegExp(Busqueda);
                var Programaciones = [];
                $scope.datalists = $scope.ListaCompleta;
                Programaciones = $scope.datalists.filter(function (item) {
                    //if (exp.test(item.FechaInicio.toLowerCase()) || exp.test(item.FechaFin.toLowerCase()) || exp.test(item.HoraInicio.toLowerCase()) || exp.test(item.HoraFin.toLowerCase()) || exp.test(item.Ambiente.toLowerCase()) || exp.test(item.Ficha.toLowerCase()) || exp.test(item.Resultado.toLowerCase())) {
                    //    return item;
                    //}
                    if (exp.test(item.Ambiente)) {
                        return item;
                    }

                });
                $scope.datalists = Programaciones;
                //Variable para setear la paginación 
                $scope.curPage = 0;
            };
        }]);