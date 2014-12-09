(function() {
    'use strict';

    angular.module('np.validation')
        .directive('npValidateUsername', npValidateUsername);

    npValidateUsername.$inject = ['$q', 'Auth'];

    function npValidateUsername($q, Auth) {
        var dir = {
            require: 'ngModel',
            restrict: 'A',
            link: link
        };

        return dir;

        function link(scope, element, attrs, ngModel) {
            ngModel.$asyncValidators.username = validateUserName;

            function validateUserName(modelValue, viewValue) {
                //ngModel.$setValidity('username', true);

                return Auth.userExists(viewValue)
                    .then(userExistsSuccess, userExistsError);
            }
        }

        function userExistsSuccess(response) {
            return !response.data || $q.reject(false);
        }

        function userExistsError(reason) {
            return true;
        }
    }

})();