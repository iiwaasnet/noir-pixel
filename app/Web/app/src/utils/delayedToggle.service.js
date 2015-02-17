(function() {
    'use strict';

    angular.module('np.utils')
        .service('DelayedToggle', delayedToggle);

    delayedToggle.$inject = ['$interval'];

    function delayedToggle($interval) {
        var srv = this;
        srv.on = on;

        function on(obj, prop) {
            return createToggle(obj, prop);
        }

        function createToggle(obj, prop) {
            var toggle = {
                stopCallTimeout: 15 * 1000,
                startDelay: 500,
                stopTimer: undefined,
                startTimer: undefined,
                progressStarted: false
            };
            toggle.on = on;
            toggle.off = off;

            return toggle;

            function on() {
                if (!toggle.progressStarted) {
                    toggle.progressStarted = true;
                    toggle.startTimer = $interval(internalStart, toggle.startDelay, 1);
                }

                function internalStart() {
                    if (toggle.progressStarted) {
                        obj[prop] = true;
                        toggle.stopTimer = $interval(stop, toggle.stopCallTimeout, 1);
                    }
                }

                function stop() {
                    toggle.progressStarted = false;
                    cancelTimer(toggle.startTimer);
                    obj[prop] = false;
                    cancelTimer(toggle.stopTimer);
                }

                function cancelTimer(timer) {
                    if (timer) {
                        $interval.cancel(timer);
                    }
                }
            }
        }
    }
})();