(function() {
    'use strict';

    angular.module('np.overlay')
        .service('Overlay', overlay);

    overlay.$inject = ['$q', 'ngDialog'];

    function overlay($q, ngDialog) {
        var srv = this,
            currentOverlay;
        srv.open = open;
        srv.close = close;

        function open(templateUrl, controller, locals, options) {
            options = angular.extend({
                template: templateUrl,
                cache: true,
                controller: controller,
                className: 'dialog-theme-messages overlay',
                showClose: true,
                locals: locals,
                closeByDocument: false
            }, options);

            currentOverlay = ngDialog.open(options);
        }

        function close() {
            closeCurrent();
        }

        function closeCurrent() {
            if (currentOverlay) {
                currentOverlay.close();
                return currentOverlay.closePromise;
            }
            var deferred = $q.defer();
            deferred.resolve();
            return deferred.promise;
        }
    }
})();