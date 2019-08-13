
angular.module('webapp').controller('EmpleadosCtrl', ['$scope', '$http', '$window', '$location', '$document', EmpleadosCtrl]);


function EmpleadosCtrl($scope, $http, $window, $location, $document) {

//variables
    var vm = this;


    //funciones
    vm.InsertUser = InsertUser;
    

    vm.datosUser = {};
    function InsertUser() {
        $http.post("Home/InsertUser", { data: vm.datosUser }).then(function (r) { console.log(vm.datosUser) });

       


         
    }
            
    

}