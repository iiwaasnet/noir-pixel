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
            //.state('signIn', {
            //    url: '/signin?redirectTo',
            //    templateUrl: '/app/src/Auth/signin.html',
            //    controller: 'SignInController',
            //    controllerAs: 'ctrl'
            //})
            .state('externalSignIn', {
                url: '/external-signin',
                templateUrl: '/app/src/Auth/external-signin.html',
                controller: 'ExternalSignInController',
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