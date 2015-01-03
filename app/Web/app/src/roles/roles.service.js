(function() {
    'use strict';

    angular.module('np.roles')
        .service('Roles', rolesService);

    rolesService.$inject = ['User', 'EventsHub'];

    function rolesService(User, EventsHub) {
        var srv = this,
            roles = { admin: 'Admin' };
        srv.isAdmin = isAdmin;

        activate();

        function isAdmin() {
            var userData = User.getUserData();
            return authenticated()
                && userData
                && userData.roles
                && ~userData.roles.indexOf(roles.admin);
        }

        function activate() {
            EventsHub.addListener(EventsHub.events.Auth.SignedIn, signedIn);
            EventsHub.addListener(EventsHub.events.Auth.SignedOut, signedOut);
        }

        function signedIn() {}

        function signedOut() {}
    }
})();