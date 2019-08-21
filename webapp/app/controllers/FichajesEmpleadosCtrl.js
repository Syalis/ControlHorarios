angular.module('webapp').controller('FichajesEmpleadosCtrl', ["$scope", "$http", "$window", FichajesEmpleadosCtrl]);

function FichajesEmpleadosCtrl($scope, $http, $window) {
    var vm = this;
    vm.session = $window.sessionStorage;

    //Declaracion de variables
    vm.fichajesTotales = [];
    vm.fichajesMes = [];
   
    vm.contador = 0;


    //Declaracion de funciones
    vm.getDatosInicioFichajes = getDatosInicioFichajes;
   
    vm.agrupacionPorDia = agrupacionPorDia;
    vm.sumarContador = sumarContador;
    vm.restarContador = restarContador;

    //Init
    //getDatosInicioFichajes();


    //Funciones

    //Carga Dropdown
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
    //Fin carga dropdown

    //Funcion de muestra de fichajes mes actual (inicio) 
    function getDatosInicioFichajes(id) {
        $http.post("FichajesEmpleados/getInicioFichajes", { id: id }).then(function (r) {
            if (r.data.cod == "OK") {
                vm.fichajesTotales = r.data.d.fichajesTotales;
                vm.fichajesMes = r.data.d.mesFichajes;
                
                agrupacionPorDia();

            }
        });
    }
   

    //Funcion para hacer añadir horas
    function agrupacionPorDia() {
        for (var i = 0; i < vm.fichajesTotales.length; i++) {
            for (var j = 0; j < vm.fichajesMes.length; j++) {
                if (vm.fichajesTotales[i].fecha === vm.fichajesMes[j].fecha) {
                    vm.fichajesTotales[i].horas = vm.fichajesMes[j].horas
                }
            }

        }
        

    }


    
    //Funcion para suma contador
    function sumarContador() {
        ++vm.contador;
        $http.post("FichajesEmpleados/mesResta", { id: vm.listaDropdown[0].id, nMes: vm.contador }).then(function (r) {
            if (r.data.cod == "OK") {
                vm.fichajesTotales = r.data.d.fichajesTotalesResta;
                vm.fichajesMes = r.data.d.mesFichajesResta;
                
                agrupacionPorDia();

            }
        })
    }
    //Funcion para resta contador
    function restarContador() {
        --vm.contador;
        $http.post("FichajesEmpleados/mesResta", { id: vm.listaDropdown[0].id, nMes: vm.contador }).then(function (r) {
            if (r.data.cod == "OK") {
                vm.fichajesTotales = r.data.d.fichajesTotalesResta;
                vm.fichajesMes = r.data.d.mesFichajesResta;
                            
                agrupacionPorDia();

            }
        })

    }


}