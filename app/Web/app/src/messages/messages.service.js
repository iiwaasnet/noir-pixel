(function() {
    'use strict';

    angular.module('np.messages')
        .service('Messages', messagesService);

    messagesService.$inject = ['$q', 'ngDialog', 'Strings'];

    function messagesService($q, ngDialog, Strings) {
        var srv = this,
            currentDialog;
        srv.error = error;
        srv.message = message;

        function error(err, placeholders, fallbackErrCode) {
            closeCurrent().then(function (data) {
                openDialog(err, placeholders, fallbackErrCode);
            });
        }

        function message(msg, placeholders, fallbackMsgCode) {
            currentDialog = ngDialog.open({
                template: 'app/src/messages/message.html',
                cache: true,
                controller: 'MessageController as ctrl',
                className: 'dialog-theme-messages info',
                showClose: true,
                locals: {
                    message: Strings.getLocalizedMessageObject(msg, placeholders, fallbackMsgCode)
                }
            });
        }

        function openDialog(err, placeholders, fallbackErrCode) {
            currentDialog = ngDialog.open({
                template: 'app/src/messages/message.html',
                cache: true,
                controller: 'MessageController as ctrl',
                className: 'dialog-theme-messages error',
                showClose: true,
                locals: {
                    message: Strings.getLocalizedErrorObject(err, placeholders, fallbackErrCode)
                }
            });
        }

        function closeCurrent() {
            if (currentDialog) {                
                currentDialog.close();
                return currentDialog.closePromise;
            }
            var deferred = $q.defer();
            deferred.resolve();
            return deferred.promise;
        }
    }
})();