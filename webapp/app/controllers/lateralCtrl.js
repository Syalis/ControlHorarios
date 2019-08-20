angular.module('webapp').controller('lateralCtrl', ["$scope", "$http", "$window", lateralCtrl]);

function lateralCtrl($scope, $http, $window) {
    var vm = this;
    vm.session = $window.sessionStorage;

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


}