var npApp = angular.module('npApp', ['ngRoute']);

npApp.run(['Strings', function(Strings) {
    Strings.setCurrentLanguage(Strings.getCurrentLanguage());
}]);