﻿(function() {
    'use strict';

    angular.module('np.i18n')
        .service('Strings', stringsService);

    stringsService.$injector = [
        '$q', '$http', '$rootScope', '$window', '$interval', 'localStorageService', 'Config', 'ApplicationLogging', 'Moment', 'MessageBus'
    ];

    function stringsService($q, $http, $rootScope, $window, $interval, localStorageService, Config, ApplicationLogging, Moment, MessageBus) {
        var service = this,
            dictionary = {},
            langStorageKey = 'currentLang',
            language = localStorageService.get(langStorageKey)
                || ($window.navigator.userLanguage || $window.navigator.language).split('-')[0];

        service.init = init;
        service.setCurrentLanguage = setCurrentLanguage;
        service.getCurrentLanguage = getCurrentLanguage;
        service.getLocalizedString = getLocalizedString;

        debugger;
        MessageBus.addListener('bla', alert1);
        MessageBus.addListener('bla', alert1);
        MessageBus.addListener('bla', alert2);

        MessageBus.removeListener('bla', alert1);

        MessageBus.publishEvent('bla', null);


        function alert1() {
            alert('1');
        }

        function alert2() {
            alert('2');
        }

        function init() {
            var promise = initStringResources();
            scheduleCacheInvalidation();

            return promise;
        }

        function initStringResources() {
            return checkStringVersions()
                .then(invalidateLocalCache);
        }

        function checkStringVersions() {
            var deferred = $q.defer();

            $http({ method: "GET", url: Config.strings.versionsUri, cache: false })
                .success(function(data) { deferred.resolve(data.versions); })
                .error(deferred.reject);

            return deferred.promise;
        }

        function invalidateLocalCache(versions) {
            var deferred = $q.defer(),
                promises = [];

            versions.forEach(function(ver) {
                promises.push(checkInvalidateLocaleStrings(ver));
            });

            $q.all(promises)
                .then(function() {
                        removeUnusedStrings(versions);
                        deferred.resolve();
                    },
                    deferred.reject);

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

        function checkInvalidateLocaleStrings(version) {
            var cache = readStringsFromStorage(version.locale);

            if (!cache || cache.version !== version.version) {
                return loadStringsForLocale(version.locale);
            }
            return $q.when();
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

        function scheduleCacheInvalidation() {
            var interval = Moment.duration(Config.strings.invalidationTimeout).asMilliseconds();
            $interval(initStringResources, interval, 0, false);
        }


        function loadStringsForLocale(locale) {
            return $http({ method: "GET", url: Config.strings.localizedUri + locale, cache: false })
                .success(function(data) {
                    saveStringsToStorage(locale, data);
                })
                .error(function() {
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