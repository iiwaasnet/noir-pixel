(function() {
    'use strict';

    angular.module('np.home')
        .controller('HomeController', homeController);

    homeController.$inject = ['Messages'];

    function homeController(Messages) {
        var ctrl = this;
        ctrl.error = error;
        ctrl.message = message;

        function error() {
            Messages.error({header: 'Simple error message'});
        }

        function message() {
            Messages.message({header: 'Simple text message'});
        }
    }
})();