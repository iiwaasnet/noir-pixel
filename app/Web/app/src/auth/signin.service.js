(function() {
    'use strict';

    angular.module('np.auth')
        .service('Signin', signinService);

    signinService.$inject = ['$controller', '$rootScope', '$injector', '$http', 'Auth'];

    function signinService($controller, $rootScope, $injector, $http, Auth) {
        var srv = this;
        srv.signin = signin;

        function signin() {
            debugger;
            $http.get('http://noir-pixel.com/app/src/auth/signin.html').then(getViewSuccess, getViewError);
            //var ctor = $injector.invoke('SignInController');
            //var ctrl = $controller('SignInController', { $scope: $rootScope.$new() });
        }

        function getViewSuccess(response) {
            debugger;
            var target = angular.element(document.getElementById('main-menu-target'));
            target.html(response.data);
        }

        function getViewError(err) {
        }
    }
})();