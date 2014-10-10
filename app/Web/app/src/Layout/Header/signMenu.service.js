(function() {
    'use strict';

    angular.module('np.layout')
        .service('SingMenu', signMenuService);


    function signMenuService() {
        var service = this;
        service.getMenu = getMenu;

        function getMenu() {
            
        }
    }
})();