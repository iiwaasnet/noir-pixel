(function() {
    'use strict';

    angular.module('np.storage')
        .service('Storage', storageService);

    storageService.$inject = ['webStorage'];

    function storageService(webStorage) {
        var srv = this;

        srv.set = set;
        srv.get = get;
        srv.remove = remove;

        function set(key, value) {
            webStorage.add(key, value, false);
        }

        function get(key) {
            return webStorage.get(key, false);
        }

        function remove(key) {
            webStorage.remove(key, true);
        }
    }
})();