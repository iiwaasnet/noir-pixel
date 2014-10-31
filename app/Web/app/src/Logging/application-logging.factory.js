(function() {
    'use strict';

    angular.module('np.logging')
        .factory('ApplicationLogging', applicationLoggingFactory);

    applicationLoggingFactory.$injector = ['$log', '$window', '$httpBackend', 'Trace', 'Config', 'Url'];

    function applicationLoggingFactory($log, $window, $httpBackend, Trace, Config, Url) {
        var logger = {
            error: error,
            debug: debug,
            warn: warn
        };

        return logger;


        function error(exception, cause) {
            $log.error.apply($log, arguments);

            try {
                var errorMessage = (exception || '').toString();
                var stackTrace = [];
                if (typeof exception === 'object') {
                    stackTrace = Trace.print({ exception: exception });
                }

                sendLoggingRequest({
                    url: $window.location.href,
                    message: errorMessage,
                    type: 'exception',
                    stackTrace: stackTrace,
                    cause: (cause || '')
                });
            } catch (e) {
                $log.warn("Error server-side logging failed");
                $log.log(e);
            }
        }

        function debug(message) {
            $log.debug.apply($log, arguments);

            sendLoggingRequest({
                url: $window.location.href,
                message: message,
                type: 'debug'
            });
        }

        function warn(message) {
            $log.warn.apply($log, arguments);

            sendLoggingRequest({
                url: $window.location.href,
                message: message,
                type: 'warn'
            });
        }

        function sendLoggingRequest(data) {
            $httpBackend('POST',
                Url.build(Config.loggingApiUri),
                angular.toJson(data),
                responseLogging,
                {
                    'Content-Type': 'application/json',
                }
            );
        }

        function responseLogging(status, resp, headerString) {
            if (status !== 200) {
                $log.log(status, resp, headerString);
            }
        }
    }
})();