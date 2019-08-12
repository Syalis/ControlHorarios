angular.module('webapp').controller('homeCtrl', ["$scope", "$http", "$timeout", "$window", homeCtrl]);

function homeCtrl($scope, $http, $timeout, $window) {
    //var vm = this;
    //vm.session = $window.sessionStorage;

    //vm.fecha = moment().toDate();
    //vm.rankDistris = {};
    //vm.selloutTerminales = {};
    //vm.incentivosProducto = {};

    //vm.initData = initData;

    //initData();

    //function initData() {
    //    vm.loading = true;
    //    $http.post("Home/getData", { fecha: vm.fecha})
    //    .then(function (r) {
    //        if (r.data.cod == "OK") {
    //            vm.forecast = r.data.d.forecast;
    //            vm.indicadoresMes = r.data.d.indicadores_mes;
    //            vm.rankDistris = { data: r.data.d.rank_distribuidores, disp: [].concat(vm.rankDistris.data) };
    //            vm.selloutTerminales = { data: r.data.d.sellout_terminales, disp: [].concat(vm.selloutTerminales.data) };
    //            vm.incentivosProducto = { data: r.data.d.incentivos_producto, disp: [].concat(vm.incentivosProducto.data) };
    //            vm.stockGeneral = { data: r.data.d.stock_general, disp: [].concat(vm.stockGeneral.data) };
    //        }
    //        vm.loading = false;
    //    });
    //}
}