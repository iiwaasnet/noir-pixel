﻿var npLogging = angular.module('npLogging', ['npUtils'])
    .constant('ApplicationLoggingConfig', {
        loggingApiUri: 'api.logging.noir-pixel.com/logging',
        errorUri: 'error',
        debugUri: 'debug'
    })
    .factory('Trace', [
        function() {
            return { print: printStackTrace };
        }
    ])
    .factory('ApplicationLogging', [
            '$log', '$window', '$httpBackend', 'Trace', 'ApplicationLoggingConfig', 'Url',
            function($log, $window, $httpBackend, Trace, ApplicationLoggingConfig, Url) {
                var config = ApplicationLoggingConfig;

                var logger = {
                    error: function(exception, cause) {
                        $log.error.apply($log, arguments);

                        try {
                            var errorMessage = (exception || '').toString();
                            var stackTrace = [];
                            if (typeof exception === 'object') {
                                stackTrace = Trace.print({ exception: exception });
                            }
                            $httpBackend('POST',
                                Url.combine([config.loggingApiUri, config.errorUri]),
                                angular.toJson({
                                    url: $window.location.href,
                                    message: errorMessage,
                                    type: 'exception',
                                    stackTrace: stackTrace,
                                    cause: (cause || '')
                                }),
                                responseLogging,
                                {
                                    'Content-Type': 'application/json',
                                }
                            );

                        } catch (e) {
                            $log.warn("Error server-side logging failed");
                            $log.log(e);
                        }
                    },
                    debug: function(message) {
                        $log.log.apply($log, arguments);

                        $httpBackend('POST',
                            Url.combine([config.loggingApiUri, config.debug]),
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
    .factory('ExceptionLogging', [
        'ApplicationLogging', function(ApplicationLogging) {
            return ApplicationLogging.error;
        }
    ])
    .provider('$exceptionHandler', {
            $get: function(ExceptionLogging) {
                return (ExceptionLogging);
            }
        }
    );