(function () {
    'use strict';

    angular.module('np.logging')
        .constant('printStackTrace', printStackTrace)
        .factory('Trace', traceFactory);

    traceFactory.$inject = ['printStackTrace'];

    function traceFactory(printStackTrace) {
        return { print: printStackTrace };
    }
})();