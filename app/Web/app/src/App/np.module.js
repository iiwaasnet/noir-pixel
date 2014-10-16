(function() {
    'use strict';

    angular.module('np', ['ui.router',
        'np.logging', 'np.i18n', 'np.home', 'np.layout', 'np.auth'
        ]);   
})();