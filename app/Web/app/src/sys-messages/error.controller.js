(function(){
    'use strict';

    angular.module('np.messages')
        .controller('ErrorController', errorController);

    errorController.$inject = ['error'];

    function errorController(error) {
        var ctrl = this;
        ctrl.error = error;
    }
})();