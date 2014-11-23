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
            Messages.error({
                main: {
                    text: 'Simple error message'
                }
            });
        }

        function message() {
            Messages.message({
                main: {
                    text: 'Simple text message'
                }
            });
        }
    }
})();