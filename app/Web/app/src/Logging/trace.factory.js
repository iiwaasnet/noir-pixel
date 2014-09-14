(function () {
    'use strict';
    angular.module('np.logging')
        .constant('printStackTrace', printStackTrace)
        .factory('Trace', traceFactory);

    traceFactory.$injector = ['printStackTrace'];

    function traceFactory(printStackTrace) {
        return { print: printStackTrace };
    }
})();