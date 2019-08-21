angular.module('webapp').controller('EmpleadosCtrl', ["$scope", "$http", "$window", EmpleadosCtrl]);

function EmpleadosCtrl($scope, $http, $window) {
    var vm = this;
    vm.session = $window.sessionStorage;

    //Declaracion de variables
    vm.lista = { data: [], disp: [], filter: [] };
    vm.listaEmpleados = { data: [], disp: [], filter: [] };
    vm.listaDepartamentos = { data: [], disp: [], filter: [] };

    //Declaracion de funciones
    vm.getEmpleadosTabla = getEmpleadosTabla;
    vm.getEmpleadosDropdown = getEmpleadosDropdown;
    vm.getDepartamentosDropdown = getDepartamentosDropdown;
    vm.getDepartamentoEmpleados = getDepartamentoEmpleados;

    //Init
    getEmpleadosTabla();
    getEmpleadosDropdown();
    getDepartamentosDropdown();

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
    //Funcion para cargar empleados en dropdown
    function getEmpleadosDropdown() {
        $http.post("lateral/getNombresDropdown").then(function (r) {
            if (r.data.cod == "OK") {
                vm.listaEmpleados.data = r.data.d.data;
                
            }
        })
    }
    //Funcion para cargar departamentos en dropdown
    function getDepartamentosDropdown() {
        $http.post("Departamentos/getDepartamentosTotal").then(function (r) {
            if (r.data.cod == "OK") {
                vm.listaDepartamentos.data = r.data.d.data;
                
            }
        })
    }
    //Metodo para filtrar por id de departamento en la tabla mediante dropdown
    function getDepartamentoEmpleados(id) {
        $http.post("Departamentos/getDepartamentoEmpleados", { id: id }).then(function (r) {
            if (r.data.cod == "OK") {
                vm.lista.data = r.data.d.getDepartamentoEmpleados
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