﻿(function() {
    'use strict';

    angular.module('np.progress')
        .service('Progress', progressService);

    progressService.$inject = ['$interval', '$document', 'NProgress'];

    function progressService($interval, $document, NProgress) {
        var srv = this,
            stopCallTimeout = 15 * 1000,
            startDelay = 500,
            stopTimer = undefined,
            startTimer = undefined,
            body = angular.element($document[0].getElementById('progress-overlay')),
            progressStarted = false;
        srv.start = start;
        srv.stop = stop;

        function start() {
            if (!progressStarted) {
                progressStarted = true;
                startTimer = $interval(internalStart, startDelay, 1);
            }
        }

        function internalStart() {
            if (progressStarted) {
                NProgress.start();
                body.removeClass('hidden');
                stopTimer = $interval(stop, stopCallTimeout, 1);
            }
        }

        function stop() {
            progressStarted = false;
            cancelTimer(startTimer);
            body.addClass('hidden');
            NProgress.done();
            cancelTimer(stopTimer);
        }

        function cancelTimer(timer) {
            if (timer) {
                $interval.cancel(timer);
            }
        }
    }
})();