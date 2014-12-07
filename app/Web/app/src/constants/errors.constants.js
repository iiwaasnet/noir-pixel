(function () {
    'use strict';

    var errors = {
        Auth: {
            UserAlreadyRegistered: 'EAPI_Auth_UserAlreadyRegistered'
        }
    };

    angular.module('np.const')
        .constant('Errors', errors);
})();