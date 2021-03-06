﻿(function() {
    'use strict';

    angular.module('np.progress', [])
        .constant('NProgress', NProgress)
        .config(config);

    config.$inject = ['NProgress'];

    function config(NProgress) {
        NProgress.configure({
            showSpinner: false,
            trickleRate: 0.2,
            trickleSpeed: 1000
        });
    }
})();