angular.module('webapp').controller('DepartamentosCtrl', ["$scope", "$http", "$window", DepartamentosCtrl]);

function DepartamentosCtrl($scope, $http, $window) {
    var vm = this;
    vm.session = $window.sessionStorage;

    //Declaracion de variables
    vm.lista = { data: [], disp: [], filter: [] };
    vm.listaEmpleados = { data: [], disp: [], filter: [] };
    //Declaracion de funciones
    vm.getDepartamentosTabla = getDepartamentosTabla;
    vm.getEmpleadosDropdown = getEmpleadosDropdown;
    //Init
    getDepartamentosTabla();
    getEmpleadosDropdown();
    //Funciones
    //funcion para cargar todos los empleados en la tabla
    function getDepartamentosTabla() {
        $http.post("Departamentos/getDepartamentosTabla").then(function (r) {
            if (r.data.cod == "OK") {
                vm.lista.data = r.data.d.departamentosTabla
                vm.lista.disp = [].concat(vm.lista.data);
            }
        })
    }
    function getEmpleadosDropdown() {
        $http.post("lateral/getNombresDropdown").then(function (r) {
            if (r.data.cod == "OK") {
                vm.listaEmpleados.data = r.data.d.data
                console.log(vm.listaEmpleados.data);
            }
        })
    }

}