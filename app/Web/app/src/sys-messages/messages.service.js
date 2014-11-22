(function () {
    'use strict';

    angular.module('np.messages')
        .service('Messages', messagesService);

    messagesService.$inject = ['ngDialog', 'Strings'];

    function messagesService(ngDialog, Strings) {
        var srv = this;
        srv.error = error;
        srv.message = message;

        function error(errorMsg) {
            ngDialog.open({
                template: 'app/src/sys-messages/error.html',
                cache: true,
                controller: 'ErrorController as ctrl',
                className: 'dialog-theme-messages error',
                showClose: true,
                locals: {
                    error: { header: 'Simple error message' }
                }
            });
        }

        function message(){}
    }
})();