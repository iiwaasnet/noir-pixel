﻿(function () {
    'use strict';

    angular.module('np.progress')
        .service('Progress', progressService);

    progressService.$inject = ['NProgress'];

    function progressService(NProgress) {
        var srv = this;
        srv.start = start;
        srv.stop = stop;

        function start() {
            NProgress.start();
        }

        function stop() {
            NProgress.done();
        }
    }
})();