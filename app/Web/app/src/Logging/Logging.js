var npLogging = angular.module('npLogging', [])
    .factory('Trace', [
        function() {
            return { print: printStackTrace };
        }
    ])
    .factory('ExceptionLogging', [
        '$log', '$window', '$httpBackend', 'Trace', function($log, $window, $httpBackend, Trace) {
            function error(exception, cause) {
                $log.error.apply($log, arguments);

                try {
                    var errorMessage = exception.toString();

                    var stackTrace = Trace.print({ exception: exception });

                    $httpBackend('POST',
                        'http://api.logging.noir-pixel.com/logging/error',
                        { 'Content-Type': 'application/json' },
                        angular.toJson({
                            url: $window.location.href,
                            message: errorMessage,
                            type: "exception",
                            stackTrace: stackTrace,
                            cause: (cause || "")
                        }),
                        function(status, resp, headerString) {
                            $log.log('Logging failed!', status, resp, headerString);
                        }
                    );

                } catch (loggingError) {
                    $log.warn("Error server-side logging failed");
                    $log.log(loggingError);
                }
            }

            return error;
        }
    ])
    .factory("ApplicationLogging", [
            "$log", "$window", function($log, $window) {
                return ({
                    error: function(message) {
                        $log.error.apply($log, arguments);

                        $httpBackend('POST',
                            'http://api.logging.noir-pixel.com/logging/error',
                            { 'Content-Type': 'application/json' },
                            angular.toJson({
                                url: $window.location.href,
                                message: message,
                                type: "error"
                            }),
                            function(status, resp, headerString) {
                                $log.log('Logging failed!', status, resp, headerString);
                            }
                        );
                    },
                    debug: function(message) {
                        $log.log.apply($log, arguments);

                        $httpBackend('POST',
                            'http://api.logging.noir-pixel.com/logging/debug',
                            { 'Content-Type': 'application/json' },
                            angular.toJson({
                                url: $window.location.href,
                                message: message,
                                type: "debug"
                            }),
                            function(status, resp, headerString) {
                                $log.log('Logging failed!', status, resp, headerString);
                            }
                        );
                    }
                });
            }
        ]
    )
    .provider("$exceptionHandler", {
            $get: function(ExceptionLogging) {
                return (ExceptionLogging);
            }
        }
    );