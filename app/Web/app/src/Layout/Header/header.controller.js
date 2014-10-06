(function() {
    'use strict';

    angular.module('np.layout')
        .controller('HeaderController', headerController);

    headerController.$injector = ['MainMenu', '$scope'];

    function headerController(MainMenu, $scope) {
        var ctrl = this;
        $scope.mainMenu = MainMenu.getMainMenu();
        $scope.add = add;
        $scope.title = 'bla';

        function add() {
            $scope.title = 'bla-bla';
            $scope.mainMenu[0].text = 'bla';
            //$scope.mainMenu = [
            //{
            //    text: 'Bla1',
            //    uri: 'gallery'
            //},
            //    {
            //        text: 'Bla2',
            //        uri: 'gallery'
            //    }
            //];
        }
    };
})();