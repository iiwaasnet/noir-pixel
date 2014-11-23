(function() {
    'use strict';

    angular.module('np.layout')
        .controller('HeaderController', headerController);

    headerController.$inject = ['$scope', 'Strings', 'Auth', 'EventsHub', 'Signin'];

    function headerController($scope, Strings, Auth, EventsHub, Signin) {
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

        function activate() {            
            buildMenus();

            EventsHub.addListener(EventsHub.events.SignedIn, onSignStatuesChanged);
            EventsHub.addListener(EventsHub.events.SignedOut, onSignStatuesChanged);

            $scope.$on('$destroy', function() {
                EventsHub.removeListener(EventsHub.events.SignedIn, onSignStatuesChanged);
                EventsHub.removeListener(EventsHub.events.SignedOut, onSignStatuesChanged);
            });
        }
    }
})();