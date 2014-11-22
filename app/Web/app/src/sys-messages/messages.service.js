(function () {
    'use strict';

    angular.module('np.messages')
        .service('Messages', messagesService);

    messagesService.$inject = ['ngDialog', 'Strings'];

    function messagesService(ngDialog, Strings) {
        var srv = this;
        srv.error = error;
        srv.message = message;

        function error(err) {
            ngDialog.open({
                template: 'app/src/sys-messages/error.html',
                cache: true,
                controller: 'ErrorController as ctrl',
                className: 'dialog-theme-messages error',
                showClose: true,
                locals: {
                    error: err
                }
            });
        }

        function message(msg) {
            ngDialog.open({
                template: 'app/src/sys-messages/error.html',
                cache: true,
                controller: 'ErrorController as ctrl',
                className: 'dialog-theme-messages info',
                showClose: true,
                locals: {
                    error: {header: 'Simple message'}
                }
            });
        }
    }
})();