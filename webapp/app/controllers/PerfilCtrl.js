angular.module('webapp').controller('PerfilCtrl', ["$scope", "$http", "$window", PerfilCtrl]);

function PerfilCtrl($scope, $http, $window) {
    var vm = this;
    vm.session = $window.sessionStorage;

    //Declaracion de variables
    vm.nombreUsuario = {};
    vm.emailUsuario = {};
    vm.departamentoUsuario = {};
    //vm.setPass = setPass;
    
    vm.listaDepartamentos = { data: [], disp: [], filter: [] };

    //Declaracion de funciones
    vm.setUsuario = setUsuario;
    vm.setEmail = setEmail;
    vm.setDepartamento = setDepartamento;
    
    vm.getDepartamentosDropdown = getDepartamentosDropdown;

    //Init
    getDepartamentosDropdown();

    //Funciones
    //Funcion para cambiar nombre y apellidos del usuario
    function setUsuario() {
        $http.post("Perfil/setNombre", { id: vm.session.id, nombreUsuario: vm.nombreUsuario }).then(function (r) {
            if (r.data.cod == "OK") {
                vm.session.nombre = vm.nombreUsuario.nombre;
                vm.session.primer_apellido = vm.nombreUsuario.primer_apellido;
                vm.session.segundo_apellido = vm.nombreUsuario.segundo_apellido;
                Swal.fire({
                    position: 'top-end',
                    type: 'success',
                    title: 'Petición correta!',
                    text: 'Nombre y apellidos cambiados!',
                    showConfirmButton: false,
                    timer: 1700
                })
            } else {
                //Swal.fire({
                //    position: 'top-end',
                //    type: 'error',
                //    title: 'Petición incorreta!',
                //    text: 'Revisa los campos!',
                //    showConfirmButton: false,
                //    timer: 1700
                //})
            }
        })
    }

    //function setPass() {
    //    $http.post("Perfil/setNombre", { pass: vm.pass}).then(function (r) {
    //        if (r.data.cod == "OK") {
    //            vm.session.nombre = vm.nombreUsuario.nombre;
    //            vm.session.primer_apellido = vm.nombreUsuario.primer_apellido;
    //            vm.session.segundo_apellido = vm.nombreUsuario.segundo_apellido;
    //            Swal.fire({
    //                position: 'top-end',
    //                type: 'success',
    //                title: 'Petición correta!',
    //                text: 'Nombre y apellidos cambiados!',
    //                showConfirmButton: false,
    //                timer: 1700
    //            })
    //        } else {
    //            //Swal.fire({
    //            //    position: 'top-end',
    //            //    type: 'error',
    //            //    title: 'Petición incorreta!',
    //            //    text: 'Revisa los campos!',
    //            //    showConfirmButton: false,
    //            //    timer: 1700
    //            //})
    //        }
    //    })
    //}
    //Funcion actualizar el email
    function setEmail() {
        $http.post("Perfil/setEmail", { id: vm.session.id, emailUsuario: vm.emailUsuario }).then(function (r) {
            if (r.data.cod == "OK") {
                vm.session.usuario = vm.emailUsuario.email;
                
                Swal.fire({
                    position: 'top-end',
                    type: 'success',
                    title: 'Petición correta!',
                    text: 'Email cambiado',
                    showConfirmButton: false,
                    timer: 1700
                })
            } else {
                //Swal.fire({
                //    position: 'top-end',
                //    type: 'error',
                //    title: 'Petición incorreta!',
                //    text: 'Revisa los campos!',
                //    showConfirmButton: false,
                //    timer: 1700
                //})
            }
        })
    }
    //Funcion actualizar departamento
    function setDepartamento() {
        $http.post("Perfil/setDepartamento", { id1: vm.session.id, departamentoUsuario: vm.listaDepartamentos.select[0].id }).then(function (r) {
            if (r.data.cod == "OK") {
                vm.session.departamento = vm.departamentoUsuario;
                
                Swal.fire({
                    position: 'top-end',
                    type: 'success',
                    title: 'Petición correta!',
                    text: 'Departamento cambiado!',
                    showConfirmButton: false,
                    timer: 1700
                })
            } else {
                //Swal.fire({
                //    position: 'top-end',
                //    type: 'error',
                //    title: 'Petición incorreta!',
                //    text: 'Revisa los campos!',
                //    showConfirmButton: false,
                //    timer: 1700
                //})
            }
        })
    }
    //Funcion para cargar departamentos en dropdown
    function getDepartamentosDropdown() {
        $http.post("Departamentos/getDepartamentosTotal").then(function (r) {
            if (r.data.cod == "OK") {
                vm.listaDepartamentos.data = r.data.d.data;

            }
        })
    }
    
}