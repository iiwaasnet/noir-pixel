if (!Array.prototype.first) {
    Array.prototype.first = function(predicate) {
        'use strict';

        for (var i = 0; i < this.length; i++) {
            var current = this[i];
            if (predicate(current)) {
                return current;
            }
        }

        return undefined;
    };
}

if (!Array.prototype.any) {
    Array.prototype.any = function (predicate) {
        'use strict';

        return this.first(predicate) != undefined;
    };
}