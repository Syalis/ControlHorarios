angular.module('webapp').component('topDatepicker', {
    templateUrl: '../app/components/top-datepicker/top-datepicker.html',
    bindings: {
        placeholder: '@',
        ngModel: '=',
        ngDisabled: '<',
        ngRequired: '<'
    }, controller: function () {
        var vm = this;

        vm.$onInit = function () {
            var d = new Date();
            vm.id = "dp" + d.getYear() + d.getMonth() + d.getDay() + d.getHours() + d.getMinutes() + d.getSeconds() + d.getMilliseconds();
            vm.config = {
                dropdownSelector: "#" + vm.id
            }
        }

        
    }
});