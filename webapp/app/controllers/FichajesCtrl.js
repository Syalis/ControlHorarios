angular.module('webapp').controller('FichajesCtrl', ["$scope", "$http", "$window", FichajesCtrl]);

function FichajesCtrl($scope, $http, $window) {
    var vm = this;
    vm.session = $window.sessionStorage;

    //Declaracion de variables
    vm.fichajesTotales = [];
    vm.fichajesMes = [];
    vm.fichajesBoton = [];
    vm.contador = 0;


    //Declaracion de funciones
    vm.getDatosInicioFichajes = getDatosInicioFichajes;
    vm.empleadoCheckIn = empleadoCheckIn;
    vm.empleadoCheckOut = empleadoCheckOut;
    vm.comprobacionBoton = comprobacionBoton;
    vm.agrupacionPorDia = agrupacionPorDia;
    vm.sumarContador = sumarContador;
    vm.restarContador = restarContador;
   
    //Init
    getDatosInicioFichajes();
    
    
    //Funciones



    //Funcion de muestra de fichajes mes actual (inicio) 
    function getDatosInicioFichajes() {
        $http.post("Fichajes/getInicioFichajes", { id: vm.session.id }).then(function (r) {
            if (r.data.cod == "OK") {
                vm.fichajesTotales = r.data.d.fichajesTotales;
                vm.fichajesMes = r.data.d.mesFichajes;
                vm.fichajesBoton = r.data.d.boton;
                console.log(r.data.d);
                comprobacionBoton();
                agrupacionPorDia();

            }
        });
    }
    //Funcion de comprobacion de boton de check
    function comprobacionBoton() {
        for (var i = 0; i < vm.fichajesBoton.length; i++) {
            if (vm.fichajesBoton[i].hora_salida == null) {
                $('#checkIn').prop('disabled', true);
                $('#checkOut').prop('disabled', false);
                break;
            } else {
                $('#checkIn').prop('disabled', false);
                $('#checkOut').prop('disabled', true);
            }
        }
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
        console.log(vm.fichajesTotales)

    }
    

    //Funcion para hacer CheckIn
    function empleadoCheckIn() {
    $http.post("Fichajes/CheckIn", {
          checkIn: {
            id_usuario: vm.session.id
            }
          }).then(function (r) {
             getDatosInicioFichajes();

          });
    }


    //Funcion para hacer CheckOut
    function empleadoCheckOut() {
    $http.post("Fichajes/CheckOut", {
          checkOut: {
            id_usuario: vm.session.id
            }
          }).then(function (r) {
             getDatosInicioFichajes();

          });
    }
    //Funcion para suma contador
    function sumarContador() {
        ++vm.contador;
        $http.post("Fichajes/mesResta", { id: vm.session.id, nMes: vm.contador }).then(function (r) {
            if (r.data.cod == "OK") {
                vm.fichajesTotales = r.data.d.fichajesTotalesResta;
                vm.fichajesMes = r.data.d.mesFichajesResta;
                vm.fichajesBoton = r.data.d.boton;
                console.log(r.data.d);
                comprobacionBoton();
                agrupacionPorDia();

            }
        })
    }
    //Funcion para resta contador
    function restarContador() {
        --vm.contador;
        $http.post("Fichajes/mesResta", { id: vm.session.id, nMes: vm.contador }).then(function (r) {
            if (r.data.cod == "OK") {
                vm.fichajesTotales = r.data.d.fichajesTotalesResta;
                vm.fichajesMes = r.data.d.mesFichajesResta;
                vm.fichajesBoton = r.data.d.boton;
                console.log(r.data.d);
                comprobacionBoton();
                agrupacionPorDia();

            }
        })

    }


}
