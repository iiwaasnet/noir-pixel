(function() {
    'use strict';

    angular.module('np.layout')
        .controller('HeaderController', headerController);

    headerController.$inject = ['$scope', '$http', 'Strings', 'Auth', 'EventsHub', 'Signin', 'Config', 'Url'];

    function headerController($scope, $http, Strings, Auth, EventsHub, Signin, Config, Url) {
        var ctrl = this;
        ctrl.mainMenu = [];
        ctrl.signInMenu = {};
        ctrl.authenticated = Auth.authenticated();
        ctrl.signin = signin;

        activate();

        function signin() {
            Signin.open();
        }

        function getMainMenu() {
            var gallery = {
                    text: Strings.getLocalizedString('MainMenu_Gallery'),
                    uri: 'home'
                },
                exhibition = {
                    text: Strings.getLocalizedString('MainMenu_Gallery'),
                    uri: 'exhibition'
                },
                projects = {
                    text: Strings.getLocalizedString('MainMenu_Gallery'),
                    uri: 'projects'
                };

            var menu = [
                gallery,
                exhibition,
                projects
            ];

            return menu;
        }

        function getSignInMenu() {
            return {
                signOut: Strings.getLocalizedString('AuthMenu_SignOut'),
                signIn: Strings.getLocalizedString('AuthMenu_SignIn')
            };
        }

        function buildMenus() {
            ctrl.mainMenu = getMainMenu();
            ctrl.signInMenu = getSignInMenu();
        }

        function onSignStatuesChanged() {
            buildMenus();
        }

        function signedIn(data) {
            var uri = Url.build(Config.ApiUris.Base, Config.ApiUris.Users.Home.formatNamed({ userName: data.userName }));
            $http.get(uri).then(getHomeSuccess);
        }

        function getHomeSuccess(response) {
            debugger;
            ctrl.profileThumbnail = response.data.thumbnail.url;
            ctrl.authenticated = Auth.authenticated();
        }

        function activate() {            
            buildMenus();

            EventsHub.addListener(EventsHub.events.Auth.SignedIn, signedIn);
            EventsHub.addListener(EventsHub.events.Auth.SignedOut, onSignStatuesChanged);

            $scope.$on('$destroy', function() {
                EventsHub.removeListener(EventsHub.events.Auth.SignedIn, onSignStatuesChanged);
                EventsHub.removeListener(EventsHub.events.Auth.SignedOut, onSignStatuesChanged);
            });
        }
    }
})();