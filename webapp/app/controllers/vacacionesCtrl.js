angular.module('webapp').controller('vacacionesCtrl', ["$scope", "$http", "$window", vacacionesCtrl]);

function vacacionesCtrl($scope, $http, $window) {

    var vm = this;
    vm.lista = { data: [] };

    vm.session = $window.sessionStorage;
    vm.peticionVacaciones = peticionVacaciones;
    vm.saveEvent = saveEvent;
    vm.calendario = calendario;
    vm.diasRestantesVacaciones = diasRestantesVacaciones

    diasRestantesVacaciones();
    calendario();

    vm.diasVacaciones = [];
    vm.vacacionesCalendario = [];
    setDataSource = [];


    function diasRestantesVacaciones() {
        try {
            $http.post("Vacaciones/getDiasVacaciones", {
                item:
                {
                    id_usuario: 3
                }
            }).then(function (r) {
                if (r.data.cod == "OK") {
                    vm.diasVacaciones = r.data.d.data;
                    console.log(r.data);
                } else {
                    console.log("Error");
                }
               
            });
        } catch (ex) {
            return ex.message;
        }
    }

    function calendario() {
        try {
            $http.post("Vacaciones/getDiasVacacionesCalendario", {
                item:
                {
                    id_usuario: 3
                }
            }).then(function (r) {
                if (r.data.cod == "OK") {
                    vm.vacacionesCalendario = r.data.d.data;
                    console.log(r.data);
                } else {
                    console.log("Error");
                }
               
            });
        } catch (ex) {
            return ex.message;
        }
    }


    function peticionVacaciones(item) {
        console.log(item);
        $http.post("Vacaciones/createPeticionVacaciones", {
            item:
            {
                id_usuario: 3,
                fecha_inicio_vacaciones: vm.item.fecha_inicio_vacaciones,
                fecha_final_vacaciones: vm.item.fecha_final_vacaciones
            }
        }).then(function (r) {
            vm.lista.data = r.data.d.data;
            diasRestantesVacaciones();
            calendario();
        });
    }

    function saveEvent() {
        var event = {
            //id: ,

            //startDate: ,
            //endDate: 
        }

        var dataSource = $('#calendar').data('calendar').getDataSource();

        if (event.id) {
            for (var i in dataSource) {
                if (dataSource[i].id == event.id) {

                    dataSource[i].startDate = event.startDate;
                    dataSource[i].endDate = event.endDate;
                }
            }
        }
        else {
            var newId = 0;
            for (var i in dataSource) {
                if (dataSource[i].id > newId) {
                    newId = dataSource[i].id;
                }
            }

            newId++;
            event.id = newId;

            dataSource.push(event);
        }

        $('#calendar').data('calendar').setDataSource(dataSource);

    }

    $(function () {
        var currentYear = new Date().getFullYear();

        $('#calendar').calendar({
            dataSource: [
                {
                    id: 0,
                    startDate: new Date(currentYear, 4, 28),
                    endDate: new Date(currentYear, 4, 29)
                },
                {
                    id: 1,

                    startDate: new Date(currentYear, 2, 16),
                    endDate: new Date(currentYear, 2, 19)
                }
          
            ]
        });

        $('#save-event').click(function () {
            saveEvent();
        });
    });

}