(function() {
    'use strict';

    angular.module('np', ['ngRoute', 'LocalStorageModule',
        'np.logging', 'np.i18n', 'np.home'])
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