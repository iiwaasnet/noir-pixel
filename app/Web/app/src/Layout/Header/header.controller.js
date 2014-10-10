(function() {
    'use strict';

    angular.module('np.layout')
        .controller('HeaderController', headerController);

    headerController.$injector = ['MainMenu'];

    function headerController(MainMenu) {
        var ctrl = this;
        ctrl.mainMenu = MainMenu.getMenu();
    }
})();