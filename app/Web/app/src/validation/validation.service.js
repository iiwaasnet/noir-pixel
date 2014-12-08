(function() {
    'use strict';

    angular.module('np.validation')
        .service('Validation', validation);

    validation.$inject = ['Errors'];

    function validation(Errors) {
        var srv = this,
            existingErrors = getExistingErrors();
        srv.setValidationErrors = setValidationErrors;
        srv.knownError = knownError;


        function knownError(errorCode) {
            return ~existingErrors.indexOf(errorCode);
        }

        function setValidationErrors(form, errors) {
            if (form) {
                angular.forEach(errors, function(e) { setModelError(form, e); });
            }
        }

        function getExistingErrors() {
            var errorCodes = [];
            angular.forEach(Object.keys(Errors), function(key) { getKeyValue(Errors, key, errorCodes); });

            return errorCodes;
        }

        function getKeyValue(obj, key, array) {
            var val = obj[key];

            var keys = (typeof(val) === 'object')
                ? Object.keys(val)
                : [];
            if (keys.length === 0) {
                array.push(val);
            } else {
                angular.forEach(keys, function(k) { getKeyValue(val, k, array); });
            }
        }

        function setModelError(form, error) {
            if (error.field && error.code) {
                var control = form[error.field];
                if (control) {
                    control.$error[error.code] = true;
                }
            }
        }
    }
})();