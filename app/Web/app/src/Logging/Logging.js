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
                        angular.toJson({
                            url: $window.location.href,
                            message: errorMessage,
                            type: 'exception',
                            stackTrace: stackTrace,
                            cause: (cause || '')
                        }),
                        function(status, resp, headerString) {
                            if (status !== 200) {
                                $log.log(status, resp, headerString);
                            }
                        },
                        {
                            'Content-Type': 'application/json',
                        }
                    );

                } catch (e) {
                    $log.warn("Error server-side logging failed");
                    $log.log(e);
                }
            }

            return error;
        }
    ])
    .factory('ApplicationLogging', [
            '$log', '$window', '$httpBackend', 'Trace' ,function($log, $window, $httpBackend, Trace) {
                var logger = {
                    error: function(error) {
                        $log.error.apply($log, arguments);

                        var message = error,
                            stackTrace = '';
                        if (error && typeof error === 'object') {
                            message = error.toString();
                            stackTrace = Trace.print({ exception: error });
                        }

                        $httpBackend('POST',
                            'http://api.logging.noir-pixel.com/logging/error',
                            angular.toJson({
                                url: $window.location.href,
                                message: message,
                                stackTrace: stackTrace,
                                type: 'error'
                            }),
                            responseLogging,
                            {
                                'Content-Type': 'application/json',
                            }
                        );
                    },
                    debug: function(message) {
                        $log.log.apply($log, arguments);

                        $httpBackend('POST',
                            'http://api.logging.noir-pixel.com/logging/debug',
                            angular.toJson({
                                url: $window.location.href,
                                message: message,
                                type: 'debug'
                            }),
                            responseLogging,
                            {
                                'Content-Type': 'application/json',
                            }
                        );
                    }

                    
                };

                function responseLogging(status, resp, headerString) {
                    if (status !== 200) {
                        $log.log(status, resp, headerString);
                    }
                }

                return logger;
            }
        ]
    )
    .provider('$exceptionHandler', {
            $get: function(ExceptionLogging) {
                return (ExceptionLogging);
            }
        }
    );