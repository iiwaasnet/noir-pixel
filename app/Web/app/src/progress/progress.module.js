(function() {
    'use strict';

    angular.module('np.progress', [])
        .constant('NProgress', NProgress)
        .config(config);

    config.$inject = ['NProgress'];

    function config(NProgress) {
        NProgress.configure({
            showSpinner: true,
            trickleRate: 0.2,
            trickleSpeed: 800
        });
    }
})();