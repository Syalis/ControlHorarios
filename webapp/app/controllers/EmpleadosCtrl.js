
angular.module('webapp').controller('EmpleadosCtrl', ['$scope', '$http', '$window', '$location', '$document', EmpleadosCtrl]);


function EmpleadosCtrl($scope, $http, $window, $location, $document) {

    //variables
    var vm = this;

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