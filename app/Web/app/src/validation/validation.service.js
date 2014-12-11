(function() {
    'use strict';

    angular.module('np.validation')
        .service('Validation', validation);

    validation.$inject = [];

    function validation() {
        var srv = this;
        srv.tryParseError = tryParseError;

        function tryParseError(error) {
            var errorCode = '';
            var placeholders = {};

            if (error) {
                errorCode = error.code || errorCode;
                placeholders = error.placeholderValues || placeholders;

                if (error.errors && error.errors.length > 0) {
                    errorCode = error.errors[0].code || errorCode;
                    placeholders = error.errors[0].placeholderValues || placeholders;
                }
            }

            return {
                errorCode: errorCode,
                placeholders: placeholders
            };
        }
    }
})();