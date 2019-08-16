angular.module('webapp').controller('vacacionesCtrl', ["$scope", "$http", "$window",'auxVacacionesCtrl', vacacionesCtrl]);

function vacacionesCtrl($scope, $http, $window, auxVacacionesCtrl) {

    var vm = this;
    vm.lista = { data: [] };

    // Variables
    vm.session = $window.sessionStorage;
    vm.peticionVacaciones = peticionVacaciones;
    vm.calendario = calendario;
    vm.diasRestantesVacaciones = diasRestantesVacaciones
    vm.eventoCalendario = eventoCalendario;

    // Método que se inicia al recargar la página. 
    diasRestantesVacaciones();
    calendario();

    vm.item = [];
    vm.dataSource = [];
    vm.diasVacaciones = [];
    vm.vacacionesCalendario = [];
    vm.vacacionesBD = [];

    //$(document).ready(function () {
    //    $("save-event").show();

    //});


    // Método para saber cúantos días restatntes nos queda de vacaciones
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
                    //if (vm.diasVacaciones[0].total_vacaciones == 0) {
                    //    $("#save-event").hide();
                    //}
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
                    id_usuario: 3
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
                    id_usuario: 3,
                    fecha_inicio_vacaciones: vm.item.fecha_inicio_vacaciones,
                    fecha_final_vacaciones: vm.item.fecha_final_vacaciones
                }
            }).then(function (r) {
                if (r.data.cod == "OK") {
                    vm.lista.data = r.data.d.data;
                    diasRestantesVacaciones();
                    calendario();
                    eventoCalendario();
                    //auxVacacionesCtrl.consultaCorrecta();
                    vm.item = {};
                } else {
                    vm.item = {};
                    //auxVacacionesCtrl.consultaInCorrecta();

                }
            });
        } else {

            auxVacacionesCtrl.validarFecha();
        }
    }

    function resetearDiasVacaciones() {

        var currentYear = new Date().getFullYear();

        $('#calendar').calendar({

            

        });

    }

    // Método para pintar los días que nos vamos de vacaciones
    function eventoCalendario() {

        var currentYear = new Date().getFullYear();

        for (var i = 0; i < vm.vacacionesCalendario.length; i++) {
            
                var event = {
                    id: vm.vacacionesCalendario[i].id,
                    startDate: new Date(currentYear, (vm.vacacionesCalendario[i].mes_inicio -1), vm.vacacionesCalendario[i].dia_inicio ),
                    endDate: new Date(currentYear, (vm.vacacionesCalendario[i].mes_final -1), vm.vacacionesCalendario[i].dia_final)
                }
           
            vm.dataSource[i] = event;
           
        }
   
        vm.dataSource = $('#calendar').data('calendar').getDataSource();

        $('#calendar').data('calendar').setDataSource(vm.dataSource);

        console.log(vm.dataSource);


    }

    // Método que hace el dibujado en el calendario. 
    $(function () {

        $('#calendar').calendar({

            dataSource:  vm.dataSource
                
        });
       
    });

    

}