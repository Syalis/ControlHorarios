angular.module('webapp')
.factory("BD", ["$http", function ($http) {
    return {
        getDistribuidores: function () {
            return $http.post("BD/getDistribuidores", {});
        },
        getTipologiasIncentivos: function () {
            return $http.post("BD/getTipologiasIncentivos", {});
        },
        getTerritoriales: function () {
            return $http.post("BD/getTerritoriales", {});
        },
        getCanales: function () {
            return $http.post("BD/getCanales", {});
        },
        getCuentas: function () {
            return $http.post("BD/getCuentas", {});
        }
    }
}]);