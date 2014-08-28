npApp.service('Strings', [
    '$http', '$rootScope', '$window', 'localStorageService',
    function($http, $rootScope, $window, localStorageService) {
        var service = this,
            dictionary = {},
            stringsLoaded = false,
            langStorageKey = 'currentLang',
            language = localStorageService.get(langStorageKey)
                || ($window.navigator.userLanguage || $window.navigator.language).split('-')[0];


        service.setCurrentLanguage = function(value) {
            language = value;
            localStorageService.set(langStorageKey, value);
            service.loadStrings();
        };

        service.getCurrentLanguage = function() {
            return language;
        };

        service.loadStrings = function() {
            var currentLang = service.getCurrentLanguage(),
                cache,
                url = '/strings/localized/' + currentLang;

            cache = dictionary[currentLang];
            if (!cache || Object.keys(cache.strings).length === 0) {
                cache = getStringsFromCache();
            }

            if (!!cache) {
                dictionary[currentLang] = cache;
            } else {
                $http({ method: "GET", url: url, cache: false })
                    .success(function(data) { successCallback(currentLang, data); })
                    .error(function() {
                        // TODO: Error logging
                    });
            }
        };

        service.getLocalizedString = function(value) {
            var cache = dictionary[service.getCurrentLanguage()];
            if (cache && Object.keys(cache.strings).length > 0) {
                return cache.strings[value];
            }

            return '';
        };

        function getStringsFromCache() {
            var cache = angular.fromJson(localStorageService.get(getStringsStorageKey()));
            if (!!cache && Object.keys(cache.strings).length > 0) {
                return cache;
            }

            return null;
        }

        function successCallback(lang, data) {
            var strings = {};
            data.strings.forEach(function(el) {
                strings[el.key] = el.value;
            });

            var cache = {
                locale: data.locale,
                language: lang,
                strings: strings
            };
            dictionary[lang] = cache;
            stringsLoaded = true;

            localStorageService.set(getStringsStorageKey(), angular.toJson(cache));

            $rootScope.$broadcast('stringsUpdates');

            if (lang !== data.locale) {
                //TODO: logging
            }
        }

        function getStringsStorageKey() {
            return 'strings-' + service.getCurrentLanguage();
        }

    }
]);