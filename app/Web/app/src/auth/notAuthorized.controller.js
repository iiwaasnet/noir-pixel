(function() {
    'use strict';

    angular.module('np.auth')
        .controller('NotAuthorizedController', notAuthorizedController);

    notAuthorizedController.$injector = ['$stateParams'];

    function notAuthorizedController($stateParams) {
    }
})();