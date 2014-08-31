var npApp = angular.module('npApp', ['ngRoute', 'LocalStorageModule', 'npLogging'])
    .run([
        'Strings', 'ApplicationLogging', function (Strings, ApplicationLogging) {
            try {
                Strings.init();
            } catch (e) {
                ApplicationLogging.error(e);
            }

        }
    ]);