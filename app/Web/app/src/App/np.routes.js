(function() {
    'use strict';

    angular.module('np')
        .config(config);

    config.$injector = ['$stateProvider', '$urlRouterProvider', '$locationProvider'];
    bootstrapData.$injector = ['Strings'];

    function config($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '/app/src/Home/home.html',
                controller: 'HomeController as ctrl',
                //resolve: { strings: function(strings) { return strings; } }
            })
            .state('gallery', {
                url: '/',
                templateUrl: '/app/src/Home/home.html',
                controller: 'HomeController as ctrl'
            })
            .state('signIn', {
                url: '/sign-in',
                templateUrl: '/app/src/Auth/sign-in.html',
                controller: 'SignInController as ctrl'
            })
            .state('signUp', {
                url: '/sign-up',
                templateUrl: '/app/src/Auth/sign-up.html',
                controller: 'SignUpController as ctrl'
            });

        $urlRouterProvider.otherwise('/');

        $locationProvider.html5Mode(true);
    }

    function bootstrapData(Strings) {
        return Strings.init();
    }
})();