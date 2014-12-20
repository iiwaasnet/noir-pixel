(function(){
    'use strict';

    angular.module('np.messages')
        .controller('MessageController', messageController);

    messageController.$inject = ['message'];

    function messageController(message) {
        var ctrl = this;
        ctrl.message = message;
    }
})();