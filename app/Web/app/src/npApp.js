var npApp = angular.module('npApp', ['ngRoute', 'LocalStorageModule']);

npApp.run(['Strings', function(Strings) {
    Strings.setCurrentLanguage(Strings.getCurrentLanguage());
}]);