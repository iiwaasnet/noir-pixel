(function() {
    'use strict';

    angular.module('np.layout')
        .service('MainMenu', mainMenuService);

    mainMenuService.$injector = ['Strings'];

    function mainMenuService(Strings) {
        var service = this;
        service.getMenu = getMenu;

        function getMenu() {
            var gallery = {
                text: Strings.getLocalizedString('MainMenu_Gallery'),
                uri: 'gallery'
            };

            var menu = [
                gallery
            ];

            return menu;
        }
    }
})();