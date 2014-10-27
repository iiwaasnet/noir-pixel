(function() {
    'use strict';

    angular.module('np.utils')
        .service('Url', urlService);

    function urlService() {
        var service = this;

        service.build = build;

        function build(parts) {
            if (!parts || !(parts instanceof Array)) {
                throw '[parts] is either null or not an Array!';
            }
            if (parts.length === 0) {
                throw '[parts] is empty!';
            }

            var url = location.protocol + '/';

            parts.forEach(function(part) {
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