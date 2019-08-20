angular.module('webapp').controller('EmpleadosCtrl', ["$scope", "$http", "$window", EmpleadosCtrl]);

function EmpleadosCtrl($scope, $http, $window) {
    var vm = this;
    vm.session = $window.sessionStorage;

    //Declaracion de variables
    vm.lista = { data: [], disp: [], filter: [] };
    //Declaracion de funciones
    vm.getEmpleadosTabla = getEmpleadosTabla
    //Init
    getEmpleadosTabla();
    //Funciones
    //funcion para cargar todos los empleados en la tabla
    function getEmpleadosTabla() {
        $http.post("EmpleadosTabla/getEmpleadosTabla").then(function (r) {
            if (r.data.cod == "OK") {
                vm.lista.data = r.data.d.tablaEmpleado
                vm.lista.disp = [].concat(vm.lista.data);
            }
        })
    }


    //Funciones de Jesus para el modal
    //funciones
    vm.InsertUser = InsertUser;

    vm.datosUser = {};
    function InsertUser() {
        $http.post("CreateUser/InsertUser", { data: vm.datosUser }).then(function (resp) {
            var respuesta = resp.data;


            if (respuesta.cod == "OK") {
                toastr.success("ususario creado");
            }
            else
                toastr.error(respuesta.msg);
        });
    }

    vm.UpdateUser = UpdateUser;
    vm.UpadateUserDatos = {};
    function UpdateUser() {
        $http.post("CreateUser/UpdateUser", { data: vm.UpadateUserDatos }).then(function (r) {
            var respuesta = resp.data;


            if (respuesta.cod == "OK") {
                toastr.success("ususario creado");
            }
            else
                toastr.error(respuesta.msg);
        });
    }

}