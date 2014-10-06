(function() {
    'use strict';

    angular.module('np.layout')
        .controller('HeaderController', headerController);

    headerController.$injector = ['MainMenu', '$scope'];

    function headerController(MainMenu, $scope) {
        var ctrl = this;
        ctrl.mainMenu = MainMenu.getMainMenu();
    };
})();