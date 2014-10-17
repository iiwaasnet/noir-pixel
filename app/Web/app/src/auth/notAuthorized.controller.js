(function() {
    'use strict';

    angular.module('np.auth')
        .controller('NotAuthorizedController', notAuthorizedController);

    notAuthorizedController.$injector = ['$stateParams'];

    function notAuthorizedController($stateParams) {
        var ctrl = this;
        ctrl.redirectTo = $stateParams.redirectTo || '';
    }
})();