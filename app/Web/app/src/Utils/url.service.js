(function() {
    'use strict';

    angular.module('np.utils')
        .service('Url', urlService);

    function urlService() {
        var service = this;

        service.build = build;
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

        function build() {
            var parts = arguments;

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