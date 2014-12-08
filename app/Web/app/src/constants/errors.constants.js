(function () {
    'use strict';

    var errors = {
        Auth: {
            UserAlreadyRegistered: 'EAPI_Auth_UserAlreadyRegistered',
            RegistrationError: 'EAPI_Auth_RegistrationError'
        },
        Validation: {
            RequiredValue: "EAPI_ValueRequired"
        }
    };

    angular.module('np.const')
        .constant('Errors', errors);
})();