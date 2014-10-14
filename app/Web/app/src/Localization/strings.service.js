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
            checkStringVersions()
                .success(invalidateLocalCache)
                .error(function(msg) { invalidateLocalCache([]); })
                .success()
                .error();
            //scheduleCacheInvalidation();

            return deferred.promise;
        }

        function invalidateLocalCache(versions) {
            var deferred = $q.defer();

            versions.forEach(checkLocaleStrings);

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

            return deferred.promise;
        }

        function checkLocaleStrings(lang) {
            var deferred = $q.defer();
            var promises = new 

            var cache = dictionary[lang.locale];
            if (!cache || cache.version !== lang.version) {
                return loadStringsForLocale(lang.locale);
            }

            return $q.when(true);
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
            $interval(function() { checkStringVersions(deferred); }, interval, 0, false);

            return deferred.promise;
        }

        function checkStringVersions() {
            var deferred = $q.defer();

            $http({ method: "GET", url: Config.strings.versionsUri, cache: false })
                .success(function(data) { deferred.resolve(data.versions); })
                .error(function(msg) { deferred.reject(msg); });

            return deferred.promise;
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

        function loadStringsForLocale(locale) {
            return $http({ method: "GET", url: Config.strings.localizedUri + locale, cache: false })
                .success(function(data) { getStringsSuccess(locale, data); })
                .error(function() { ApplicationLogging.error('Failed loading Strings for language ' + locale + '!'); });
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