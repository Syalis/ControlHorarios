angular.module('webapp').controller('FichajesCtrl', ["$scope", "$http", FichajesCtrl]);

function FichajesCtrl($scope, $http) {
    var vm = this;

    //Declaracion de variables
    vm.fichajesEmpleado = { data: [], disp: [], filter: [] };
    
    //Declaracion de funciones
    vm.getInicioFichajes = getInicioFichajes;
    
    //Init
    getInicioFichajes();

    //Funciones



    //Funcion de muestra de fichajes mes actual (inicio)
    function getInicioFichajes() {
        $http.post("Fichajes/getInicioFichajes", { id: 2}).then(function (r) {
            vm.fichajesEmpleado.data = r.data.d.data;
            console.log(vm.fichajesEmpleado.data);

        });
    }

    


}