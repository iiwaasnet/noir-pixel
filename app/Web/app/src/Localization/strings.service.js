(function() {
    'use strict';

    angular.module('np.i18n')
        .service('Strings', stringsService);

    stringsService.$injector = [
        '$http', '$rootScope', '$window', '$interval', 'localStorageService', 'Config', 'ApplicationLogging', 'Moment'
    ];

    function stringsService($http, $rootScope, $window, $interval, localStorageService, Config, ApplicationLogging, Moment) {
        var service = this,
            dictionary = {},
            stringsLoaded = false,
            langStorageKey = 'currentLang',
            language = localStorageService.get(langStorageKey)
                || ($window.navigator.userLanguage || $window.navigator.language).split('-')[0];


        service.init = init;
        service.setCurrentLanguage = setCurrentLanguage;
        service.getCurrentLanguage = getCurrentLanguage;
        service.loadStrings = loadStrings;
        service.getLocalizedString = getLocalizedString;

        function init() {
            service.setCurrentLanguage(service.getCurrentLanguage());
            scheduleCacheInvalidation();
        };

        function setCurrentLanguage(value) {
            language = value;
            localStorageService.set(langStorageKey, value);
            service.loadStrings();
        };

        function getCurrentLanguage() {
            return language;
        };

        function loadStrings() {
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

        function getLocalizedString(value) {
            var cache = dictionary[service.getCurrentLanguage()];
            if (cache && Object.keys(cache.strings).length > 0) {
                return cache.strings[value];
            }

            return '';
        };

        function scheduleCacheInvalidation() {
            var interval = Moment.duration(Config.strings.invalidationTimeout).asMilliseconds();
            $interval(checkStringVersions, interval, 0, false);
        }

        function checkStringVersions() {
            $http({ method: "GET", url: Config.strings.versionsUri, cache: false })
                .success(getVersionsSuccess);
        }


        function getVersionsSuccess(data) {
            Object.keys(dictionary).forEach(function(lang) {
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

            $http({ method: "GET", url: Config.strings.localizedUri + locale, cache: false })
                .success(function(data) { getStringsSuccess(locale, data); });
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
                ApplicationLogging.warn('No Strings defined for language ' + lang + '! Locale ' + data.locale + ' used as fall-back.');
            }
        }

        function getStringsStorageKey() {
            return 'strings-' + service.getCurrentLanguage();
        }
    }
})();