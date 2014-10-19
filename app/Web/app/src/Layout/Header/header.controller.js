(function() {
    'use strict';

    angular.module('np.layout')
        .controller('HeaderController', headerController);

    headerController.$injector = ['Strings'];

    function headerController(Strings) {
        var ctrl = this;
        ctrl.mainMenu = getMainMenu();
        ctrl.signInMenu = getSignInMenu();

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

            var menu = [
                signIn,
                signUp,
                signOut
            ];

            return menu;
        }
    }
})();