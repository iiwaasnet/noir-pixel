(function() {
    'use strict';

    var vendor = [
        'ng',
        'ngAnimate',
        'ngSanitize',
        'ui.router',
        'ngMessages',
        'ui.select',
        'flow',
        'angular-progress-arc',
        'ngScrollbar'
    ];

    var app = [
        'np.progress',
        'np.logging',
        'np.i18n',
        'np.auth',
        'np.home',
        'np.layout',
        'np.messages',
        'np.ui-elements',
        'np.utils',
        'np.validation',
        'np.events',
        'np.user',
        'np.roles',
        'np.view-resolver',
        'np.user-home',
        'np.const',
        'np.profile',
        'np.geo',
        'np.photos',
        'np.overlay'
    ];

    angular.module('np', vendor.concat(app));
})();