﻿if (!String.prototype.format) {
    String.prototype.format = function () {
        var args = arguments;
        return this.replace(/{(\d+)}/g, function (match, number) {
            return typeof args[number] != 'undefined'
              ? args[number]
              : match
            ;
        });
    };
}

if (!String.prototype.formatNamed) {
    String.prototype.formatNamed = function (nameValueDict) {
        return this.replace(/{([a-z0-9_\-]+)}/gi, function (match, key) {
            var val = nameValueDict[key]
                || nameValueDict[key.charAt(0).toUpperCase() + key.slice(1)]
                || nameValueDict[key.charAt(0).toLowerCase() + key.slice(1)];
            return val != 'undefined'
              ? val
              : match
            ;
        });
    };
}