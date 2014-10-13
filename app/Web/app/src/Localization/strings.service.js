(function() {
    'use strict';

    angular.module('np.i18n')
        .service('Strings', stringsService);

    stringsService.$injector = [
        '$q', '$http', '$rootScope', '$window', '$interval', 'localStorageService', 'Config', 'ApplicationLogging', 'Moment'
    ];

    function stringsService($q, $http, $rootScope, $window, $interval, localStorageService, Config, ApplicationLogging, Moment) {
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
            var deferred = $q.defer();

            service.setCurrentLanguage(service.getCurrentLanguage());
            checkStringVersions(deferred);
            //scheduleCacheInvalidation();

            return deferred.promise;
        }

        function setCurrentLanguage(value) {
            language = value;
            localStorageService.set(langStorageKey, value);
            service.loadStrings();
        }

        function getCurrentLanguage() {
            return language;
        }

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
        }

        function getLocalizedString(value) {
            var cache = dictionary[service.getCurrentLanguage()];
            if (cache && Object.keys(cache.strings).length > 0) {
                return cache.strings[value];
            }

            return '';
        }

        function scheduleCacheInvalidation() {
            var deferred = $q.defer();

            var interval = Moment.duration(Config.strings.invalidationTimeout).asMilliseconds();
            $interval(function () { checkStringVersions(deferred); }, interval, 0, false);

            return deferred.promise;
        }

        function checkStringVersions(deferred) {
            $http({ method: "GET", url: Config.strings.versionsUri, cache: false })
                .success(function(data) { getVersionsSuccess(data, deferred); });
        }


        function getVersionsSuccess(data, deferred) {
            Object.keys(dictionary).forEach(function(lang) {
                var cache = dictionary[lang];
                var versionInfo = data.versions.filter(function(el) {
                    return el.locale === cache.locale;
                })[0];
                //if (versionInfo && versionInfo.version !== cache.version) {
                //TODO: Decide on the strings versioning
                    loadStringsForLocale(cache.language, deferred);
                //}
            });
        }

        function loadStringsForLocale(locale, deferred) {

            $http({ method: "GET", url: Config.strings.localizedUri + locale, cache: false })
                .success(function (data) { getStringsSuccess(locale, data, deferred); });
        }

        function getStringsFromCache() {
            var cache = angular.fromJson(localStorageService.get(getStringsStorageKey()));
            if (!!cache && Object.keys(cache.strings).length > 0) {
                return cache;
            }

            return null;
        }

        function getStringsSuccess(lang, data, deferred) {
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

            deferred.resolve(true);

            $rootScope.$broadcast('stringsUpdated');

            if (lang !== data.locale) {
                ApplicationLogging.warn('No Strings defined for language ' + lang + '! Locale ' + data.locale + ' used as fall-back.');
            }
        }

        function getStringsStorageKey() {
            return 'strings-' + service.getCurrentLanguage();
        }
    }
})();