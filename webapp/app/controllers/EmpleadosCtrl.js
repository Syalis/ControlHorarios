
angular.module('webapp').controller('EmpleadosCtrl', ['$scope', '$http', '$window', '$location', '$document', EmpleadosCtrl]);


function EmpleadosCtrl($scope, $http, $window, $location, $document) {

    //variables
    var vm = this;

    //funciones
    vm.InsertUser = InsertUser;

    vm.datosUser = {};
    function InsertUser() {
        $http.post("CreateUser/InsertUser", { data: vm.datosUser }).then(function (r) { console.log(vm.datosUser) });

    }

    vm.UpdateUser = UpdateUser;
    vm.UpadateUserDatos = {};
    function UpdateUser() {
        $http.post("CreateUser/UpdateUser", { data: vm.UpadateUserDatos }).then(function (r) { console.log(vm.UpadateUserDatos) });
       
    }


}