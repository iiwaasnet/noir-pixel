﻿npApp.service('Strings', [
    '$http', '$rootScope', '$window', '$interval', 'localStorageService',
    function($http, $rootScope, $window, $interval, localStorageService) {
        var service = this,
            dictionary = {},
            stringsLoaded = false,
            langStorageKey = 'currentLang',
            language = localStorageService.get(langStorageKey)
                || ($window.navigator.userLanguage || $window.navigator.language).split('-')[0];


        service.init = function() {
            service.setCurrentLanguage(service.getCurrentLanguage());
            scheduleCacheInvalidation();
        };

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
                cache;

            cache = dictionary[currentLang];
            if (!cache || Object.keys(cache.strings).length === 0) {
                cache = getStringsFromCache();
            }

            if (!!cache) {
                dictionary[currentLang] = cache;
            } else {
                loadStringsForLocale(currentLang);
            }
        };

        service.getLocalizedString = function(value) {
            var cache = dictionary[service.getCurrentLanguage()];
            if (cache && Object.keys(cache.strings).length > 0) {
                return cache.strings[value];
            }

            return '';
        };

        function scheduleCacheInvalidation() {
            $http({ method: "GET", url: '/strings/config', cache: false })
                .success(getConfigSuccess)
                .error(function() {
                    // TODO: Error logging
                });
        }

        function getConfigSuccess(data) {
            $interval(checkStringVersions, data.invalidationTimeout, 0, false);
        }

        function checkStringVersions() {
            $http({ method: "GET", url: '/strings/versions', cache: false })
                .success(getVersionsSuccess)
                .error(function() {
                    // TODO: Error logging
                });
        }


        function getVersionsSuccess(data) {
            Object.keys(dictionary).forEach(function (lang) {
                var cache = dictionary[lang];
                var versionInfo = data.versions.filter(function(el) {
                    return el.locale === cache.locale;
                })[0];
                if (versionInfo && versionInfo.version !== cache.version) {
                    loadStringsForLocale(cache.language);
                }
            });
        }

        function loadStringsForLocale(locale) {

            $http({ method: "GET", url: '/strings/localized/' + locale, cache: false })
                .success(function(data) { getStringsSuccess(locale, data); })
                .error(function() {
                    // TODO: Error logging
                });
        }

        function getStringsFromCache() {
            var cache = angular.fromJson(localStorageService.get(getStringsStorageKey()));
            if (!!cache && Object.keys(cache.strings).length > 0) {
                return cache;
            }

            return null;
        }

        function getStringsSuccess(lang, data) {
            var strings = {};
            data.strings.forEach(function(el) {
                strings[el.key] = el.value;
            });

            var cache = {
                locale: data.locale,
                language: lang,
                version: data.version,
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