angular.module('webapp').factory("auxVacacionesCtrl", ["$http", function ($http) {
    return {

        // Función que muestra un modal cuando la consulta viene completa.
        consultaCorrecta: function () {
            try {
                Swal.fire({
                    position: 'top-end',
                    type: 'success',
                    title: 'Petición correta! Los días de vacaciones solicitados superan a los días de vacaciones',
                    showConfirmButton: false,
                    timer: 1700
                })

            } catch (ex) {
                return ex.message;
            }

        },
        consultaInCorrecta: function () {
            try {
                Swal.fire({
                    position: 'top-end',
                    type: 'error',
                    title: 'Petición incorreta!',
                    showConfirmButton: false,
                    timer: 1700
                })

            } catch (ex) {
                return ex.message;
            }

        },
        // Función que muestra un modal cuando la fecha es incorrecta.
        validarFecha: function () {

            Swal.fire({
                type: 'error',
                title: 'Oops...Fechas erroreas!',
                text: 'Revisa los campos de las fechas!'
            })

        },
        peticionVacacionesAceptada: function () {
            try {
                Swal.fire({
                    position: 'top-end',
                    type: 'success',
                    title: 'Vacaciones aceptadas!',
                    showConfirmButton: false,
                    timer: 1700
                })

            } catch (ex) {
                return ex.message;
            }

        },
    }


}]);

