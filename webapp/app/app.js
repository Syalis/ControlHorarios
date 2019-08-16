(function() {
    angular.module('webapp',
    [
        'ui.router', // Angular flexible routing
        'ui.bootstrap', // AngularJS native directives for Bootstrap
        'angles',
        //'chart.js',
        //'ngAnimate', // Angular animations
        'smart-table',
        'angular.filter', // Angular Filters
        'ngSanitize', // Angular Filters
        'angular-bind-html-compile',
        'angular-carousel',
        'isteven-multi-select',
        'ds.clock',
        'ui.bootstrap.datetimepicker',
        'toastr',
        'xeditable',                // Angular-xeditable    -   https://github.com/vitalets/angular-xeditable
        'ui.select',                //Angular-ui-select     -   https://github.com/angular-ui/ui-select
        'mwl.confirm',                //Angular bootstrap confirm     -   https://github.com/mattlewis92/angular-bootstrap-confirm
        'datetime',
        'ui.mask',
        'angular-ladda',            // Angular-ladda (Loading Buttons)
       
    ]).constant('APP_CONFIG', {
        TPL_VERSION: 14.534534534
    });
})();

angular
    .module('webapp')
    .run(['$rootScope', '$state', function ($rootScope, $state) {
        $rootScope.$state = $state;        
    }]);

