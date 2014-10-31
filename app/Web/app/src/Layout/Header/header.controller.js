(function() {
    'use strict';

    angular.module('np.layout')
        .controller('HeaderController', headerController);

    headerController.$injector = ['$scope', 'Strings', 'Auth', 'SignIn', 'EventsHub'];

    function headerController($scope, Strings, Auth, SignIn, EventsHub) {
        var ctrl = this;
        ctrl.signIn = signIn;
        ctrl.mainMenu = [];
        ctrl.signInMenu = {};

        activate();


        function signIn() {
            SignIn.signIn();
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
            if (Auth.authenticated()) {
                return {
                    signOut: Strings.getLocalizedString('AuthMenu_SignOut')
                }
            } else {
                return {
                    signIn: Strings.getLocalizedString('AuthMenu_SignIn'),
                    signUp: Strings.getLocalizedString('AuthMenu_SignUp')
                }
            }
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