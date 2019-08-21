angular.module('webapp').controller('EmpleadosCtrl', ["$scope", "$http", "$window", '$location', EmpleadosCtrl]);

function EmpleadosCtrl($scope, $http, $window, $location) {

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
                swal("Invitacion enviada con exito!!", "success");
            }
            else
                swal({ title: 'Oops...', text: respuesta.msg, type: 'error' });
        });
    }
    var newUrl = $location.search().ReturnUrl;
    vm.UpdateUser = UpdateUser;
    vm.UpadateUserDatos = {};
    function UpdateUser() {
        $http.post("CreateUser/UpdateUser", { data: vm.UpadateUserDatos }).then(function (resp) {
            var respuesta = resp.data;


            if (respuesta.cod == "OK") {
                swal("Usuario creado con exito!!", "success");
                if (newUrl == undefined) {

                    $window.location.href = webroot + resp.data.d.url;
                }
                else {
                    $window.location.href = webroot;
                }
            }
            else
                swal({ title: 'Oops...', text: resp.data.msg, type: 'error' });
        });
    }

}