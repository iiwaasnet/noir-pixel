(function() {
    'use strict';

    angular.module('np', ['ng', 'ui.router', 'ngMessages',
        'np.logging', 'np.i18n', 'np.home', 'np.layout', 'np.auth'
        ]);   
})();