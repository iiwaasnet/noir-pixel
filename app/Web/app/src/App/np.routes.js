(function() {
    'use strict';

    angular.module('np')
        .config(config);

    config.$injector = ['$stateProvider', '$urlRouterProvider', '$locationProvider'];
    bootstrapData.$injector = ['Strings'];

    function config($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider
            //.state('app', {
            //    url: '',
            //    //'abstract': true,
            //    resolve: { strings: bootstrapData }
            //})
            .state('app.home', {
                url: '/',
                templateUrl: '/app/src/Home/home.html',
                controller: 'HomeController as ctrl'
            })
            .state('app.gallery', {
                url: '/',
                templateUrl: '/app/src/Home/home.html',
                controller: 'HomeController as ctrl'
            }).state('signIn', {
                url: '/sign-in',
                templateUrl: '/app/src/Auth/sign-in.html',
                controller: 'SignInController as ctrl',
                resolve: { strings: bootstrapData }
            }).state('app.signUp', {
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