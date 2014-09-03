npApp.constant('HttpProviderConfig', {
        interceptStatuses: [500]
    })
    .config([
        '$httpProvider', function($httpProvider) {
            $httpProvider.interceptors.push([
                '$rootScope', '$q', '$injector', '$location', 'ApplicationLogging', 'HttpProviderConfig',
                function($rootScope, $q, $injector, $location, ApplicationLogging, HttpProviderConfig) {
                    function responseError(response) {
                        if (response && HttpProviderConfig.interceptStatuses.indexOf(response.status) > -1) {
                            ApplicationLogging.error(createError(response));
                        }

                        return $q.reject(response.config);
                    }

                    function createError(response) {
                        var error = {
                            method: response.config.method,
                            url: response.config.url,
                            message: (response.data && response.data.toString()) || '',
                            status: response.status
                        };

                        error.toString = function() {
                            return 'method: ' + error.method.toUpperCase()
                                + ' url: ' + error.url
                                + ' status: ' + error.status
                                + ' data: ' + error.message.substring(0, 200);
                        }

                        return error;
                    }

                    return { responseError: responseError };
                }
            ]);
        }
    ]);