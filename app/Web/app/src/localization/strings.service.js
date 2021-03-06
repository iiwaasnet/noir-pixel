﻿(function() {
    'use strict';

    angular.module('np.i18n')
        .service('Strings', stringsService);

    stringsService.$inject = [
        '$q', '$http', '$rootScope', '$window', '$interval', 'Storage', 'Config', 'ApplicationLogging', 'Moment'
    ];

    function stringsService($q, $http, $rootScope, $window, $interval, Storage, Config, ApplicationLogging, Moment) {
        var service = this,
            EAPI_Unknown = 'EAPI_Unknown',
            MAPI_Fallback = 'MAPI_Fallback',
            dictionary = {},
            langStorageKey = 'curUILang',
            fallbackLang = 'en',
            uiLanguage = Storage.get(langStorageKey)
                || ($window.navigator.userLanguage || $window.navigator.language).split('-')[0],
            strLanguage = uiLanguage;

        service.init = init;
        service.setCurrentLanguage = setCurrentLanguage;
        service.getCurrentLanguage = getCurrentLanguage;
        service.getLocalizedString = getLocalizedString;
        service.getLocalizedErrorObject = getLocalizedErrorObject;
        service.getLocalizedMessageObject = getLocalizedMessageObject;
        service.getLocalizedMessage = getLocalizedMessage;

        function init() {
            var promise = initStringResources();
            scheduleCacheInvalidation();

            return promise;
        }

        function getLocalizedMessage(msgCode, placeholders, fallbackMsgCode) {
            var obj = createLocalizedMessageObject({ main: { code: msgCode } }, placeholders, fallbackMsgCode, MAPI_Fallback);

            return obj.main;
        }

        function getLocalizedMessageObject(obj, placeholders, fallbackMsgCode) {
            return createLocalizedMessageObject(obj, placeholders, fallbackMsgCode, MAPI_Fallback);
        }

        function getLocalizedErrorObject(obj, placeholders, fallbackErrCode) {
            return createLocalizedMessageObject(obj, placeholders, fallbackErrCode, EAPI_Unknown);
        }

        function createLocalizedMessageObject(obj, placeholders, fallbackMsgCode, lastResort) {
            var tmp = {};
            if (obj) {
                if (obj.main) {
                    tmp.main = obj.main.message ? obj.main.message : getLocalizedString(obj.main.code);
                    if (placeholders) {
                        tmp.main = tmp.main.formatNamed(placeholders);
                    }
                    if (obj.aux) {
                        tmp.aux = obj.aux.message ? obj.aux.message : getLocalizedString(obj.aux.code);
                    }
                }
            }
            if (!tmp.main) {
                tmp.main = getLocalizedString(fallbackMsgCode) || getLocalizedString(lastResort);
            }

            return tmp;
        }

        function setCurrentLanguage(lang) {
            uiLanguage = strLanguage = lang;
            Storage.set(langStorageKey, uiLanguage);

            if (!loadStrings(lang)) {
                strLanguage = fallbackLang;
                loadStrings(strLanguage);
            }
        }

        function getCurrentLanguage() {
            return strLanguage;
        }

        function getLocalizedString(value) {
            var cache = dictionary[service.getCurrentLanguage()];
            if (value && cache && Object.keys(cache.strings).length > 0) {
                return cache.strings[value];
            }

            return '';
        }

        function initStringResources() {
            return checkStringVersions()
                .then(invalidateLocalCache);
        }

        function checkStringVersions() {
            var deferred = $q.defer();

            $http({ method: "GET", url: Config.Strings.VersionsUri, cache: false })
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

        function loadStrings(lang) {
            var cache = dictionary[lang];
            if (!cache || Object.keys(cache.strings).length === 0) {
                cache = readStringsFromStorage(lang);
            }

            var found = !!cache;

            if (found) {
                dictionary[lang] = cache;
            } else {
                ApplicationLogging.error('Failed reading strings from storage for language ' + lang + '!');
            }

            return found;
        }
       
        function scheduleCacheInvalidation() {
            var interval = Moment.duration(Config.Strings.InvalidationTimeout).asMilliseconds();
            $interval(initStringResources, interval, 0, false);
        }


        function loadStringsForLocale(locale) {
            return $http({ method: "GET", url: Config.Strings.LocalizedUri + locale, cache: false })
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
            Storage.set(getStringsStorageKey(data.locale), angular.toJson(data));
        }

        function readStringsFromStorage(lang) {
            var cache = angular.fromJson(Storage.get(getStringsStorageKey(lang)));
            if (!!cache && Object.keys(cache.strings).length > 0) {
                return cache;
            }

            return null;
        }

        function removeStringsFromStorage(lang) {
            Storage.remove(getStringsStorageKey(lang));
            removeStoredLocales(lang);
        }

        function getStoredLocales() {
            return angular.fromJson(Storage.get(getLocaleListStorageKey()))
                || [];
        }

        function addStoredLocales(locale) {
            var locales = getStoredLocales();

            if (locales.indexOf(locale) === -1) {
                locales.push(locale);
                Storage.set(getLocaleListStorageKey(), angular.toJson(locales));
            }
        }

        function removeStoredLocales(locale) {
            var locales = getStoredLocales();

            var index = locales.indexOf(locale);
            if (~index) {
                locales.splice(index, 1);
                Storage.set(getLocaleListStorageKey(), angular.toJson(locales));
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