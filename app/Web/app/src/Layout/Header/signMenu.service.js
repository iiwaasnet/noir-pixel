(function() {
    'use strict';

    angular.module('np.layout')
        .service('SingMenu', signMenuService);

    signMenuService.$inject = ['Auth', 'Strings'];

    function signMenuService(Auth, Strings) {
        var service = this;
        service.getSignInMenu = getSignInMenu;

        function getSignInMenu() {
            var signIn = {
                text: Strings.getLocalizedString('AuthMenu_SignIn'),
                uri: 'signIn'
            },
            signUp = {
                text: Strings.getLocalizedString('AuthMenu_SignUp'),
                uri: 'signUp'
            };

            var menu = [
                signIn,
                signUp
            ];

            return menu;
        }
    }
})();