(function() {
    'use strict';

    angular.module('np.messages')
        .service('Messages', messagesService);

    messagesService.$inject = ['ngDialog', 'Strings'];

    function messagesService(ngDialog, Strings) {
        var srv = this,
            EAPI_Unknown = 'EAPI_Unknown';
        srv.error = error;
        srv.message = message;

        function error(err, palceholders) {
            ngDialog.open({
                template: 'app/src/sys-messages/message.html',
                cache: true,
                controller: 'MessageController as ctrl',
                className: 'dialog-theme-messages error',
                showClose: true,
                locals: {
                    message: convertToMessage(err, palceholders)
                }
            });
        }

        function message(msg) {
            ngDialog.open({
                template: 'app/src/sys-messages/message.html',
                cache: true,
                controller: 'MessageController as ctrl',
                className: 'dialog-theme-messages info',
                showClose: true,
                locals: {
                    message: convertToMessage(msg)
                }
            });
        }

        function convertToMessage(obj, palceholders) {
            var tmp = { main: obj };
            if (obj.main) {
                tmp.main = obj.main.message ? obj.main.message : Strings.getLocalizedString(obj.main.code);
                if (palceholders) {
                    tmp.main = tmp.main.formatNamed(palceholders);
                }
                if (obj.aux) {
                    tmp.aux = obj.aux.message ? obj.aux.message : Strings.getLocalizedString(obj.aux.code);
                }
            }
            if (!tmp.main) {
                tmp.main = Strings.getLocalizedString(EAPI_Unknown);
            }

            return tmp;
        }
    }
})();