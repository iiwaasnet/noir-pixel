(function() {
    'use strict';

    angular.module('np')
        .config(config);

    config.$injector = ['$stateProvider', '$urlRouterProvider', '$locationProvider'];

    function config($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '/app/src/Home/home.html',
                controller: 'HomeController',
                controllerAs: 'ctrl',
                //resolve: {
                //    Auth: 'Auth',
                //    userInfo: function (Auth) {
                //        return Auth.getUserInfo();
                //    }
                //}
            })
            .state('gallery', {
                url: '/',
                templateUrl: '/app/src/Home/home.html',
                controller: 'HomeController',
                controllerAs: 'ctrl'
            })
            .state('signIn', {
                url: '/sign-in?redirectTo',
                templateUrl: '/app/src/Auth/sign-in.html',
                controller: 'SignInController',
                controllerAs: 'ctrl'
            })
            .state('signUp', {
                url: '/sign-up',
                templateUrl: '/app/src/Auth/sign-up.html',
                controller: 'SignUpController',
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