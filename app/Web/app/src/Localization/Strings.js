npApp.service('Strings', [
    '$http', '$rootScope', '$window',
    function($http, $rootScope, $window) {
        var service = this,
            language = ($window.navigator.userLanguage || $window.navigator.language).split('-')[0],
            dictionary = [],
            stringsLoaded = false;

        service.setCurrentLanguage = function(value) {
            language = value;
            service.loadStrings();
        };

        service.getCurrentLanguage = function() {
            return language;
        };

        service.loadStrings = function() {
            var url = '/strings/localized/' + service.getCurrentLanguage();
            $http({ method: "GET", url: url, cache: false })
                .success(successCallback)
                .error(function() {
                    // TODO: Error logging
                });
        };

        service.getLocalizedString = function (value) {
            var strings = dictionary[service.getCurrentLanguage()];
            if (strings && strings !== []) {
                return strings[value];
            }
            
            return '';
        };

        function successCallback(data) {
            var strings = [];
            data.strings.forEach(function(el) {
                strings[el.key] = el.value;
            });

            dictionary[data.locale] = strings;
            stringsLoaded = true;

            $rootScope.$broadcast('stringsUpdates');
        }

    }
]);