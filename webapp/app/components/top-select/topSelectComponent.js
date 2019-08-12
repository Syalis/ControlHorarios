angular.module('webapp').component('topSelect', {
    templateUrl: webroot+'app/components/top-select/topSelectTemplate.html',
    bindings: {
        model: '=',
        output: '=',
        label: '@',
        onClose: '@',
        mode:'@'
    }, controller: function () {
        var vm = this;

        vm.$onInit = function () {
            var a = 2;
        }

        
    }
});