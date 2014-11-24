(function() {
    'use strict';

    angular.module('np.home')
        .controller('HomeController', homeController);

    homeController.$inject = ['Messages', 'Progress'];

    function homeController(Messages, Progress) {
        var ctrl = this;
        ctrl.error = error;
        ctrl.message = message;

        function error() {
            Progress.start();
            //Messages.error({
            //    main: {
            //        text: 'Simple error message'
            //    }
            //});
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