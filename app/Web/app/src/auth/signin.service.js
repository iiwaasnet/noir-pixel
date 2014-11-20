﻿(function() {
    'use strict';

    angular.module('np.auth')
        .service('Signin', signinService);

    signinService.$inject = ['$controller', '$rootScope', '$injector', '$http', 'Auth', 'ngDialog'];

    function signinService($controller, $rootScope, $injector, $http, Auth, ngDialog) {
        var srv = this;
        srv.signin = signin;

        function signin() {
            ngDialog.open({
                template: 'app/src/auth/signin.html',
                appendTo: '#signin-menu-target',
                cache: true,
                controller: 'SignInController as ctrl',
                showClose: false
            });
        }
    }
})();