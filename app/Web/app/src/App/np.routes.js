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
            .state('userHome', {
                url: '/home',
                abstract: true,
                templateProvider: ['ViewResolver', '$stateParams', function(ViewResolver, $stateParams) { return ViewResolver.resolveTemplateUrl('userHome', $stateParams); }],
                controllerProvider: ['ViewResolver', '$stateParams', function(ViewResolver) { return ViewResolver.resolveController('userHome'); }],
                controllerAs: 'ctrl'
            })
            .state('userHome.profile', {
                url: '/:userName',
                templateProvider: ['ViewResolver', '$stateParams', function (ViewResolver, $stateParams) { return ViewResolver.resolveTemplateUrl('userHome.profile', $stateParams); }],
                controllerProvider: ['ViewResolver', '$stateParams', function (ViewResolver) { return ViewResolver.resolveController('userHome.profile'); }],
                controllerAs: 'profileCtrl'
            })
            //.state('userPublic', {
            //    url: '/people',
            //    abstract: true,
            //    templateProvider: ['ViewResolver', '$stateParams', function (ViewResolver, $stateParams) { return ViewResolver.resolveTemplateUrl('user', $stateParams); }],
            //    controllerProvider: ['ViewResolver', '$stateParams', function (ViewResolver) { return ViewResolver.resolveController('user'); }],
            //    controllerAs: 'ctrl'
            //})
            //.state('userPublic.photos', {
            //    url: '/:userName/photos',
            //    templateProvider: ['ViewResolver', '$stateParams', function(ViewResolver, $stateParams) { return ViewResolver.resolveTemplateUrl('userPublic.photos', $stateParams); }],
            //    controllerProvider: ['ViewResolver', '$stateParams', function(ViewResolver) { return ViewResolver.resolveController('userPublic.photos'); }],
            //    controllerAs: 'photosCtrl'
            //})
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