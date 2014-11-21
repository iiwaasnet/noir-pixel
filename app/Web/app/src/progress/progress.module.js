(function() {
    'use strict';

    angular.module('np.progress', [])
        .constant('NProgress', NProgress)
        .config(config);

    config.$inject = ['NProgress'];

    function config(NProgress) {
        NProgress.configure({
            showSpinner: false,
            parent: '#loading-progress',
            trickleRate: 0.2,
            trickleSpeed: 800
        });
    }

    //angular.module('np.progress', ['angular-loading-bar'])
    //.config(config);

    //config.$inject = ['cfpLoadingBarProvider'];

    //function config(cfpLoadingBarProvider) {
    //    cfpLoadingBarProvider.latencyThreshold = 500;
    //    cfpLoadingBarProvider.includeSpinner = false;
    //}
})();