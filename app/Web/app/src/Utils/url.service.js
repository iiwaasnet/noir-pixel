(function() {
    'use strict';

    angular.module('np.utils')
        .service('Url', urlService);

    function urlService() {
        var service = this;

        service.build = build;

        function build() {
            var parts = arguments;

            if (parts.length === 0) {
                throw 'No arguments are provided!';
            }

            var url = location.protocol + '/';

            angular.forEach(parts, function (part) {
                if (part.length !== 0) {
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