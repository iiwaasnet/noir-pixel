(function() {
    'use strict';

    angular.module('np', ['ng', 'ngAnimate', 'ui.router', 'ngMessages', 'multi-select',
        'np.progress',
        'np.logging', 'np.i18n', 'np.auth', 'np.home', 'np.layout', 'np.messages',
        'np.ui-elements', 'np.utils', 'np.validation', 'np.events', 'np.images',
        'np.user', 'np.roles', 'np.view-resolver', 'np.user-home', 'np.const', 'np.profile'
        ]);   
})();