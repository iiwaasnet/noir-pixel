(function() {
    'use strict';

    angular.module('np.utils')
        .service('Url', urlService);

    urlService.$inject = ['Config'];

    function urlService(Config) {
        var service = this;

        service.buildUrl = buildUrl;
        service.buildApiUrl = buildApiUrl;
        service.parseQueryString = parseQueryString;

        function parseQueryString(queryString) {
            var parsed = {};

            var queryParams = queryString.split("&");
            angular.forEach(queryParams, function(param) {
                var temp = param.split('=');
                if (temp.length === 2) {
                    parsed[temp[0]] = temp[1];
                }
            });

            return parsed;
        }

        function buildApiUrl() {
            var parts = [].slice.call(arguments);
            parts.splice(0, 0, Config.ApiUris.Base);
            
            return build(parts);
        }

        function buildUrl() {
            return build(arguments);
        }

        function build(parts) {
            if (parts.length === 0) {
                throw 'No arguments are provided!';
            }

            var url = location.protocol + '/';

            angular.forEach(parts, function (part) {
                if (part) {
                    if (part[0] !== '/') {
                        part = '/' + part;
                    }
                    url += part;
                }
            });

            return url;
        }
    }
})();