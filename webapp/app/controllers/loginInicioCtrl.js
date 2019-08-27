angular.module('webapp').controller('loginInicioCtrl', ['$scope', '$http', '$window', '$location', '$document', loginInicioCtrl]);


function loginInicioCtrl($scope, $http, $window, $location, $document) {

    //Funcion para mostrar y ocultar la funcion ¿no recuerdas tu contraseña?.
    

    function show () {
        vm.visible = false;
        vm.visible = vm.visible = true;
    }
    function enviar () {
        vm.visible = false;
        vm.visible = vm.visible = false;
    }


    var vm = this;
    //Variables
    vm.usuario = "";
    vm.password = "";
    vm.loading = false;
    vm.email = {};
    vm.pass= { };
 
    //////////////////////////////

    //Funciones
    vm.login = login;
    vm.ShowHide = show;
    vm.Enviar = enviar;
    vm.forgotPass = forgotPass;
    vm.resetPass = resetPass;
    //////////////////////////////

    //INIT

    //////////////////////////////

    //Estilo
    $document[0].body.style.height = "100%";
    /////////////////////////////////////////////

    var newUrl = $location.search().ReturnUrl;

    function login() {
        if (vm.usuario && vm.password) {
          vm.loading = true
            $http.post("Account/LoginUser", { usuario: vm.usuario, pass: vm.password })
                .then(function (response) {
                    if (response.data.cod === "OK")
                       {
                        $window.sessionStorage.webroot = webroot;
                        $window.sessionStorage.usuario = response.data.d.email;
                        $window.sessionStorage.tipo_perfil = response.data.d.tipo_perfil
                        $window.sessionStorage.id_perfil = response.data.d.id_perfil
                        $window.sessionStorage.id = response.data.d.id
                        $window.sessionStorage.departamento_nombre = response.data.d.departamento_nombre
                        $window.sessionStorage.nombre = response.data.d.nombre
                        $window.sessionStorage.primer_apellido = response.data.d.primer_apellido
                        $window.sessionStorage.segundo_apellido = response.data.d.segundo_apellido

                        if (newUrl == undefined) {
                            $window.location.href = webroot + response.data.d.url;
                        } else {
                            swal({ title: 'Oops.12345..', text: response.data.msg, type: 'error' });
                            $window.location.href = webroot;
                        }

                    } else {
                        vm.loading = false;
                        swal({ title: 'Oops...', text: response.data.msg, type: 'error' });
                    }
          
                });
        }
    }
    
   //enviar email de restablecimiento de contraseña
    function forgotPass() {
        $http.post("Account/forgotPass", { data:vm.email })
            .then(function (resp) {
                var respuesta = resp.data;

                if (respuesta.cod == "OK") {
                    swal("revisa tu bandeja de entrada");
                    
                }
                else {
                    vm.loading = false;
                    swal({ title: 'Oops...', text: resp.data.msg, type: 'success' });
                }

            });
    }

    //actualizar contraseña antigua olvidada por nueva
    function resetPass() {

        $http.post("Account/resetPass", { data: vm.pass })
            .then(function (resp) {
                var respuesta = resp.data;


                if (respuesta.cod === "OK") {
                    Swal.fire(
                        'Cambio de contraseña completado con exito!',

                    )
                    if (newUrl == undefined) {

                        $window.location.href = webroot + resp.data.d.url;
                    }
                    else {
                        $window.location.href = webroot;
                    }
                }
                else
                    swal({ title: 'Oops...', text: resp.data.msg, type: 'error' });
            });
    }

}


