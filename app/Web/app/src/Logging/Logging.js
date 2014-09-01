var npLogging = angular.module('npLogging', [])
    .factory('Trace', [
        function() {
            return { print: printStackTrace };
        }
    ])
    .factory('ExceptionLogging', [
        '$log', '$window', '$httpBackend', 'Trace', function ($log, $window, $httpBackend, Trace) {
            function error(exception, cause) {
                $log.error.apply($log, arguments);

                try {
                    var errorMessage = exception.toString();
                    var stackTrace = Trace.print({ exception: exception });

                    //var $http = $injector.get('$http');

                    //$http({
                    //        method: 'OPTIONS',
                    //        url: 'http://api.logging.noir-pixel.com/logging/error',
                    //        headers: {
                    //            Origin: 'http://noir-pixel.com',
                    //            'Access-Control-Request-Method': 'POST',
                    //            'Access-Control-Request-Headers': 'X-PINGOTHER'
                    //        }
                    //    })
                    //    .success(function(data, status) {
                    //        //TODO
                    //    })
                    //    .error(function(data, status) {
                    //        // TODO: Error logging
                    //    });

                    //$http({
                    //        method: 'POST',
                    //        url: 'http://api.logging.noir-pixel.com/logging/error',
                    //        data: angular.toJson({
                    //            url: $window.location.href,
                    //            message: errorMessage,
                    //            type: "exception",
                    //            stackTrace: stackTrace,
                    //            cause: (cause || "")
                    //        })
                    //    })
                    //    .success(function(data, status) {
                    //        //TODO
                    //    })
                    //    .error(function(data, status) {
                    //        // TODO: Error logging
                    //    });

                    //$httpBackend('OPTIONS',
                    //    'http://api.logging.noir-pixel.com/logging/error',
                    //    {
                    //        'Access-Control-Request-Method': 'POST',
                    //        Origin: 'http://noir-pixel.com'
                    //    },
                    //    function(status, resp, headerString) {
                    //        $log.log('Logging failed!', status, resp, headerString);
                    //    }
                    //);

                    var data = angular.toJson({
                        url: $window.location.href,
                        message: errorMessage,
                        type: 'exception',
                        stackTrace: stackTrace,
                        cause: (cause || '')
                    });

                    data = angular.toJson({bla: 'bla'});

                    $httpBackend('POST',
                        'http://api.logging.noir-pixel.com/logging/error',
                        data,
                        function(status, resp, headerString) {
                            $log.log('Logging failed!', status, resp, headerString);
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
            '$log', '$window', '$httpBackend', function($log, $window, $httpBackend) {
                return ({
                    error: function(message) {
                        $log.error.apply($log, arguments);

                        $httpBackend('POST',
                            'http://api.logging.noir-pixel.com/logging/error',
                            {
                                'Content-Type': 'application/json',
                            },
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
                            {
                                'Content-Type': 'application/json',
                            },
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
    .provider('$exceptionHandler', {
            $get: function(ExceptionLogging) {
                return (ExceptionLogging);
            }
        }
    );