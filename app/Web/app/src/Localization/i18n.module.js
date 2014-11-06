(function() {
    'use strict';

    angular
        .module('np.i18n', ['np.logging', 'np.config', 'np.utils', 'LocalStorageModule'])
        .config(config);

    config.$inject = ['localStorageServiceProvider'];

    function config(localStorageServiceProvider) {
        localStorageServiceProvider.prefix = 'np';
    }
})();