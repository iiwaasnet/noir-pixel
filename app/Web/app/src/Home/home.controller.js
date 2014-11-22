(function() {
    'use strict';

    angular.module('np.home')
        .controller('HomeController', homeController);

    homeController.$inject = ['Messages'];

    function homeController(Messages) {
        var ctrl = this;
        ctrl.error = error;

        function error() {
            Messages.message({header: 'Simple error message'});
        }
    }
})();