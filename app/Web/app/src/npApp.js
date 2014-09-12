angular.module('npApp', ['ngRoute', 'LocalStorageModule', 'npLogging', 'npUtils'])
    .run([
        'Strings', 'ApplicationLogging', function(Strings, ApplicationLogging) {
            try {
                Strings.init();
            } catch (e) {
                ApplicationLogging.error(e);
            }

        }
    ]);