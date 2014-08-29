var npApp = angular.module('npApp', ['ngRoute', 'LocalStorageModule'])
    .run([
        'Strings', function(Strings) {
            Strings.init();
        }
    ]);