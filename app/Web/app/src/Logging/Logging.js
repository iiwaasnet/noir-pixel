var npLogging = angular.module('npLogging', ['npUtils'])
    .constant('ApplicationLoggingConfig', {
        loggingUri: 'api.logging.noir-pixel.com/log/add'
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
                    },
                    debug: function(message) {
                        $log.debug.apply($log, arguments);

                        sendLoggingRequest({
                            url: $window.location.href,
                            message: message,
                            type: 'debug'
                        });
                    },
                    warn: function (message) {
                        $log.warn.apply($log, arguments);

                        sendLoggingRequest({
                            url: $window.location.href,
                            message: message,
                            type: 'warn'
                        });
                    }
                };

                function sendLoggingRequest(data) {
                    $httpBackend('POST',
                        Url.build([config.loggingUri]),
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