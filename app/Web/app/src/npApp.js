var npApp = angular.module('npApp', ['ngRoute', 'LocalStorageModule', 'npLogging'])
    .run([
        'Strings', 'ApplicationLogging', function(Strings, ApplicationLogging) {
            try {
                Strings.init();
                noFunc();
            } catch (e) {
                ApplicationLogging.error(e);
            }

        }
    ]);