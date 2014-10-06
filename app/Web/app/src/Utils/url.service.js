﻿(function() {
    'use strict';

    angular.module('np.utils')
        .service('Url', urlService);

    function urlService() {
        var protocolMatch = new RegExp('^http(s|)', 'i'),
            service = this;

        service.build = build;

        function build(parts) {
            if (!parts || !(parts instanceof Array)) {
                throw '[parts] is either null or not an Array!';
            }
            if (parts.length === 0) {
                throw '[parts] is empty!';
            }

            var protocol = protocolMatch.exec(window.location) || [];
            if (protocol.length === 0) {
                throw 'Unknown protocol for url ' + window.location;
            }

            var url = protocol[0] + ':/';

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