(function() {
    'use strict';

    angular.module('np.progress')
        .service('Progress', progressService);

    progressService.$inject = ['$interval', 'NProgress'];

    function progressService($interval, NProgress) {
        var srv = this,
            stopCallTimeout = 15 * 1000,
            timer = undefined;
        srv.start = start;
        srv.stop = stop;

        function start() {
            stop();

            timer = $interval(stop, stopCallTimeout);
            NProgress.start();
        }

        function stop() {
            NProgress.done();
            cancelTimer();

        }

        function cancelTimer() {
            if (timer) {
                $interval.cancel(timer);
                timer = undefined;
            }
        }
    }
})();