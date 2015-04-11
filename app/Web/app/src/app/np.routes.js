(function() {
    'use strict';

    angular.module('np')
        .config(config);

    config.$inject = ['$stateProvider', '$urlRouterProvider', '$locationProvider', 'States'];

    function config($stateProvider, $urlRouterProvider, $locationProvider, States) {
        $stateProvider
            .state(States.Home.Name, {
                url: '/',
                templateUrl: '/app/src/home/home.html',
                controller: 'HomeController',
                controllerAs: 'ctrl'
            })
            .state(States.UserHome.Name, {
                url: '/home',
                abstract: true,
                templateProvider: ['ViewResolver', '$stateParams', function(ViewResolver, $stateParams) { return ViewResolver.resolveTemplateUrl(States.UserHome.Name, $stateParams); }],
                controllerProvider: ['ViewResolver', '$stateParams', function(ViewResolver, $stateParams) { return ViewResolver.resolveController(States.UserHome.Name, $stateParams); }],
                controllerAs: 'ctrl',
                resolve: {
                    viewResolver: 'ViewResolver',
                    profile: 'Profile',
                    geo: 'Geo',
                    profileData: function (profile) {
                         return profile.getOwnProfile();
                    },
                    countries: function (geo) {
                        return geo.getCountries();
                    }
                }
            })
            .state(States.UserHome.Profile.Name, {
                url: '/:userName',
                abstract: true,
                templateProvider: ['ViewResolver', '$stateParams', function (ViewResolver, $stateParams) { return ViewResolver.resolveTemplateUrl(States.UserHome.Profile.Name, $stateParams); }],
                controllerProvider: ['ViewResolver', '$stateParams', function (ViewResolver, $stateParams) { return ViewResolver.resolveController(States.UserHome.Profile.Name, $stateParams); }],
                controllerAs: 'profileCtrl'
            })
            .state(States.UserHome.Profile.Public.Name, {
                url: '/public',
                templateProvider: ['ViewResolver', '$stateParams', function (ViewResolver, $stateParams) { return ViewResolver.resolveTemplateUrl(States.UserHome.Profile.Public.Name, $stateParams); }],
                controllerProvider: ['ViewResolver', '$stateParams', function (ViewResolver, $stateParams) { return ViewResolver.resolveController(States.UserHome.Profile.Public.Name, $stateParams); }],
                controllerAs: 'publicCtrl',
                resolve: {
                    countries: function (geo) {
                        return geo.getCountries();
                    }
                }
            })
            .state(States.UserHome.Profile.Private.Name, {
                url: '/private',
                templateProvider: ['ViewResolver', '$stateParams', function (ViewResolver, $stateParams) { return ViewResolver.resolveTemplateUrl(States.UserHome.Profile.Private.Name, $stateParams); }],
                controllerProvider: ['ViewResolver', '$stateParams', function (ViewResolver, $stateParams) { return ViewResolver.resolveController(States.UserHome.Profile.Private.Name, $stateParams); }],
                controllerAs: 'privateCtrl'
            })
            .state(States.UserHome.Darkroom.Name, {
                url: '/darkroom',
                templateProvider: ['ViewResolver', '$stateParams', function (ViewResolver, $stateParams) { return ViewResolver.resolveTemplateUrl(States.UserHome.Darkroom.Name, $stateParams); }],
                controllerProvider: ['ViewResolver', '$stateParams', function (ViewResolver, $stateParams) { return ViewResolver.resolveController(States.UserHome.Darkroom.Name, $stateParams); }],
                controllerAs: 'darkroomCtrl'
            })
            .state(States.UserHome.Photos.Name, {
                url: '/photos',
                templateProvider: ['ViewResolver', '$stateParams', function (ViewResolver, $stateParams) { return ViewResolver.resolveTemplateUrl(States.UserHome.Photos.Name, $stateParams); }],
                controllerProvider: ['ViewResolver', '$stateParams', function (ViewResolver, $stateParams) { return ViewResolver.resolveController(States.UserHome.Photos.Name, $stateParams); }],
                controllerAs: 'photosCtrl'
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
            .state(States.ExternalSignIn.Name, {
                url: '/external-signin',
                templateUrl: '/app/src/auth/external-signin.html',
                controller: 'ExternalSignInController',
                controllerAs: 'ctrl'
            })
            .state(States.ExternalRegister.Name, {
                url: '/external-register?external_access_token&provider&access_token_secret',
                templateUrl: '/app/src/auth/external-register.html',
                controller: 'ExternalRegisterController',
                controllerAs: 'ctrl'
            })
            .state(States.NotAuthorized.Name, {
                url: '/not-authorized?redirectTo',
                templateUrl: '/app/src/auth/not-authorized.html',
                controller: 'NotAuthorizedController',
                controllerAs: 'ctrl'
            })
            .state(States.Errors.NotFound.Name, {
                url: '404',
                templateUrl: '/app/src/errors/404.html',
                controller: 'HomeController',
                controllerAs: 'ctrl'
            });

        $urlRouterProvider.otherwise('/');

        $locationProvider.html5Mode(true);
    }
})();