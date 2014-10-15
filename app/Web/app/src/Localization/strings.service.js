﻿(function() {
    'use strict';

    angular.module('np.i18n')
        .service('Strings', stringsService);

    stringsService.$injector = [
        '$q', '$http', '$rootScope', '$window', '$interval', 'localStorageService', 'Config', 'ApplicationLogging', 'Moment'
    ];

    function stringsService($q, $http, $rootScope, $window, $interval, localStorageService, Config, ApplicationLogging, Moment) {
        var service = this,
            dictionary = {},
            langStorageKey = 'currentLang',
            language = localStorageService.get(langStorageKey)
                || ($window.navigator.userLanguage || $window.navigator.language).split('-')[0];


        service.init = init;
        service.setCurrentLanguage = setCurrentLanguage;
        service.getCurrentLanguage = getCurrentLanguage;
        service.getLocalizedString = getLocalizedString;

        function init() {
            return checkStringVersions()
                .then(invalidateLocalCache,
                function () { return $q.reject(); });

            //scheduleCacheInvalidation();
        }

        function checkStringVersions() {
            var deferred = $q.defer();

            $http({ method: "GET", url: Config.strings.versionsUri, cache: false })
                .success(function (data) {
                    deferred.resolve(data.versions);
                })
                .error(function() {
                    deferred.reject();
                });

            return deferred.promise;
        }

        function invalidateLocalCache(versions) {
            var deferred = $q.defer(),
                promises = [];

            versions.forEach(function(lang) {
                promises.push(checkInvalidateLocaleStrings(lang));
            });

            $q.all(promises)
                .then(function () {
                debugger;
                    removeUnusedStrings(versions);
                    deferred.resolve();
                }, function () {
                    debugger;
                    deferred.reject();
                });

            return deferred.promise;
        }

        function removeUnusedStrings(versions) {
            var unusedLocales = getUnusedLocales(versions);

            unusedLocales.forEach(function(lang) {
                removeStringsFromStorage(lang);
            });
        }

        function getUnusedLocales(versions) {
            var cachedLocales = getStoredLocales();
            var unusedLocales = cachedLocales.filter(function(el) {
                return !versions.filter(function(v) {
                    return v.locale === el;
                }).length;
            });

            return unusedLocales;
        }

        function checkInvalidateLocaleStrings(lang) {
            var cache = readStringsFromStorage(lang);

            if (!cache || cache.version !== lang.version) {
                return loadStringsForLocale(lang.locale);
            }
            return $q.resolve();
        }

        function setCurrentLanguage(lang) {
            language = lang;
            localStorageService.set(langStorageKey, lang);
            loadStrings(lang);
        }

        function getCurrentLanguage() {
            return language;
        }

        function loadStrings(lang) {
            var cache = dictionary[lang];
            if (!cache || Object.keys(cache.strings).length === 0) {
                cache = readStringsFromStorage(lang);
            }

            if (!!cache) {
                dictionary[lang] = cache;
            } else {
                ApplicationLogging.error('Failed reading strings from storage for language ' + lang + '!');
            }
        }

        function getLocalizedString(value) {
            var cache = dictionary[service.getCurrentLanguage()];
            if (cache && Object.keys(cache.strings).length > 0) {
                return cache.strings[value];
            }

            return '';
        }

        //function scheduleCacheInvalidation() {
        //    var deferred = $q.defer();

        //    var interval = Moment.duration(Config.strings.invalidationTimeout).asMilliseconds();
        //    $interval(function() { checkStringVersions(deferred); }, interval, 0, false);

        //    return deferred.promise;
        //}


        function loadStringsForLocale(locale) {
            return $http({ method: "GET", url: Config.strings.localizedUri + locale, cache: false })
                .success(function(data) {
                    saveStringsToStorage(locale, data);
                })
                .error(function () {
                debugger;
                    ApplicationLogging.error('Failed loading strings for language ' + locale + '!');
                });
        }

        function saveStringsToStorage(requestedLocale, data) {
            var strings = {};
            data.strings.forEach(function(el) {
                strings[el.key] = el.value;
            });

            var cache = {
                locale: data.locale,
                language: requestedLocale,
                version: data.version,
                strings: strings
            };

            writeStringsToStorage(cache);

            $rootScope.$broadcast('stringsUpdated');

            if (requestedLocale !== data.locale) {
                ApplicationLogging.warn('No strings defined for language ' + requestedLocale + '! Locale ' + data.locale + ' used as fall-back.');
            }
        }

        function writeStringsToStorage(data) {
            addStoredLocales(data.locale);
            localStorageService.set(getStringsStorageKey(data.locale), angular.toJson(data));
        }

        function readStringsFromStorage(lang) {
            var cache = angular.fromJson(localStorageService.get(getStringsStorageKey(lang)));
            if (!!cache && Object.keys(cache.strings).length > 0) {
                return cache;
            }

            return null;
        }

        function removeStringsFromStorage(lang) {
            localStorageService.remove(getStringsStorageKey(lang));
            removeStoredLocales(lang);
        }

        function getStoredLocales() {
            return angular.fromJson(localStorageService.get(getLocaleListStorageKey()))
                || [];
        }

        function addStoredLocales(locale) {
            var locales = getStoredLocales();

            if (locales.indexOf(locale) === -1) {
                locales.push(locale);
                localStorageService.set(getLocaleListStorageKey(), angular.toJson(locales));
            }
        }

        function removeStoredLocales(locale) {
            var locales = getStoredLocales();

            var index = locales.indexOf(locale);
            if (~index) {
                locales.splice(index, 1);
                localStorageService.set(getLocaleListStorageKey(), angular.toJson(locales));
            }
        }

        function getStringsStorageKey(locale) {
            return 'strings-' + locale;
        }

        function getLocaleListStorageKey() {
            return 'strings-localeList';
        }
    }
})();