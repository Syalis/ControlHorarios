﻿angular.module('webapp').controller('loginInicioCtrl', ['$scope', '$http', '$window', '$location', '$document', loginInicioCtrl]);


function loginInicioCtrl($scope, $http, $window, $location, $document) {

    ////Funcion para mostrar y ocultar la funcion ¿no recuerdas tu contraseña?.
    //vm.ShowHide = function () {
    //    vm.visible = false;
    //    vm.visible = vm.visible = true;
    //}
    //vm.Enviar = function () {
    //    vm.visible = false;
    //    vm.visible = vm.visible = false;
    //}
    //// fin de funcion (aun en desarrollo)

    //        vm.login = login;
    //        //
    //        function login() {
    //            if (vm.usuario && vm.contraseña) {
    //                $http.post("Account/LoginUser", { Usuario: vm.usuario, Contraseña: vm.contraseña }).then(function (response) {
    //                    if (response.data.cod == "OK") {
    //                        console.log(response.data);
    //                    }
    //                    else {
    //                        swal({ title: 'Oops...', text: response.data.msg, type: 'error' })
    //                    }
    //                }
    //            }

    //        }

    //    }

    //}
    /////////////////////////////////////////

    var vm = this;
    //Variables
    vm.usuario = "";
    vm.password = "";
    vm.loading = false;
    //////////////////////////////

    //Funciones
    vm.login = login;
    //////////////////////////////

    //INIT
    $window.sessionStorage.clear();
    //////////////////////////////

    //Estilo
    $document[0].body.style.height = "100%";
    /////////////////////////////////////////////

    var newUrl = $location.search().ReturnUrl;

    function login() {
        if (vm.usuario && vm.password) {
          
            $http.post("Account/LoginUser", { usuario: vm.usuario, pass: vm.password })
                .then(function (response) {
                    if (response.data.cod === "OK")
                       {
                        $window.sessionStorage.webroot = webroot;
                        $window.sessionStorage.usuario = response.data.d.email;
                        $window.sessionStorage.id_perfil = response.data.d.perfil
                        $window.sessionStorage.nombre = response.data.d.nombre
                        $window.sessionStorage.primer_apellido = response.data.d.primer_apellido
                        $window.sessionStorage.segundo_apellido = response.data.d.segundo_apellido

                        if (newUrl == undefined) {
                            $window.location.href = webroot + response.data.d.url;
                        } else {
                            $window.location.href = webroot;
                        }

                    } else {
                        vm.loading = false;
                        swal({ title: 'Oops...', text: response.data.msg, type: 'error' });
                    }
                });
        }
    }
}


