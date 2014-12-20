(function() {
    'use strict';

    angular.module('np.messages')
        .service('Messages', messagesService);

    messagesService.$inject = ['$q', 'ngDialog', 'Strings'];

    function messagesService($q, ngDialog, Strings) {
        var srv = this,
            EAPI_Unknown = 'EAPI_Unknown',
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
                    message: convertToMessage(msg, placeholders, fallbackMsgCode)
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
                    message: convertToMessage(err, placeholders, fallbackErrCode)
                }
            });
        }

        function convertToMessage(obj, placeholders, fallbackMsgCode) {
            var tmp = { main: obj };
            if (obj.main) {
                tmp.main = obj.main.message ? obj.main.message : Strings.getLocalizedString(obj.main.code);
                if (placeholders) {
                    tmp.main = tmp.main.formatNamed(placeholders);
                }
                if (obj.aux) {
                    tmp.aux = obj.aux.message ? obj.aux.message : Strings.getLocalizedString(obj.aux.code);
                }
            }
            if (!tmp.main) {
                tmp.main = Strings.getLocalizedString(fallbackMsgCode) || Strings.getLocalizedString(EAPI_Unknown);
            }

            return tmp;
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