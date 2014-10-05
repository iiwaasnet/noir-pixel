(function() {
    'use strict';

    angular.module('np', ['ui.router', 'LocalStorageModule',
        'np.logging', 'np.i18n', 'np.home', 'np.layout'])
        .run([
            'Strings', 'ApplicationLogging', function(Strings, ApplicationLogging) {
                try {
                    Strings.init();
                } catch (e) {
                    ApplicationLogging.error(e);
                }

            }
        ]);
})();