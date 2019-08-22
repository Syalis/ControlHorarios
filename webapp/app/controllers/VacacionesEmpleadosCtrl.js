angular.module('webapp').controller('VacacionesEmpleadosCtrl', ["$scope", "$http", "$window", VacacionesEmpleadosCtrl]);

function VacacionesEmpleadosCtrl($scope, $http, $window) {

    var vm = this;
    vm.lista = { data: [] };

    // Variables
    vm.session = $window.sessionStorage;

    vm.calendario = calendario;
    vm.diasRestantesVacaciones = diasRestantesVacaciones

    vm.diasRestantesVacacionesNueva = diasRestantesVacacionesNueva;
    vm.sumarAnio = sumarAnio;
    vm.restarAnio = restarAnio;
    vm.gripCompleto = gripCompleto;
    vm.contador = 0;

    vm.pintadoCalendario = [];
    vm.item = [];
    vm.eventos = [];
    vm.diasVacaciones = [];
    vm.vacacionesCalendario = [];
    vm.vacacionesBD = [];

    ////////Inicio Drppdowm///////////////

    //Declaracion de variables
    vm.listaDropdown = [];

    //Declaracion de funciones
    vm.getNombresDropdown = getNombresDropdown;

    //Init
    getNombresDropdown();

    //Funciones
    function getNombresDropdown() {
        $http.post("lateral/getNombresDropdown").then(function (r) {
            if (r.data.cod == "OK") {
                vm.listaDropdown = r.data.d.data;

            }
        })
    }
    ////////Final Drppdowm///////////

    // Método para saber cúantos días restatntes nos queda de vacaciones
    function diasRestantesVacaciones(id) {
        try {
            $http.post("Vacaciones/getDiasTotalVacaciones", {
                item:
                {
                    id_usuario: id
                }
            }).then(function (r) {
                if (r.data.cod == "OK") {
                    vm.diasVacaciones = r.data.d.data;
       
                } else {
                    console.log("Error");
                }
            });

        } catch (ex) {
            return ex.message;
        }
    }

    //Función para llamar a dos funciones y para resetar el grip del calendario
    function gripCompleto(id) {
        vm.eventos = [];
        diasRestantesVacaciones(id);
        calendario(id);

    }

    // Método para saber cúantos días restatntes nos queda de vacaciones en lo siguientes años
    function diasRestantesVacacionesNueva() {
        try {
            $http.post("Vacaciones/getDiasTotalVacacionesAnio", {
                item:
                {
                    id_usuario: vm.listaDropdown.select[0].id,
                    nYear: vm.vacacionesCalendario[0].anio
                }
            }).then(function (r) {
                if (r.data.cod == "OK") {

                    if (r.data.d.data.length == 0) {
                        vm.diasVacaciones[0].total_vacaciones = 30;
                    } else {
                        vm.diasVacaciones = r.data.d.data;
                    }

                } else {
                    console.log("Error");
                }
            });

        } catch (ex) {
            return ex.message;
        }
    }

  

    // Método para taernos todos los datos de la BBDD sobre filtrado por id_usuario para pintar el calendario
    function calendario(id) {
        try {
            $http.post("Vacaciones/getDiasVacacionesCalendario", {
                item:
                {
                    id_usuario: id
                }
            }).then(function (r) {
                if (r.data.cod == "OK") {
                    vm.vacacionesCalendario = r.data.d.data;
                    eventoCalendario();
                } else {
                    console.log("Error");
                }

            });
        } catch (ex) {
            return ex.message;
        }
    }

    

    // Método para pintar los días que nos vamos de vacaciones
    function eventoCalendario() {

        var currentYear = new Date().getFullYear();

        for (var i = 0; i < vm.vacacionesCalendario.length; i++) {

            var event = {
                id: vm.vacacionesCalendario[i].id,
                startDate: new Date(currentYear, (vm.vacacionesCalendario[i].mes_inicio - 1), vm.vacacionesCalendario[i].dia_inicio),
                endDate: new Date(currentYear, (vm.vacacionesCalendario[i].mes_final - 1), vm.vacacionesCalendario[i].dia_final)
            }

            vm.eventos[i] = event;
        }

        vm.dataSource = $('#calendar').data('calendar').getDataSource();

        $('#calendar').data('calendar').setDataSource(vm.eventos);

        console.log(vm.eventos);

    }

    // Método que hace el dibujado en el calendario. 
    $(function () {

        $('#calendar').calendar({

            dataSource: vm.eventos

        });

    });


    // Método para sumar añadir un año
    function sumarAnio(item) {
        ++vm.contador;
        $http.post("Vacaciones/getYear", {
            item:
            {
                id_usuario: vm.listaDropdown.select[0].id,
                nYear: vm.contador
            }
        }).then(function (r) {
            if (r.data.cod == "OK") {
                vm.vacacionesCalendario = r.data.d.year;
                var year = vm.vacacionesCalendario[0].anio;
                $('#calendar').data('calendar').setYear(year);
                diasRestantesVacacionesNueva();
            }
        })

    }

    // Método para restar un año
    function restarAnio(item) {
        --vm.contador;
        $http.post("Vacaciones/getYear", {
            item:
            {
                id_usuario: vm.listaDropdown.select[0].id,
                nYear: vm.contador
            }
        }).then(function (r) {
            if (r.data.cod == "OK") {
                vm.vacacionesCalendario = r.data.d.year;
                var year = vm.vacacionesCalendario[0].anio;
                $('#calendar').data('calendar').setYear(year);
                diasRestantesVacacionesNueva();
            }
        })

    }
}