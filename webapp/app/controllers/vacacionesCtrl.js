angular.module('webapp').controller('vacacionesCtrl', ["$scope", "$http", vacacionesCtrl]);

function vacacionesCtrl($scope, $http) {

    var vm = this;
    vm.lista = { data: [] };

    vm.vacaciones = vacaciones;
    vm.diasVacaciones = diasVacaciones

    diasVacaciones();
    vm.vacaciones = [];


    function diasVacaciones(item) {
        try {
            $http.post("Vacaciones/getRestantesVacaciones", {
                item:
                {
                    id_usuario: 1
                }
            }).then(function (r) {
                vm.vacaciones = r.data;
                console.log(r.data);
            });
        } catch (ex) {
            return ex.message;
        }
    }


    function vacaciones(item) {
        
        $http.post("Vacaciones/getVacaciones", {
            item:
            {
                id_usuario: 1,
                fecha_inicio_vacaciones: vm.item.fecha_inicio_vacaciones,
                fecha_final_vacaciones: vm.item.fecha_final_vacaciones
            }
        }).then(function (r) {
            console.log(item);
            if (r.data.cod == "OK") {
                vm.lista.data = r.data.d.data;
                console.log(vm.lista.data);
            } else {
                console.log("Error");
            }
        });


    }

}