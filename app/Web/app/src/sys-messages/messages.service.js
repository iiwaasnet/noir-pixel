(function() {
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
                template: 'app/src/sys-messages/message.html',
                cache: true,
                controller: 'MessageController as ctrl',
                className: 'dialog-theme-messages error',
                showClose: true,
                locals: {
                    message: convertToError(err)
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

        function convertToError(err) {
            var tmp = {
                main: err.main.text ? err.main.text : Strings.getLocalizedString(err.main.id)
            };
            if (err.aux) {
                tmp.aux = err.aux.text ? err.aux.text : Strings.getLocalizedString(err.aux.id);
            }
            return tmp;
        }

        function convertToMessage(msg) {
            var tmp = {
                main: msg.main.text ? msg.main.text : Strings.getLocalizedString(msg.main.id)
            };
            if (msg.aux) {
                tmp.aux = msg.aux.text ? msg.aux.text : Strings.getLocalizedString(msg.aux.id);
            }
            return tmp;
        }
    }
})();