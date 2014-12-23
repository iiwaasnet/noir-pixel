(function() {
    'use strict';

    angular.module('np.layout')
        .controller('HeaderController', headerController);

    headerController.$inject = ['$scope', '$http', 'Strings', 'Auth', 'EventsHub', 'Signin', 'Config', 'Url'];

    function headerController($scope, $http, Strings, Auth, EventsHub, Signin, Config, Url) {
        var ctrl = this;
        ctrl.mainMenu = [];
        ctrl.signInMenu = {};
        ctrl.signin = signin;
        ctrl.signout = signout;
        ctrl.login = {};

        activate();

        function signin() {
            Signin.open();
        }

        function signout() {
            Auth.signOut();
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
            getHome(data.userName);
        }

        function signedOut() {
            ctrl.login.authenticated = Auth.authenticated();
        }

        function getHome(userName) {
            var uri = Url.build(Config.ApiUris.Base, Config.ApiUris.Users.Home).formatNamed({ userName: userName });
            $http.get(uri).then(getHomeSuccess);
        }

        function getHomeSuccess(response) {
            ctrl.login.profileThumbnail = response.data.thumbnail.url;
            ctrl.login.userName = response.data.userName;
            ctrl.login.authenticated = Auth.authenticated();
            Auth.saveLoginData({ profileThumbnail: response.data.thumbnail.url });
        }

        function activate() {
            ctrl.login.authenticated = Auth.authenticated();

            if (Auth.authenticated()) {
                var loginData = Auth.getLoginData();
                if (loginData && loginData.userName) {
                    getHome(loginData.userName);
                } else {
                    Auth.signOut();
                }
            }
            
            buildMenus();

            EventsHub.addListener(EventsHub.events.Auth.SignedIn, signedIn);
            EventsHub.addListener(EventsHub.events.Auth.SignedOut, signedOut);

            $scope.$on('$destroy', function() {
                EventsHub.removeListener(EventsHub.events.Auth.SignedIn, signedIn);
                EventsHub.removeListener(EventsHub.events.Auth.SignedOut, signedOut);
            });
        }
    }
})();