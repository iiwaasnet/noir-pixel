(function() {
    'use strict';

    angular.module('np')
        .config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider', '$locationProvider'];

    function config($stateProvider, $urlRouterProvider, $locationProvider) {
        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '/app/src/home/home.html',
                controller: 'HomeController',
                controllerAs: 'ctrl'
            })
            .state('user', {
                url: '/people',
                abstract: true,
                templateProvider: ['ViewResolver', '$stateParams', function(ViewResolver, $stateParams) { return ViewResolver.resolveTemplateUrl('user', $stateParams); }],
                controllerProvider: ['ViewResolver', '$stateParams', function(ViewResolver) { return ViewResolver.resolveController('user'); }],
                controllerAs: 'ctrl'
            })
            .state('user.profile', {
                url: '/:userName',
                templateProvider: ['ViewResolver', '$stateParams', function(ViewResolver, $stateParams) { return ViewResolver.resolveTemplateUrl('user.profile', $stateParams); }],
                controllerProvider: ['ViewResolver', '$stateParams', function(ViewResolver) { return ViewResolver.resolveController('user.profile'); }],
                controllerAs: 'profileCtrl'
            })
            .state('user.photos', {
                url: '/:userName/photos',
                templateProvider: ['ViewResolver', '$stateParams', function(ViewResolver, $stateParams) { return ViewResolver.resolveTemplateUrl('user.photos', $stateParams); }],
                controllerProvider: ['ViewResolver', '$stateParams', function(ViewResolver) { return ViewResolver.resolveController('user.photos'); }],
                controllerAs: 'photosCtrl'
            })
            .state('externalSignIn', {
                url: '/external-signin',
                templateUrl: '/app/src/auth/external-signin.html',
                controller: 'ExternalSignInController',
                controllerAs: 'ctrl'
            })
            .state('externalRegister', {
                url: '/external-register?external_access_token&provider&access_token_secret',
                templateUrl: '/app/src/auth/external-register.html',
                controller: 'ExternalRegisterController',
                controllerAs: 'ctrl'
            })
            .state('notAuthorized', {
                url: '/not-authorized?redirectTo',
                templateUrl: '/app/src/auth/not-authorized.html',
                controller: 'NotAuthorizedController',
                controllerAs: 'ctrl'
            });

        $urlRouterProvider.otherwise('/');

        $locationProvider.html5Mode(true);
    }
})();