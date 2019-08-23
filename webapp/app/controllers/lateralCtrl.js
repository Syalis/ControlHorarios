angular.module('webapp').controller('lateralCtrl', ["$scope", "$http", "$window", lateralCtrl]);

function lateralCtrl($scope, $http, $window) {
    var vm = this;
    vm.session = $window.sessionStorage;
    vm.window = $window;
  
}