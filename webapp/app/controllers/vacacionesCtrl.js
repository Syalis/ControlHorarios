angular.module('webapp').controller('vacacionesCtrl', ["$scope", "$http", "$window", vacacionesCtrl]);

function vacacionesCtrl($scope, $http, $window) {

    var vm = this;
    vm.lista = { data: [] };

    // Variables
    vm.session = $window.sessionStorage;
    vm.peticionVacaciones = peticionVacaciones;
    vm.calendario = calendario;
    vm.diasRestantesVacaciones = diasRestantesVacaciones
    vm.eventoCalendario = eventoCalendario;

    vm.item = [];
    vm.eventos = [];
    vm.diasVacaciones = [];
    vm.vacacionesCalendario = [];
    vm.vacacionesBD = [];

    // Método que se inicia al iniciar la página. 
    diasRestantesVacaciones();
    calendario();

    // Método para saber cúantos días restatntes nos queda de vacaciones
    function diasRestantesVacaciones() {
        try {
            $http.post("Vacaciones/getDiasVacaciones", {
                item:
                {
                    id_usuario: vm.session.id
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

    // Método para taernos todos los datos de la BBDD sobre filtrado por id_usuario para pintar el calendario
    function calendario() {
        try {
            $http.post("Vacaciones/getDiasVacacionesCalendario", {
                item:
                {
                    id_usuario: vm.session.id
                }
            }).then(function (r) {
                if (r.data.cod == "OK") {
                    vm.vacacionesCalendario = r.data.d.data;
                    eventoCalendario();
                    console.log(r.data);
                    
                } else {
                    console.log("Error");
                }
               
            });
        } catch (ex) {
            return ex.message;
        }
    }

    // Método para enviar a la BBDD los dias de que nos vamos de vacaciones
    function peticionVacaciones(item) {
        console.log(item);

        if (vm.item.fecha_inicio_vacaciones != null && vm.item.fecha_final_vacaciones != null) {
            $http.post("Vacaciones/createPeticionVacaciones", {
                item:
                {
                    id_usuario: vm.session.id,
                    fecha_inicio_vacaciones: vm.item.fecha_inicio_vacaciones,
                    fecha_final_vacaciones: vm.item.fecha_final_vacaciones
                }
            }).then(function (r) {
                if (r.data.cod == "OK") {
                    vm.lista.data = r.data.d.data;
                    diasRestantesVacaciones();
                    calendario();
                    eventoCalendario();
                    Swal.fire({
                        position: 'top-end',
                        type: 'success',
                        title: 'Petición correta!',
                        footer: 'Vacaciones aceptadas!',
                        showConfirmButton: false,
                        timer: 1700
                    })
                    vm.item = {};
                } else {
                    vm.item = {};
                    Swal.fire({
                        position: 'top-end',
                        type: 'error',
                        title: 'Petición incorreta!',
                        showConfirmButton: false,
                        timer: 1700
                    })

                }
            });
        } else {
            Swal.fire({
                type: 'error',
                title: 'Oops...Fechas erroreas!',
                text: 'Revisa los campos de las fechas!'
            })
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

    

}