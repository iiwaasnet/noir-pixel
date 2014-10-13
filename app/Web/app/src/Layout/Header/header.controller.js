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
                uri: 'app.gallery'
            };

            var menu = [
                gallery
            ];

            return menu;
        }

        function getSignInMenu() {
            var signIn = {
                    text: Strings.getLocalizedString('AuthMenu_SignIn'),
                    uri: 'app.signIn'
                },
                signUp = {
                    text: Strings.getLocalizedString('AuthMenu_SignUp'),
                    uri: 'app.signUp'
                };

            var menu = [
                signIn,
                signUp
            ];

            return menu;
        }
    }
})();