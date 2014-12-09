(function() {
    'use strict';

    angular.module('np.validation')
        .directive('npUniqueUsername', npUniqueUsername);

    npUniqueUsername.$inject = ['$q', 'Auth'];

    function npUniqueUsername($q, Auth) {
        var dir = {
            require: 'ngModel',
            restrict: 'A',
            link: link
        };

        return dir;

        function link(scope, element, attrs, ngModel) {
            ngModel.$asyncValidators.npUniqueUsername = checkUserNameUnique;

            function checkUserNameUnique(modelValue, viewValue) {
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