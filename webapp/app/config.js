function configState($stateProvider, $urlRouterProvider, $compileProvider) {
    $compileProvider.debugInfoEnabled(true);
}
angular
    .module('webapp')
    .config(['$stateProvider', '$urlRouterProvider', '$compileProvider', configState])
    .config(['stConfig', function (stConfig) {  // Global Config Smart Table
        stConfig.sort.delay = false;
        stConfig.search.delay = false;
    }])
    .config(['$locationProvider', function ($locationProvider) {
        // enable HTML5mode to disable hashbang urls
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false
        });
    }])
    .config(['$httpProvider', function config($httpProvider) {
        //Intercepta todas las llamadas $http, y convierte los JsonDate (/Date(XXXXXXXXXXXXXXXX)/) a Javascript Date
        var dateRegex = /\/Date\((\-?\d*?)([\+\-]\d*)?\)\//
        function recurseObject(object) {
            var result = object;
            if (object !== null && object !== undefined) {
                result = angular.copy(object);
                for (var key in result) {
                    var property = result[key];
                    if (typeof property === 'object') {
                        result[key] = recurseObject(property);
                    } else if (typeof property === 'string' && dateRegex.test(property)) {
                        result[key] = moment(property).toDate();
                    }
                }
            }
            return result;
        }

        $httpProvider.defaults.transformResponse = function (data) {
            try {
                var object;
                if (typeof data === 'object') {
                    object = data;
                } else {
                    object = JSON.parse(data);
                }
                return recurseObject(object);
            } catch (e) {
                return data;
            }
        };
    }])
    .run(['$rootScope', '$state', function ($rootScope, $state) {
        $rootScope.$state = $state;
    }])
    .run(['editableOptions', function (editableOptions) {
        editableOptions.theme = 'bs3'; // bootstrap3 theme. Can be also 'bs2', 'default' xeditable
    }])
    .run(['confirmationPopoverDefaults', function (confirmationPopoverDefaults) {
        //Angular bootstrap confirm defaults
        confirmationPopoverDefaults.confirmButtonType = 'danger';
        confirmationPopoverDefaults.placement = 'top';
        confirmationPopoverDefaults.confirmText = 'Si';
        confirmationPopoverDefaults.cancelText = 'No';
    }]);