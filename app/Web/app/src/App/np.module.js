(function() {
    'use strict';

    angular.module('np', ['ui.router', 'ngMessages',
        'np.logging', 'np.i18n', 'np.home', 'np.layout', 'np.auth'
        ]);   
})();