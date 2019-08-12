angular.module('webapp').controller('vacacionesCtrl', ["$scope", "$http", "$window", vacacionesCtrl]);

function vacacionesCtrl($scope, $http, $window) {

    var vm = this;
    vm.lista = { data: [] };

    vm.session = $window.sessionStorage;
    vm.peticionVacaciones = peticionVacaciones;
    vm.diasRestantesVacaciones = diasRestantesVacaciones

    diasRestantesVacaciones();

    vm.diasVacaciones = [];


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
            console.log(r.data.d.data);
            diasRestantesVacaciones();
           
        });


    }

}