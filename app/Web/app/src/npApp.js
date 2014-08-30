var npApp = angular.module('npApp', ['ngRoute', 'LocalStorageModule', 'npLogging'])
    .run([
        'Strings', function(Strings) {
            Strings.init();
        }
    ]);