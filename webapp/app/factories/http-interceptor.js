angular.module('webapp').factory('redirectInterceptor', ['$q', '$location', '$window', function ($q, $location, $window) {
    return {
        'response': function (response) {
            if (typeof response.data === 'string' && response.data.indexOf("Login") > -1) {
                console.log("LOGIN!!");
                console.log(response.data);
                $window.location.href = "Account/Login";
                return $q.reject(response);
            } else {
                return response;
            }
        }
    }
}]).factory('responseErrorInterceptor', ['$q', '$location', '$window', function ($q, $location, $window) {
    var responseErrorInterceptor = {
        responseError: function (response) {            
            if (response.status == 401 || response.status == 419) { // Session has expired
                $window.location.href = "login.aspx";
                return $q.reject(response);
            } else {
                return response;
            }
        }
    };
    return responseErrorInterceptor;
}]).factory('HttpInterceptor', ['$templateCache', 'APP_CONFIG', function ($templateCache, APP_CONFIG) {
    return {
        'request': function (request) {
            if (request.method === 'POST') {
                //Incluyo Webroot antes de la llamada para que concatene siempre en el caso de subcarpeta la ruta raiz del proyecto
                request.url = webroot + request.url;
            }
            else if (request.method === 'GET' && $templateCache.get(request.url) === undefined) {
                //Agrego Versión al Template para Evitar Caché
                request.url += '?v=' + APP_CONFIG.TPL_VERSION;
            }
            return request;
        }
    };
}]).config(['$httpProvider', function ($httpProvider) {
    $httpProvider.interceptors.push('redirectInterceptor');
    $httpProvider.interceptors.push('responseErrorInterceptor');
    $httpProvider.interceptors.push('HttpInterceptor');
}]);