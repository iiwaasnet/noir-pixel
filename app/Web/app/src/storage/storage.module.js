(function() {
    'use strict';

    angular.module('np.storage', ['webStorageModule'])
        .config(config);

    config.$inject = ['defaultSettings'];

    function config(defaultSettings) {
        defaultSettings.prefix = 'np.';
    }
})();