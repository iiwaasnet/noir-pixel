(function() {
    'use strict';

    angular.module('np.validation')
        .service('Validation', validation);


    function validation() {
        var srv = this;
        srv.setValidationErrors = setValidationErrors;

        function setValidationErrors(form, errors) {
            if (form) {
                angular.forEach(errors, function(e) { setModelError(form, e); });
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