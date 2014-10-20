(function() {
    'use strict';

    angular.module('np.layout')
        .controller('HeaderController', headerController);

    headerController.$injector = ['$scope', 'Strings', 'Auth', 'EventsHub'];

    function headerController($scope, Strings, Auth, EventsHub) {
        var ctrl = this;
        
        activate();

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
            var signIn = {
                    text: Strings.getLocalizedString('AuthMenu_SignIn'),
                    uri: 'signIn'
                },
                signUp = {
                    text: Strings.getLocalizedString('AuthMenu_SignUp'),
                    uri: 'signUp'
                },
                signOut = {
                    text: Strings.getLocalizedString('AuthMenu_SignOut'),
                    uri: 'signOut'
                };

            var menu = [];

            if (Auth.authenticated()) {
                menu.push(signOut);
            } else {
                menu.push(signIn);
                menu.push(signUp);
            }
            
            return menu;
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

            $scope.$on('$destroy', function () {
                EventsHub.removeListener(EventsHub.events.SignedIn, onSignStatuesChanged);
                EventsHub.removeListener(EventsHub.events.SignedOut, onSignStatuesChanged);
            });
        }
    }
})();