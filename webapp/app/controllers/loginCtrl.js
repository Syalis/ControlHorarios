angular.module('webapp').controller('loginCtrl', ['$scope', '$http', '$window', '$location', '$document', loginCtrl]);


function loginCtrl($scope, $http, $window, $location, $document) {


    //    var vm = this;
    //    //variables a usar
    //    vm.usuario = "";
    //    vm.contraseña = "";
    //    vm.loading = false;

    //    //Funcion para mostrar y ocultar la funcion ¿no recuerdas tu contraseña?.
    //    vm.ShowHide = function () {
    //        vm.visible = false;
    //        vm.visible = vm.visible = true;
    //    }
    //    vm.Enviar = function () {
    //        vm.visible = false;
    //        vm.visible = vm.visible = false;
    //    }
    //    // fin de funcion (aun en desarrollo)


    //    //funcion login
    //    vm.login = login;
    //    //
    //    function login() {
    //        if (vm.usuario && vm.contraseña) {
    //            $http.post("Account/LoginUser", { Usuario: vm.usuario, Contraseña: vm.contraseña }).then(function (response) {
    //                if (response.data.cod == "OK") {
    //                    console.log(response.data);
    //                }
    //                else {
    //                    swal({ title: 'Oops...', text: response.data.msg, type: 'error' })
    //                }
    //            }
    //        }

    //    }

    //}

}
///////////////////////////////////////////


    ////Variables
    //vm.usuario = "";
    //vm.password = "";
    //vm.loading = false;
    ////////////////////////////////

    ////Funciones
    //vm.login = login;
    ////////////////////////////////

    ////INIT
    //$window.sessionStorage.clear();
    ////////////////////////////////

    ////Estilo
    //$document[0].body.style.height = "100%";
    ///////////////////////////////////////////////

    //var newUrl = $location.search().ReturnUrl;

    //function login() {
    //    if (vm.usuario && vm.password) {
    //        vm.loading = true;
    //        $http.post("Account/LoginUser", { usuario: vm.usuario, pass: vm.password })
    //        .then(function (response) {
    //            if (response.data.cod === "OK") {
    //                $window.sessionStorage.webroot = webroot;
    //                $window.sessionStorage.usuario = response.data.d.usuario;

    //                if (newUrl == undefined) {
    //                    $window.location.href = webroot + response.data.d.url;
    //                } else {
    //                    $window.location.href = webroot;
    //                }

    //            } else {
    //                vm.loading = false;
    //                swal({ title: 'Oops...', text: response.data.msg, type: 'error' });
    //            }
    //        });
    //    }
    //}
