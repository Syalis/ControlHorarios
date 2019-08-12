angular.module('webapp').controller('mainCtrl', ["$scope", "$http", "$timeout", "$window", mainCtrl]);

function mainCtrl($scope, $http, $timeout, $window) {
    var vm = this;

    if (!$window.sessionStorage.usuario) {
        $http.post("Account/getSessionData").then(function (r) {
            if (r.data.cod == "OK") {
                $window.sessionStorage.webroot = webroot;
                $window.sessionStorage.usuario = r.data.d.usuario;
                $window.sessionStorage.nombre = r.data.d.nombre;
                $window.sessionStorage.apellidos = r.data.d.apellidos;
                $window.sessionStorage.email = r.data.d.email;
                $window.sessionStorage.isAdmin = r.data.d.isAdmin;
            } else {
                $window.location.href = "/Account/Login";
            }
        });
    }

    vm.session = $window.sessionStorage;

    //Variables

    //Funciones
    vm.logout = logout;

    //Init

    ////////////////////

    function logout() {
        $http.post("Account/logout").then(function() {
            $window.location.href = "/Account/Login";
        });
    }
}