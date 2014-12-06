(function() {
    'use strict';

    angular.module('np')
        .config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider', '$locationProvider'];

    function config($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '/app/src/Home/home.html',
                controller: 'HomeController',
                controllerAs: 'ctrl',
            })
            .state('gallery', {
                url: '/',
                templateUrl: '/app/src/Home/home.html',
                controller: 'HomeController',
                controllerAs: 'ctrl'
            })
            .state('externalSignIn', {
                url: '/external-signin',
                templateUrl: '/app/src/Auth/external-signin.html',
                controller: 'ExternalSignInController',
                controllerAs: 'ctrl'
            })
            .state('externalRegister', {
                url: '/external-register?external_access_token&provider&access_token_secret',
                templateUrl: '/app/src/Auth/external-register.html',
                controller: 'ExternalRegisterController',
                controllerAs: 'ctrl'
            })
            .state('notAuthorized', {
                url: '/not-authorized?redirectTo',
                templateUrl: '/app/src/Auth/not-authorized.html',
                controller: 'NotAuthorizedController',
                controllerAs: 'ctrl'
        });

        $urlRouterProvider.otherwise('/');

        $locationProvider.html5Mode(true);
    }
})();